using UnityEngine;

public class InitScene : BaseScene<InitScene>
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

    protected override void OnDestroy_()
    {
    }

    private void CreateGlobalScene()
    {
        /// Destroy
        GlobalScene instnace = FindObjectOfType<GlobalScene>();
        if (instnace != null)
        {
            Manager.ResourceMng.DestroyGameObject(instnace.gameObject);
        }

        /// Create
        Util.CreateGlobalObject<GlobalScene>();
    }
}
