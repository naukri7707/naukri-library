using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using System;
using UnityEngine;


namespace Naukri.CSV
{
    public class CSVEmptyColumn : CSVColumn<CSVEmptyColumn.EmptyData>
    {
        public struct EmptyData { }

        public readonly string trueText = true.ToString();

        public readonly string falseText = false.ToString();

        public CSVEmptyColumn(string key) : base(key) { }

        public CSVEmptyColumn(string key, string name) : base(name) { }

        public override EmptyData Deserialize(string data) => default(EmptyData);

        public override string Serialize(EmptyData obj) => "";
    }

    public class CSVBoolColumn : CSVColumn<bool>
    {
        public readonly string trueText = true.ToString();

        public readonly string falseText = false.ToString();

        public CSVBoolColumn(string key) : base(key) { }

        public CSVBoolColumn(string key, string name) : base(name) { }

        public CSVBoolColumn(string key, string name, string trueText, string falseText) : base(key, name)
        {
            this.trueText = trueText;
            this.falseText = falseText;
        }

        public override bool Deserialize(string data)
        {
            return data.Equals(trueText)
                ? true
                : data.Equals(falseText)
                ? false
                : throw new FormatException();
        }

        public override string Serialize(bool obj)
        {
            return obj
                ? trueText
                : falseText;
        }
    }

    public class CSVIntColumn : CSVColumn<int>
    {
        public CSVIntColumn(string key) : base(key) { }

        public CSVIntColumn(string key, string name) : base(key, name) { }

        public override int Deserialize(string data)
        {
            return int.Parse(data);
        }

        public override string Serialize(int obj)
        {
            return obj.ToString();
        }
    }

    public class CSVFloatColumn : CSVColumn<float>
    {
        public CSVFloatColumn(string key) : base(key) { }

        public CSVFloatColumn(string key, string name) : base(key, name) { }

        public override float Deserialize(string data)
        {
            return float.Parse(data);
        }

        public override string Serialize(float obj)
        {
            return obj.ToString();
        }
    }

    public class CSVTextColumn : CSVColumn<string>
    {
        public CSVTextColumn(string key) : base(key) { }

        public CSVTextColumn(string key, string name) : base(key, name) { }

        public override string Deserialize(string data)
        {
            return string.IsNullOrEmpty(data)
                ? null
                : data.Substring(1, data.Length - 2);
        }

        public override string Serialize(string obj)
        {
            return $"\"{obj}\"";
        }
    }
}