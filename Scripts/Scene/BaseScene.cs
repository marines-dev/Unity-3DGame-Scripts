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

    [Obsolete("�ӽ�")]
    public TResource LoadResource<TResource>(string path) where TResource : UnityEngine.Object
    {
        return Manager.ResourceMng.Load<TResource>(path);
    }

    [Obsolete("�ӽ�")]
    public GameObject InstantiateResource(string pPath, Transform pParent = null)
    {
        return Manager.ResourceMng.Instantiate(pPath, pParent);
    }

    [Obsolete("�ӽ�")]
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
        //        Util.LogWarning($"���� �ʱ�ȭ�� ���� {typeof(InitScene).Name} ������ �̵��մϴ�.");
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
            Util.LogSuccess($"<{this.gameObject}> ���� �����ϰ�, Instance�� ��ϵǾ����ϴ�.");
        }
        else
        {
            Destroy(gameObject);
            Util.LogWarning($"<{this.gameObject}> ���� �ߺ� �����Ǿ� �ı��Ͽ����ϴ�. Ȯ���� �ʿ��մϴ�.");
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

        Util.LogSuccess($"<{this.gameObject}> ���� �ı��ǰ�, Instance�� �����Ǿ����ϴ�.");
    }

    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDestroy_();

    /// <summary>
    /// �ı����� �ʴ� Managers�� ������Ʈ�� �����ϰ� ��ȯ�մϴ�.
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
