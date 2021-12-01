using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangeDrawer : BetterPropertyDrawer
    {
        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                const float valueFieldWidth = 50F;
                const float controlSpaceWidth = 4.5F;
                var attr = attribute as MinMaxRangeAttribute;
                var limit = (attr.limitFieldName != null) switch
                {
                    true => property.serializedObject.FindProperty(attr.limitFieldName).vector2Value,
                    false => new Vector2(attr.min, attr.max),
                };
                var value = property.vector2Value;

                yield return BetterGUILayout.Wrapper(() =>
                {
                    var controlRect = EditorGUI.PrefixLabel(position, label);
                    var minRect = new Rect(controlRect) { width = valueFieldWidth };
                    var maxRect = new Rect(controlRect) { x = position.xMax - valueFieldWidth, width = valueFieldWidth };
                    var sliderRect = new Rect(controlRect) { xMin = minRect.xMax + controlSpaceWidth, xMax = maxRect.xMin - controlSpaceWidth };
                    value.x = EditorGUI.FloatField(minRect, value.x);
                    value.y = EditorGUI.FloatField(maxRect, value.y);
                    EditorGUI.MinMaxSlider(
                        sliderRect, GUIContent.none,
                        ref value.x, ref value.y,
                        limit.x, limit.y
                        );
                });


                property.vector2Value = value;
            }
            else
            {
                throw new System.TypeAccessException($"{nameof(MinMaxRangeAttribute)} 只能繪製 {nameof(Vector2)}");
            }
        }
    }
}