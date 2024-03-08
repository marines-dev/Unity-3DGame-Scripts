using UnityEngine;

public class InitScene : BaseScene
{
    public static bool IsInitSceneLoaded { get; private set; } = false;


    protected override void OnAwake()
    {
        /// CreateGlobalScene
        GlobalScene.CreateGlobalScene();

        // ResisteredUI
        //GlobalScene.UIMng.ResisteredBaseUI();

        // LoadData
        IsInitSceneLoaded = true;
    }

    protected override void OnStart()
    {
        GlobalScene.SceneMng.LoadBaseScene<TitleScene>();
    }

    protected override void OnDestroy_()
    {
    }
}
