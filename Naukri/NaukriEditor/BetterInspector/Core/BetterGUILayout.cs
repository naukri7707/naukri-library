using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector.Core
{
    public static class BetterGUILayout
    {
        public class IndentScope : GUI.Scope
        {
            private readonly int indentLevel = 0;

            public IndentScope(int indentLevel)
            {
                this.indentLevel = indentLevel;
                EditorGUI.indentLevel += indentLevel;
            }

            protected override void CloseScope()
            {
                EditorGUI.indentLevel -= indentLevel;
            }
        }

        public static Rect LableSeparator(string label, float thickness = 1.6F, float padleft = 30F, int indent = 0)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.xMin += indent * 14F;
                var leftRect = new Rect(rect)
                {
                    width = padleft,
                    y = (rect.yMin + rect.yMax) / 2,
                    height = thickness
                };
                EditorGUI.DrawRect(leftRect, new Color(0.5F, 0.5F, 0.5F, 1));
                var labelRect = new Rect(rect)
                {
                    xMin = leftRect.xMax + 2,
                    width = EditorStyles.label.CalcSize(new GUIContent(label)).x + 4
                };
                EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);
                var rightRect = new Rect(rect)
                {
                    xMin = labelRect.xMax + 2,
                    y = leftRect.y,
                    height = leftRect.height
                };
                EditorGUI.DrawRect(rightRect, new Color(0.5F, 0.5F, 0.5F, 1));
                return rect;
            }
        }

        public static bool AutoField<T>(GUIContent label, dynamic value, out object result)
        {
            return AutoField(typeof(T), label, value, out result);
        }

        public static bool AutoField(Type valueType, GUIContent label, dynamic value, out object result)
        {
            TypeCode typeCode = Type.GetTypeCode(valueType);
            if (IsIntegerType(typeCode)) // 處理 Enum 型態
            {
                if (valueType.IsEnum)
                {
                    result = valueType.GetCustomAttribute<FlagsAttribute>() is null
                           ? EditorGUILayout.EnumPopup(label, value)
                           : EditorGUILayout.EnumFlagsField(label, value);
                    return true;
                }
            }

            switch (typeCode)
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:

                    if (valueType.IsSubclassOf(typeof(UnityEngine.Object)))
                    {
                        result = EditorGUILayout.ObjectField(label, value, valueType, true);
                        return true;
                    }
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.Boolean:
                    result = EditorGUILayout.Toggle(label, value);
                    return true;
                case TypeCode.Char:
                    result = EditorGUILayout.TextField(label, value)[0];
                    return true;
                case TypeCode.SByte:
                    result = (sbyte)Mathf.Clamp(
                        EditorGUILayout.IntField(label, value),
                        sbyte.MinValue,
                        sbyte.MaxValue
                        );
                    return true;
                case TypeCode.Byte:
                    result = (byte)Mathf.Clamp(
                        EditorGUILayout.IntField(label, value),
                        byte.MinValue,
                        byte.MaxValue
                        );
                    return true;
                case TypeCode.Int16:
                    result = (short)Mathf.Clamp(
                        EditorGUILayout.IntField(label, value),
                        short.MinValue,
                        short.MaxValue
                        );
                    return true;
                case TypeCode.UInt16:
                    result = (ushort)Mathf.Clamp(
                        EditorGUILayout.IntField(label, value),
                        ushort.MinValue,
                        ushort.MaxValue
                        );
                    return true;
                case TypeCode.Int32:
                    result = EditorGUILayout.IntField(label, value);
                    return true;
                case TypeCode.UInt32:
                    result = (uint)Mathf.Clamp(
                        EditorGUILayout.LongField(label, value),
                        uint.MinValue,
                        uint.MaxValue
                        );
                    return true;
                case TypeCode.Int64:
                    result = EditorGUILayout.LongField(label, value);
                    return true;
                case TypeCode.UInt64:
                    result = (ulong)Mathf.Clamp(
                       EditorGUILayout.LongField(label, value),
                       ulong.MinValue,
                       ulong.MaxValue
                       );
                    return true;
                case TypeCode.Single:
                    result = EditorGUILayout.FloatField(label, value);
                    return true;
                case TypeCode.Double:
                    result = EditorGUILayout.DoubleField(label, value);
                    return true;
                case TypeCode.Decimal:
                    if (decimal.TryParse(EditorGUILayout.TextField(label, value.ToString()), out decimal dec))
                    {
                        result = dec;
                        return true;
                    }
                    result = value;
                    return true;
                case TypeCode.DateTime:
                    if (DateTime.TryParse(EditorGUILayout.TextField(label, value.ToString()), out DateTime date))
                    {
                        result = date;
                        return true;
                    }
                    result = value;
                    return true;
                case TypeCode.String:
                    result = EditorGUILayout.TextField(label, value.ToString());
                    return true;
                default:
                    break;
            }
            result = null;
            return false;
        }

        private static bool IsIntegerType(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetDispalyName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            var res = new StringBuilder();
            res.Append(char.ToUpper(name[0]));
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && !char.IsUpper(name[i - 1])) // 大寫且前一個字元不是大寫
                {
                    res.Append(" ");
                }
                res.Append(name[i]);
            }
            return res.ToString();
        }
    }
}
