using System;
using Interface;
using UnityEngine;

public class GlobalScene : BaseScene<GlobalScene>
{
    public static void CreateGlobalScene()
    {
        if (! Manager.SceneMng.IsActiveScene<InitScene>())
        {
            Util.LogWarning();
            return;
        }

        /// Destroy
        GlobalScene instnace = FindObjectOfType<GlobalScene>();
        if(instnace != null)
        {
            Manager.ResourceMng.DestroyGameObject(instnace.gameObject);
        }

        /// Create
        Util.CreateGlobalObject<GlobalScene>();
    }

    protected override void OnAwake() { }
    protected override void OnStart() { }
    protected override void OnDestroy_() { }

    [Obsolete("임시")]
    public TResource LoadResource<TResource>(string path) where TResource : UnityEngine.Object
    {
        return Manager.ResourceMng.Load<TResource>(path);
    }

    [Obsolete("임시")]
    public GameObject InstantiateResource(string pPath, Transform pParent = null)
    {
        return Manager.ResourceMng.Instantiate(pPath, pParent);
    }

    [Obsolete("임시")]
    public void DestroyGameObject(GameObject pGameObject)
    {
        Manager.ResourceMng.DestroyGameObject(pGameObject);
    }
}

public abstract class BaseScene<TScene> : MonoBehaviour, IBaseScene where TScene : BaseScene<TScene>
{
    static TScene instance;
    public static TScene Instance
    {
        get
        {
            if (instance == null)
            {
                Util.LogError();
                return null;
            }
            return instance;
        }
    }

    private static Manager manager;
    protected static Manager Manager
    {
        get
        {
            if (manager == null) { manager = CreateInstance(); }
            return manager;
            //return instance ?? (instance = CreateInstance());
        }
    }


    protected virtual void Awake()
    {
        //if (!InitScene.IsInitSceneLoaded)
        //{
        //    string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        //    string initSceneName = typeof(InitScene).Name;
        //    if(activeSceneName != initSceneName)
        //    {
        //        Util.LogWarning($"게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다.");
        //        UnityEngine.SceneManagement.SceneManager.LoadScene(initSceneName);
        //        return;
        //    }
        //}

        if (!InitScene.IsInitSceneLoaded)
        {
            if (! Manager.SceneMng.IsActiveScene<InitScene>())
            {
                Manager.SceneMng.LoadInitScene();
                return;
            }
        }

        if (instance == null)
        {
            instance = this as TScene;
            Util.LogSuccess($"<{this.gameObject}> 씬을 생성하고, Instance가 등록되었습니다.");
        }
        else
        {
            Destroy(gameObject);
            Util.LogWarning($"<{this.gameObject}> 씬이 중복 생성되어 파괴하였습니다. 확인이 필요합니다.");
        }

        OnAwake();
    }

    protected virtual void Start()
    {
        OnStart();
    }

    private void OnDestroy()
    {
        OnDestroy_();

        instance = null;

        Util.LogSuccess($"<{this.gameObject}> 씬이 파괴되고, Instance가 해제되었습니다.");
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDestroy_();

    /// <summary>
    /// 파괴되지 않는 Managers의 오브젝트를 생성하고 반환합니다.
    private static Manager CreateInstance()
    {
        /// Delete
        if (manager != null)
        {
            manager = null;
        }

        /// New
        manager = new Manager();
        return manager;
    }
}
