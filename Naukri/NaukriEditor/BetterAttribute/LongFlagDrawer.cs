using Naukri;
using Naukri.BetterAttribute;
using NaukriEditor.BetterAttribute.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterAttribute
{
    [CustomPropertyDrawer(typeof(LongFlagAttribute))]
    public class LongFlagDrawer : BetterPropertyDrawer
    {
        private const string None = nameof(None);

        private const string Everything = nameof(Everything);

        private const string Mixed = nameof(Mixed);

        public override IEnumerable<BetterGUIWrapper> OnGUILayout(SerializedProperty property, GUIContent label, bool isOnGUI)
        {
            var attr = attribute as LongFlagAttribute;
            var underlyingType = Enum.GetUnderlyingType(attr.enumType);
            var signed = false;
            if (underlyingType == typeof(ulong))
            {
                signed = false;
            }
            else if (underlyingType == typeof(long))
            {
                signed = true;
            }
            else
            {
                yield break;
            }

            var enumValues = Utility.GetEnumValues(attr.enumType);
            var distinctEnumValues = Distinct(enumValues).ToArray();
            var controlLabel = ControlLabel(property.longValue, distinctEnumValues, signed);
            var isClick = false;
            yield return BetterGUILayout.ButtonField(res => isClick = res, label, new GUIContent(controlLabel), out var _, out var controlRect, EditorStyles.popup);
            if (isClick)
            {
                var menuRect = controlRect;
                menuRect.yMin = controlRect.yMax;
                ContextMenu(property, menuRect, distinctEnumValues, signed);
            }
        }

        private string ControlLabel(long flagValue, object[] enumValues, bool signed)
        {
            var res = "None";
            var isEverything = true;
            foreach (var enumValue in enumValues)
            {
                var name = enumValue.ToString();
                switch (name)
                {
                    case None:
                    case Everything:
                        continue;
                    default:
                        var longValue = Caster(enumValue, signed);
                        if ((flagValue & longValue) > 0)
                        {
                            if (res is Mixed) continue;
                            if (res is None)
                            {
                                res = name;
                            }
                            else
                            {
                                res = Mixed;
                            }
                        }
                        else
                        {
                            isEverything = false;
                        }
                        break;
                }
            }
            return isEverything ? Everything : res;
        }

        private void ContextMenu(SerializedProperty property, Rect menuRect, object[] enumValues, bool signed)
        {
            var menu = new GenericMenu();
            menuRect.size = Vector2.zero;

            var everythingFlag = 0L;
            var asa = property.longValue;
            // None
            menu.AddItem(
              new GUIContent(None),
              property.longValue == 0L,
              () =>
              {
                  property.longValue = 0L;
                  property.serializedObject.ApplyModifiedProperties();
              });
            // Flags
            foreach (var enumValue in enumValues)
            {
                switch (enumValue.ToString())
                {
                    case None:
                    case Everything:
                        continue;
                    default:
                        var longValue = Caster(enumValue, signed);
                        everythingFlag |= longValue;
                        menu.AddItem(
                            new GUIContent(enumValue.ToString()),
                            (property.longValue & longValue) != 0,
                            () =>
                            {
                                property.longValue ^= longValue;
                                property.serializedObject.ApplyModifiedProperties();
                            });
                        break;
                }
            }
            // Everything
            menu.AddItem(
            new GUIContent(Everything),
            property.longValue == everythingFlag,
            () =>
            {
                property.longValue = everythingFlag;
                property.serializedObject.ApplyModifiedProperties();
            });
            menu.DropDown(menuRect);
        }

        private IEnumerable<object> Distinct(Array array)
        {
            var set = new HashSet<object>();
            foreach (var item in array)
            {
                if (set.Add(item))
                {
                    yield return item;
                }
            }
        }

        private long Caster(object value, bool signed)
        {
            return signed  // op '?' 的回傳型態是固定的，所以簡化成會造成錯誤 (long)(singned ? enumValue : (ulong)enumValue)
                ? (long)value
                : (long)(ulong)value;
        }
    }
}