using System.Reflection;

namespace Naukri.Reflection
{
    public readonly struct FastProperty<TObject, TValue>
    {
        private readonly FastGetter<TObject, TValue> getter;

        private readonly FastSetter<TObject, TValue> setter;

        public FastProperty(PropertyInfo property)
        {
            getter = property.CreateFastGetter<TObject, TValue>();
            setter = property.CreateFastSetter<TObject, TValue>();
        }


        public TValue this[TObject obj]
        {
            get => GetValue(obj);
            set => SetValue(obj, value);
        }

        public TValue this[params TObject[] objs]
        {
            set
            {
                foreach (var obj in objs)
                {
                    SetValue(obj, value);
                }
            }
        }

        public TValue GetValue(TObject obj)
        {
            if (getter is null)
                throw new NaukriException("Getter Not Found");
            return getter(obj);
        }

        public void SetValue(TObject obj, TValue value)
        {
            if (getter is null)
                throw new NaukriException("Setter Not Found");
            setter(obj, value);
        }
    }
}