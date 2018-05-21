// Adapted from https://gist.github.com/LotteMakesStuff/0de9be35044bab97cbe79b9ced695585

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif
using UnityEngine;

namespace UE.Attributes
{
    public class MinMaxAttribute : PropertyAttribute
    {
        public float MinLimit = 0;
        public float MaxLimit = 1;
        public bool ShowEditRange = true;
        public bool ShowDebugValues;
        public bool ClampValues = true;

//        public bool Inverted => MinLimit > MaxLimit;

        public MinMaxAttribute(int min, int max)
        {
            if (min > max)
            {
                //Inverted
                MinLimit = max;
                MaxLimit = min;
                
                return;
            }
            
            MinLimit = min;
            MaxLimit = max;
        }
    }
}

#if UNITY_EDITOR

namespace UE.Attributes
{
    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // cast the attribute to make life easier
            MinMaxAttribute minMax = attribute as MinMaxAttribute;

            // This only works on a vector2! ignore on any other property type (we should probably draw an error message instead!)
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                // if we are flagged to draw in a special mode, lets modify the drawing rectangle to draw only one line at a time
                if (minMax.ShowDebugValues || minMax.ShowEditRange)
                {
                    position = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
                }

                // pull out a bunch of helpful min/max values....
                // the currently set minimum and maximum value
                var minValue = property.vector2Value.x;
                var maxValue = property.vector2Value.y;
                // the limit for both min and max, min cant go lower than minLimit and max cant top maxLimit
                var minLimit = minMax.MinLimit;
                var maxLimit = minMax.MaxLimit;

                //Draw that min max slider
//                var sliderRect = new Rect(position.x + 15, position.y, position.width - 30, position.height);
//                EditorGUI.MinMaxSlider(sliderRect, label, ref minValue, ref maxValue, minLimit, maxLimit);
                EditorGUI.MinMaxSlider(position, label, ref minValue, ref maxValue, minLimit, maxLimit);

                //draw limits
//                var minLabelRect = new Rect(position.x, position.y, 15, position.height);
//                EditorGUI.LabelField(minLabelRect, minLimit.ToString("F1"));
//                
//                var maxLabelRect = new Rect(position.x + position.width - 15, position.y, 15, position.height);
//                EditorGUI.LabelField(maxLabelRect, maxLimit.ToString("F1"));


                var vec = Vector2.zero; // save the results into the property!
                vec.x = minValue;
                vec.y = maxValue;

                property.vector2Value = vec;

                // Do we have a special mode flagged? time to draw lines!
                if (!minMax.ShowDebugValues && !minMax.ShowEditRange) return;


                var isEditable = minMax.ShowEditRange;

                if (!isEditable)
                    // if were just in debug mode and not edit mode, make sure all the UI is read only!
                    GUI.enabled = false;

                // move the draw rect on by one line
                position.y += EditorGUIUtility.singleLineHeight;

                // shove the values and limits into a vector4 and draw them all at once
                var val = new Vector2(minValue, maxValue);
//                val = EditorGUI.Vector2Field(position, " ", val);
                val = EditorGUI.Vector2Field(position, "    " + minLimit + " - X - Y - " + maxLimit, val);

//                GUI.enabled = false; // the range part is always read only
//                position.y += EditorGUIUtility.singleLineHeight;
//                EditorGUI.FloatField(position, "Selected Range", maxValue - minValue);
//                GUI.enabled = true; // remember to make the UI editable again!

                if (!isEditable)
                    return;

                //clamp values
                if (minMax.ClampValues)
                {
                    val.x = Mathf.Clamp(val.x, minLimit, val.y);
                    val.y = Mathf.Clamp(val.y, val.x, maxLimit);
                }

                // save off any change to the value~                
                property.vector2Value = new Vector2(val.x, val.y);
            }
        }

        // this method lets unity know how big to draw the property. We need to override this because it could end up meing more than one line big
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var minMax = attribute as MinMaxAttribute;

            // by default just return the standard line height
            var size = EditorGUIUtility.singleLineHeight;

            // if we have a special mode, add two extra lines!
            if (minMax.ShowEditRange || minMax.ShowDebugValues)
            {
                size += EditorGUIUtility.singleLineHeight;
            }

            return size;
        }
    }
}
#endif