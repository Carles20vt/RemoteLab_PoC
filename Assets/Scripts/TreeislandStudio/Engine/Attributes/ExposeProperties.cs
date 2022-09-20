#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TreeislandStudio.Engine.Attributes
{
    // ReSharper disable CheckNamespace

    /// <summary>
    /// Based on http://wiki.unity3d.com/index.php?title=Expose_properties_in_inspector by http://wiki.unity3d.com/index.php/User:Mift
    /// </summary>
    public static class ExposeProperties {
        public class PropertyEditor { };
        public class NormalEditor : PropertyEditor { };
        public class RangeEditor : PropertyEditor {
            public float Minimum = 0f;
            public float Maximum = 1f;

            public RangeEditor(float minimum, float maximum)
            {
                Minimum = minimum;
                Maximum = maximum;
            }
        };

        /// <summary>
        /// Expose (show editor) for the given properties
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="properties"></param>
        /// <param name="excludedProperties"></param>
        /// <returns>Return whenever any of the properties has changed</returns>
        public static bool Expose(MonoBehaviour behaviour, PropertyField[] properties, List<String> excludedProperties)
        {
            var dirty = false;

            var emptyOptions = new GUILayoutOption[0];

            EditorGUILayout.BeginVertical(emptyOptions);

            foreach (var field in properties) {
                object newValue = null;
                EditorGUI.BeginChangeCheck();

                if (excludedProperties.Contains(field.Name)) {
                    continue;
                }

                EditorGUILayout.BeginHorizontal(emptyOptions);
                GUI.enabled = field.IsWritable();
                switch (field.Type) {
                    case SerializedPropertyType.Integer:
                        newValue = IntegerFieldEditor(emptyOptions, field);
                        break;

                    case SerializedPropertyType.Float:
                        newValue = FloatFieldEditor(emptyOptions, field);
                        break;

                    case SerializedPropertyType.Boolean:

                        newValue = EditorGUILayout.Toggle(field.Name, (bool) field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.String:
                        newValue = EditorGUILayout.TextField(field.Name, (string) field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.Vector2:
                        newValue = EditorGUILayout.Vector2Field(field.Name, (Vector2) field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.Vector3:
                        newValue = EditorGUILayout.Vector3Field(field.Name, (Vector3) field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.Enum:
                        newValue = EditorGUILayout.EnumPopup(field.Name, (Enum)field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.Color:
                        newValue = EditorGUILayout.ColorField(field.Name, (Color)field.GetValue(), emptyOptions);
                        break;

                    case SerializedPropertyType.ObjectReference:
                        newValue = EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object)field.GetValue(), field.GetPropertyType(), true, emptyOptions);
                        break;

                    default:
                        break;
                }

                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();

                if (!EditorGUI.EndChangeCheck()) continue;
            
                dirty = true;

                Debug.Log("Changed property " + field.Name + " of " + behaviour.GetType().Name + " ( " + behaviour.gameObject.name + ")");
                Undo.RecordObject(behaviour, "Changed property " + field.Name + " of " + behaviour.GetType().Name + " ( " + behaviour.gameObject.name + ")");
                field.SetValue(newValue);
            }

            EditorGUILayout.EndVertical();

            return dirty;
        }

        /// <summary>
        /// Float specialized editor, supporting ranges
        /// </summary>
        /// <param name="layoutOptions"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static object FloatFieldEditor(GUILayoutOption[] layoutOptions, PropertyField field)
        {
            if (field.specializedEditor is RangeEditor) {
                return EditorGUILayout.Slider(field.Name, (float)field.GetValue(), ((RangeEditor)field.specializedEditor).Minimum, ((RangeEditor)field.specializedEditor).Maximum, layoutOptions);
            }

            return EditorGUILayout.FloatField(field.Name, (float)field.GetValue(), layoutOptions);
        }

        /// <summary>
        /// Integer specialized editor, supporting ranges
        /// </summary>
        /// <param name="layoutOptions"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static object IntegerFieldEditor(GUILayoutOption[] layoutOptions, PropertyField field)
        {
            if (field.specializedEditor is RangeEditor) {
                return EditorGUILayout.IntSlider(
                    field.Name,
                    (int)field.GetValue(),
                    (int)((RangeEditor)field.specializedEditor).Minimum,
                    (int)((RangeEditor)field.specializedEditor).Maximum,
                    layoutOptions
                );
            }

            return EditorGUILayout.IntField(field.Name, (int)field.GetValue(), layoutOptions);
        }

        public static PropertyField[] GetProperties(System.Object obj)
        {
            var fields = new List<PropertyField>();
        
            if (obj == null)
            {
                return fields.ToArray();
            }

            var infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo info in infos) {

                if (!info.CanRead)
                    continue;

                object[] attributes = info.GetCustomAttributes(true);

                bool isExposed = false;

                PropertyEditor editor = new NormalEditor();
                foreach (object o in attributes) {
                    if (o.GetType() == typeof(ExposePropertyAttribute)) {
                        isExposed = true;
                    }
                    if (o.GetType() == typeof(ExposeRange)) {
                        editor = new RangeEditor(((ExposeRange)o).Minimum, ((ExposeRange)o).Maximum);
                    }
                }

                if (!isExposed)
                    continue;

                SerializedPropertyType type = SerializedPropertyType.Integer;

                if (PropertyField.GetPropertyType(info, out type)) {
                    PropertyField field = new PropertyField(obj, info, type, editor);
                    fields.Add(field);
                }

            }

            return fields.ToArray();

        }

    }

    public class PropertyField {
        System.Object m_Instance;
        PropertyInfo m_Info;
        SerializedPropertyType m_Type;
        public ExposeProperties.PropertyEditor specializedEditor;

        MethodInfo m_Getter;
        MethodInfo m_Setter;

        public SerializedPropertyType Type {
            get {
                return m_Type;
            }
        }

        public String Name {
            get {
                return ObjectNames.NicifyVariableName(m_Info.Name);
            }
        }

        public PropertyField(System.Object instance, PropertyInfo info, SerializedPropertyType type, ExposeProperties.PropertyEditor editor)
        {

            m_Instance = instance;
            m_Info = info;
            m_Type = type;
            specializedEditor = editor;

            m_Getter = m_Info.GetGetMethod();
            m_Setter = m_Info.GetSetMethod();
        }

        public System.Object GetValue()
        {
            return m_Getter.Invoke(m_Instance, null);
        }

        public void SetValue(System.Object value)
        {
            if (m_Setter == null) {
                return;
            }
            m_Setter.Invoke(m_Instance, new System.Object[] { value });
        }

        public bool IsWritable()
        {
            return m_Setter != null;
        }

        public Type GetPropertyType()
        {
            return m_Info.PropertyType;
        }

        public static bool GetPropertyType(PropertyInfo info, out SerializedPropertyType propertyType)
        {

            propertyType = SerializedPropertyType.Generic;

            Type type = info.PropertyType;

            if (type == typeof(int)) {
                propertyType = SerializedPropertyType.Integer;
                return true;
            }

            if (type == typeof(float)) {
                propertyType = SerializedPropertyType.Float;
                return true;
            }

            if (type == typeof(Color)) {
                propertyType = SerializedPropertyType.Color;
                return true;
            }

            if (type == typeof(bool)) {
                propertyType = SerializedPropertyType.Boolean;
                return true;
            }

            if (type == typeof(string)) {
                propertyType = SerializedPropertyType.String;
                return true;
            }

            if (type == typeof(Vector2)) {
                propertyType = SerializedPropertyType.Vector2;
                return true;
            }

            if (type == typeof(Vector3)) {
                propertyType = SerializedPropertyType.Vector3;
                return true;
            }

            if (type.IsEnum) {
                propertyType = SerializedPropertyType.Enum;
                return true;
            }
            // COMMENT OUT to NOT expose custom objects/types
            propertyType = SerializedPropertyType.ObjectReference;
            return true;

            //return false;
        }
    }
}
#endif