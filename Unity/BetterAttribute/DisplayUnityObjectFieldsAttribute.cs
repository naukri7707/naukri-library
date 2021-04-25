using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUnityObjectFieldsAttribute : PropertyAttribute
{
    public string name;

    public bool defaultExpanded = false;

    public bool skipScriptField = false;

    public string[] skipFieldNames = new string[0];
}