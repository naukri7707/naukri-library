namespace Naukri.Unity.BetterAttribute
{
    public class DisplayWhenFieldNotEqualAttribute : DisplayWhenFieldEqualAttribute
    {
        public DisplayWhenFieldNotEqualAttribute(string fieldName, object value) : base(fieldName, value) { }
    }
}