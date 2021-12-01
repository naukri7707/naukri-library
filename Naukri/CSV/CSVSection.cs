using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace Naukri.CSV
{
    public class CSVSection : IEnumerable<CSVRow>
    {
        public string Key { get; }

        protected readonly List<CSVRow> rows;

        public int RowCount => rows.Count;

        public int ColumnCount { get; }

        protected CSVSection() { }

        public CSVSection(string key, int columnCount, int rowCount = 4)
        {
            Key = key;
            ColumnCount = columnCount;
            rows = new List<CSVRow>(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                rows.Add(NewRow());
            }
        }

        public CSVRow this[int row]
        {
            get => rows[row];
            set => rows[row] = value;
        }

        public virtual CSVRow NewRow() => new CSVRow(ColumnCount);

        public void AddRow()
        {
            rows.Add(NewRow());
        }

        public void InsertRow(int index)
        {
            rows.Insert(index, NewRow());
        }

        public bool RemoveRow(CSVRow row)
        {
            return rows.Remove(row);
        }

        public void RemoveRowAt(int index)
        {
            rows.RemoveAt(index);
        }

        public void Clear()
        {
            rows.Clear();
        }

        public virtual void OnSerialize(CSVTable table, StreamWriter writer)
        {
            foreach (var row in rows)
            {
                var rowText = row.OnSerialize(table);
                writer.WriteLine(rowText);
            }
        }

        public virtual void OnDeserialize(CSVTable table, StreamReader reader)
        {
            foreach (var row in rows)
            {
                var rowText = reader.ReadLine();
                row.OnDeserialize(table, rowText);
            }
        }

        public IEnumerator<CSVRow> GetEnumerator() => rows.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}