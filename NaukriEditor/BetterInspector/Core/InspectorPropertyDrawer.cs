using System.Reflection;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace NaukriEditor.BetterInspector.Core
{
    public abstract class InspectorPropertyDrawer : InspectorMemberDrawer
    {
        public PropertyInfo PropertyInfo => memberInfo as PropertyInfo;

        public dynamic Value
        {
            get
            {
                if (!PropertyInfo.CanRead)
                {
                    throw new UnityException($"{Target.GetType()}.{PropertyInfo.Name} 沒有 getter");
                }
                return PropertyInfo.GetValue(Target);
            }
            set
            {
                if (!PropertyInfo.CanWrite)
                {
                    throw new UnityException($"{Target.GetType()}.{PropertyInfo.Name} 沒有 setter");
                }
                PropertyInfo.SetValue(Target, value);
            }
        }
    }
}