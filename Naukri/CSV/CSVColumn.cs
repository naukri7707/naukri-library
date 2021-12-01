namespace Naukri.CSV
{
    public interface ICSVColumn
    {
        public string Key { get; }

        public string Name { get; }

        public string Serialize(object obj);

        public object Deserialize(string data);

        public ICSVColumn ShallowCopy();
    }

    public abstract class CSVColumn<T> : ICSVColumn
    {
        public string Key { get; }

        public string Name { get; }

        public virtual string Empty => "";

        public CSVColumn(string key)
        {
            Key = key;
            Name = key;
        }

        public CSVColumn(string key, string name)
        {
            Key = key;
            Name = name;
        }

        public abstract string Serialize(T obj);

        public abstract T Deserialize(string data);

        string ICSVColumn.Serialize(object obj)
        {
            return obj is null
                ? Empty
                : Serialize((T)obj);
        }

        object ICSVColumn.Deserialize(string data)
        {
            return Deserialize(data);
        }

        public virtual ICSVColumn ShallowCopy()
        {
            return (ICSVColumn)MemberwiseClone();
        }
    }
}