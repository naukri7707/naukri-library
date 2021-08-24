namespace Naukri.BetterAttribute
{
    public class DisplayWhenFieldNotEqualAttribute : DisplayWhenFieldEqualAttribute
    {
        public DisplayWhenFieldNotEqualAttribute(string fieldName, params object[] values) : base(fieldName, values) { }
    }
}