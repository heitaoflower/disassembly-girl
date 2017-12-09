using System;
using System.Collections;
using System.Collections.Generic;

using Utils;
using Manager;
using View.Login;

using UnityEngine;

public class DisassemblyGirl : MonoBehaviour {
    
    private static GameObject root = null;

    private static IList<Action> singletons = new List<Action>();

    void Awake()
    {
        root = this.gameObject;

        Screen.SetResolution(GlobalDefinitions.SCREEN_WIDTH, GlobalDefinitions.SCREEN_HEIGHT, false);
    
        GameObject.DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = GlobalDefinitions.FPS;

        StartCoroutine(InitializeSingletons());

        StartCoroutine(StartGame());
    }

    public void OnApplicationQuit()
    {
        for (int index = singletons.Count - 1; index >= 0; index--)
        {
            singletons[index]();
        }
    }

    private IEnumerator InitializeSingletons()
    {
        yield return null;

        AddSingleton<SystemManager>();
        AddSingleton<UserManager>();
        AddSingleton<GuideManager>();
        AddSingleton<TrophyManager>();
        AddSingleton<DungeonManager>();
        AddSingleton<HomeManager>();
        AddSingleton<LayerManager>();
        AddSingleton<TaskManager>();
        AddSingleton<SoundManager>();
        AddSingleton<CameraManager>();
        AddSingleton<NetManager>();
    }

    private void AddSingleton<T>() where T : Singleton<T>
    {
        if (root.GetComponent<T>() == null)
        {
            T type = root.AddComponent<T>();
            type.SetInstance(type);
            type.Initialize();

            singletons.Add(delegate() { type.Release(); });
        }
    }

    private IEnumerator StartGame()
    {
        yield return null;

        LayerManager.GetInstance().AddPopUpView<LoginWindow>();
    }
}
