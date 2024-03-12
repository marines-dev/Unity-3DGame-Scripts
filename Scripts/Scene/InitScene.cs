﻿using UnityEngine;

public class InitScene : BaseScene
{
    public static bool IsInitSceneLoaded { get; private set; } = false;


    protected override void OnAwake()
    {
        /// LoadManager
        ManagerLoader.CreateManagers();

        /// CreateGlobalScene
        Global.RegisteredGlobalObjects();

        // LoadData
        IsInitSceneLoaded = true;
    }

    protected override void OnStart()
    {
        SceneManager.Instance.LoadBaseScene<TitleScene>();
    }

    protected override void OnDestroy_()
    {
    }
}
