using UnityEngine;

public class InitScene : BaseScene<InitScene, InitUI>
{
    public static bool IsInitSceneLoaded { get; private set; } = false;


    protected override void OnAwake()
    {
        /// LoadManager
        Manager.CreateManagers();
        CreateGlobalScene();

        IsInitSceneLoaded = true;
    }

    protected override void OnStart()
    {
        Manager.SceneMng.LoadBaseScene<TitleScene>();
    }

    protected override void onDestroy()
    {
    }

    private void CreateGlobalScene()
    {
        /// Destroy
        GlobalScene instnace = FindObjectOfType<GlobalScene>();
        if (instnace != null)
        {
            ResourceLoader.DestroyGameObject(instnace.gameObject);
        }

        /// Create
        Util.CreateGlobalObject<GlobalScene>();
    }
}
