using System.Collections;
using System.Text;
using System.Text.RegularExpressions;


namespace Naukri.CSV
{
    public sealed class CSVRow : IEnumerable
    {
        private readonly object[] data;

        public int Length => data.Length;

        public CSVRow(int column)
        {
            data = new object[column];
        }

        public object this[int column]
        {
            get => data[column];
            set => data[column] = value;
        }

        public string OnSerialize(CSVTable table)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                var serial = table.Columns[i].Serialize(data[i]);
                sb.Append(serial).Append(',');
            }
            sb.Length--;
            return sb.ToString();
        }

        public void OnDeserialize(CSVTable table, string rowData)
        {
            var regex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");

            var idx = -1;
            foreach (Match match in regex.Matches(rowData))
            {
                idx++;
                var serial = match.Value;
                data[idx] = table.Columns[idx].Deserialize(serial);
            }
        }

        public IEnumerator GetEnumerator() => data.GetEnumerator();
    }
}