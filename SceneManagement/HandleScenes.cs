using Naukri;
using Naukri.BetterAttribute;
using Naukri.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleScenes : MonoBehaviour
{
    [ExpandElement]
    public SceneObject[] targetScenes;

    private void Start()
    {
        foreach (var scene in targetScenes)
        {
            SceneManager.HandleByLoadingMode(scene.Name, scene.LoadingMode);
        }
    }
}