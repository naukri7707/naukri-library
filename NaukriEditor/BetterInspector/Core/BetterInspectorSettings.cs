using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using Naukri.Singleton;
using NaukriEditor.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultInspector, AssetPath("Naukri/Editor/BetterInspectorSettings")]
public class BetterInspectorSettings : SingletonAsset<BetterInspectorSettings>
{
    public bool displayFields = true;
    
    public bool displayProperties;
    
    public bool displayMethods;
}