using Naukri;
using Naukri.BetterAttribute;
using Naukri.BetterInspector;
using Naukri.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleScenes : NaukriBehaviour
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

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadScene(sceneName);
    }

    public void EnableScene(string sceneName)
    {
        SceneManager.EnableScene(sceneName);
    }

    public void DisableScene(string sceneName)
    {
        SceneManager.DisableScene(sceneName);
    }

    public void EnableOrLoadScene(string sceneName)
    {
        SceneManager.EnableOrLoadScene(sceneName);
    }

    public void LoadAndDisableScene(string sceneName)
    {
        SceneManager.LoadAndDisableScene(sceneName);
    }

    [DisplayMethod]
    public void HandleByLoadingMode(string sceneName, LoadingMode loadingMode)
    {
        SceneManager.HandleByLoadingMode(sceneName, loadingMode);
    }
}