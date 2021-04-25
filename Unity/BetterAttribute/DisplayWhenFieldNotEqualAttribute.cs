using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWhenFieldNotEqualAttribute : DisplayWhenFieldEqualAttribute
{
    public DisplayWhenFieldNotEqualAttribute(string fieldName, object value) : base(fieldName, value) { }
}