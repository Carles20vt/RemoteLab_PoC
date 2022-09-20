namespace TreeislandStudio.Engine.Attributes
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

//Original by DYLAN ENGELMAN http://jupiterlighthousestudio.com/custom-inspectors-unity/
//Altered by Brecht Lecluyse https://www.brechtos.com

    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                if (attribute is TagSelectorAttribute attrib && attrib.UseDefaultTagFieldDrawer)
                {
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                }
                else
                {
                    //generate the taglist + custom tags
                    var tagList = new List<string> {"<NoTag>"};
                    tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                    var propertyString = property.stringValue;
                    var index = -1;
                    
                    if (propertyString == "")
                    {
                        //The tag is empty
                        index = 0; //first index is the special <notag> entry
                    }
                    else
                    {
                        //check if there is an entry that matches the entry and get the index
                        //we skip index 0 as that is a special custom case
                        for (var i = 1; i < tagList.Count; i++)
                        {
                            if (tagList[i] != propertyString)
                            {
                                continue;
                            }
                            
                            index = i;
                            break;
                        }
                    }

                    //Draw the popup box with the current selected index
                    index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());

                    property.stringValue = index switch
                    {
                        //Adjust the actual string value of the property based on the selection
                        0 => "",
                        >= 1 => tagList[index],
                        _ => ""
                    };
                }

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
#endif
}