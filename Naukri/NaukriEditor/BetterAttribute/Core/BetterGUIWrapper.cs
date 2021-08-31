namespace NaukriEditor.BetterAttribute.Core
{
    public delegate void DrawEvent();

    public delegate void DrawResult<T>(T result);

    public struct BetterGUIWrapper
    {
        public float height;

        public DrawEvent drawEvent;

        public BetterGUIWrapper(DrawEvent drawEvent)
        {
            height = BetterGUILayout.PropertyHeight;
            this.drawEvent = drawEvent;
        }

        public BetterGUIWrapper(float height, DrawEvent drawEvent)
        {
            this.height = height;
            this.drawEvent = drawEvent;
        }

        public static implicit operator BetterGUIWrapper((float height, DrawEvent drawEvent) propertyField)
        {
            (var height, var drawEvent) = propertyField;
            return new BetterGUIWrapper(height, drawEvent);
        }
    }
}
