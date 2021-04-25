using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementNameAttribute : PropertyAttribute
{
    public readonly string name;

    public ElementNameAttribute(string name) => this.name = name;
}