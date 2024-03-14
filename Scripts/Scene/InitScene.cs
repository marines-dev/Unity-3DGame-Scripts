using UnityEngine;

public class InitScene : BaseScene<InitScene>
{
    public static bool IsInitSceneLoaded { get; private set; } = false;


    protected override void OnAwake()
    {
        /// LoadManager
        Manager.InitializedManagers();
        GlobalScene.CreateGlobalScene();

        IsInitSceneLoaded = true;
    }

    protected override void OnStart()
    {
        Manager.SceneMng.LoadBaseScene<TitleScene>();
    }

    protected override void OnDestroy_()
    {
    }
}
