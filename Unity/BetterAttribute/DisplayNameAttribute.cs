using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNameAttribute : PropertyAttribute
{
    public readonly string name;

    public DisplayNameAttribute(string name) => this.name = name;
}