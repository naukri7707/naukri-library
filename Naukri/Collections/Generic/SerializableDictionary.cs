using System;
using System.Collections.Generic;
using UnityEngine;

namespace Naukri.Collections.Generic
{
    [Serializable]
    public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [Serializable]
        public struct KeyValuePair
        {
            public TKey key;
            public TValue value;
        }

        [SerializeField]
        private List<KeyValuePair> values = new List<KeyValuePair>();

        [SerializeField]
        private KeyValuePair newData;

        public SerializableDictionary() { newData = default; } // newData 的賦值為解決 unity 警告問題

        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        public void OnBeforeSerialize()
        {
            values.Clear();
            foreach (var pair in this)
            {
                values.Add(new KeyValuePair() { key = pair.Key, value = pair.Value });
            }
        }

        public void OnAfterDeserialize()
        {
            var isAddValue = values.Count > Count;
            Clear();
            // 如果有兩筆以上的資料，且末兩項資料相等，視為 Add 按鈕被按下，此時將newData 取代最末項資料以維護 Dictionary 的唯一性
            if (isAddValue)
            {
                values[values.Count - 1] = newData;
            }
            foreach (var item in values)
            {
                Add(item.key, item.value ?? default);
            }
        }
    }
}