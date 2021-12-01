using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Naukri.CSV
{
    public sealed class CSVTable : IEnumerable<CSVSection>
    {
        private readonly List<CSVSection> sections = new List<CSVSection>();

        private readonly Encoding encoding;

        public ICSVColumn[] Columns { get; }

        public CSVSection this[int index] => sections[index];

        public CSVSection this[string key] => sections.Find(it => it.Key == key);

        public CSVTable(params ICSVColumn[] columns) : this(Encoding.Unicode, columns) { }

        public CSVTable(Encoding encoding, params ICSVColumn[] columns)
        {
            this.encoding = encoding;
            Columns = columns.Select(c => c.ShallowCopy()).ToArray();
        }

        public void AddSection(params CSVSection[] sections)
        {
            foreach (var section in sections)
            {
                this.sections.Add(section);
            }
        }

        private void ValidateSection(CSVSection section)
        {
            if (sections.Any(it => it.Key == section.Key))
            {
                throw new ArgumentException($"Key '{section.Key}' 已存在於 Table 中");
            }
            if (section.ColumnCount != Columns.Length)
            {
                throw new ArgumentException($"{nameof(section)}.{nameof(section.ColumnCount)} 與 {nameof(Columns)} 長度不匹配");
            }
        }

        public T GetSection<T>(string key) where T : CSVSection
        {
            return this[key] as T;
        }

        public T GetSection<T>(int index) where T : CSVSection
        {
            return sections[index] as T;
        }

        public void AddSection(CSVSection section)
        {
            ValidateSection(section);
            sections.Add(section);
        }

        public void InsertSection(int index, CSVSection section)
        {
            ValidateSection(section);
            sections.Insert(index, section);
        }

        public bool RemoveSection(CSVSection section)
        {
            return sections.Remove(section);
        }

        public void RemoveSectionAt(int index)
        {
            sections.RemoveAt(index);
        }

        public void Clear()
        {
            sections.Clear();
        }

        public void SaveTable(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.OpenOrCreate);
            using (var writer = new StreamWriter(stream, encoding))
            {
                writer.WriteLine(ColumnNameRowText());
                foreach (var section in sections)
                {
                    section.OnSerialize(this, writer);
                }
            }
        }

        public bool LoadTable(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            using (var reader = new StreamReader(filePath, encoding))
            {
                reader.ReadLine();
                foreach (var section in sections)
                {
                    section.OnDeserialize(this, reader);
                }
            }
            return true;
        }

        private string ColumnNameRowText()
        {
            var sb = new StringBuilder();
            foreach (var column in Columns)
            {
                sb.Append(column.Name).Append(',');
            }
            sb.Length--;
            return sb.ToString();
        }


        public IEnumerator<CSVSection> GetEnumerator() => sections.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}