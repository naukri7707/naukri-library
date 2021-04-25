using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWhenFieldEqualAttribute : PropertyAttribute
{
    public readonly string fieldName;

    public readonly object value;

    public DisplayWhenFieldEqualAttribute(string fieldName, object value)
    {
        this.fieldName = fieldName;
        this.value = value;
    }
}
