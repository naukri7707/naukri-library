using Naukri.BetterInspector;
using NaukriEditor.BetterInspector.Core;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaukriEditor.BetterInspector
{
    [CustomInspectorDrawer(typeof(DisplayFieldAttribute), true)]
    public class DisplayFieldDrawer : InspectorPropertyDrawer
    {
        public override bool OnGUILayout()
        {
            TypeCode propertyTypeCode = Type.GetTypeCode(PropertyInfo.PropertyType);
            switch (propertyTypeCode)
            {
                case TypeCode.Empty:
                    break;
                case TypeCode.Object:
                    
                    if (PropertyInfo.PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
                    {
                        AutoReadOnly(() => EditorGUILayout.ObjectField(PropertyInfo.Name, Value, PropertyInfo.PropertyType, true));
                        return true;
                    }
                    return false;
                case TypeCode.DBNull:
                    break;
                // bool
                case TypeCode.Boolean:
                    AutoReadOnly(() => EditorGUILayout.Toggle(PropertyInfo.Name, Value));
                    return true;
                // char
                case TypeCode.Char:
                    AutoReadOnly(() =>
                    {
                        string value = EditorGUILayout.TextField(PropertyInfo.Name, Value.ToString());
                        return value[0];
                    });
                    return true;
                // byte
                case TypeCode.SByte:
                case TypeCode.Byte:
                    AutoReadOnly(() =>
                    {
                        int value = EditorGUILayout.IntField(PropertyInfo.Name, Value);
                        if (propertyTypeCode is TypeCode.SByte)
                            return (sbyte)Mathf.Clamp(value, sbyte.MinValue, sbyte.MaxValue);
                        else
                            return (byte)Mathf.Clamp(value, byte.MinValue, byte.MaxValue);
                    });
                    return true;
                // short
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    AutoReadOnly(() =>
                    {
                        int value = EditorGUILayout.IntField(PropertyInfo.Name, Value);
                        if (propertyTypeCode is TypeCode.Int16)
                            return (short)Mathf.Clamp(value, short.MinValue, short.MaxValue);
                        else
                            return (ushort)Mathf.Clamp(value, ushort.MinValue, ushort.MaxValue);
                    });
                    return true;
                // int
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    AutoReadOnly(() => EditorGUILayout.IntField(PropertyInfo.Name, Value));
                    return true;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    AutoReadOnly(() => EditorGUILayout.LongField(PropertyInfo.Name, Value));
                    return true;
                case TypeCode.Single:
                    AutoReadOnly(() => EditorGUILayout.FloatField(PropertyInfo.Name, Value));
                    return true;
                case TypeCode.Double:
                    AutoReadOnly(() => EditorGUILayout.DoubleField(PropertyInfo.Name, Value));
                    return true;
                case TypeCode.Decimal:
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.TextField(PropertyInfo.Name, Value.ToString());
                    }
                    break;
                case TypeCode.DateTime:
                    using (new EditorGUI.DisabledScope(true))
                    {
                        EditorGUILayout.TextField(PropertyInfo.Name, Value.ToString());
                    }
                    break;
                case TypeCode.String:
                    AutoReadOnly(() => EditorGUILayout.TextField(PropertyInfo.Name, Value.ToString()));
                    break;
                default:
                    break;
            }
            return false;
        }

        private void AutoReadOnly(Func<object> drawField)
        {
            if (PropertyInfo.CanWrite)
            {
                Value = drawField.Invoke();
            }
            else
            {
                using (new EditorGUI.DisabledScope(PropertyInfo.CanWrite))
                {
                    drawField.Invoke();
                }
            }
        }
    }
}
