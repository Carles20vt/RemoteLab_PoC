#if UNITY_EDITOR
using System.Collections.Generic;
using TreeislandStudio.Engine.Attributes;
using UnityEditor;

namespace TreeislandStudio.Engine
{
    /// <summary>
    /// Specialized editor for ThreeLightsBehaviour
    /// </summary>
    [CustomEditor(typeof(TreeislandBehaviour), true)]
// ReSharper disable once CheckNamespace
    public class TreeislandBehaviourEditor : Editor {

        protected TreeislandBehaviour MInstance;
        protected PropertyField[] MFields;
        protected List<string> ExcludedProperties = new List<string>();

        public virtual void OnEnable()
        {
            MInstance = target as TreeislandBehaviour;
            MFields = ExposeProperties.GetProperties(MInstance);
            OnSetExcludedProperties();
        }

        protected virtual void OnSetExcludedProperties()
        {
            if (MInstance == null) {
                return;
            }

            ExcludedProperties = new List<string> { "m_Script" };
        }

        public override void OnInspectorGUI()
        {
            if (MInstance == null)
                return;

            OnSetExcludedProperties();

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, ExcludedProperties.ToArray());

            ExposeProperties.Expose(MInstance, MFields, ExcludedProperties);

            InsideInspectorGui();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void InsideInspectorGui()
        {
        }
    }
}
#endif