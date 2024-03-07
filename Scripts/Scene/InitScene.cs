using UnityEngine;

public class InitScene : BaseScene
{
    public static bool IsInitSceneLoaded { get; private set; } = false;


    protected override void OnAwake()
    {
        /// CreateGlobalScene
        GlobalScene.CreateGlobalScene();

        // ResisteredUI
        GlobalScene.UIMng.ResisteredBaseUI();

        // LoadData
        GlobalScene.UIMng.LoadUI<LoadingUI>();

        IsInitSceneLoaded = true;
    }

    protected override void OnStart()
    {
        GlobalScene.SceneMng.LoadScene<TitleScene>();
    }

    protected override void OnDestroy_()
    {
    }
}
