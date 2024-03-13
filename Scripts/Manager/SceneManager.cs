using System;
using UnityEngine;


public class SceneManager : BaseManager<SceneManager>
{
    public string PreSceneName { get; private set; } = string.Empty;
    public string NextSceneName { get; private set; } = string.Empty;

    public string ActiveSceneName { get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; } }
    //public string LoadSceneName { get { return typeof(LoadScene).Name; } }

    //public bool IsBaseSceneLoading { get; private set; } = false;
    bool IsBaseSceneLoading  = false;

    //string loadingSceneType = string.Empty;

    //BaseScene currentScene
    //{
    //    get
    //    {
    //        Type type = Type.GetType(ActiveSceneName);

    //        if (type == null || typeof(BaseScene) != type.BaseType)
    //        {
    //            Debug.LogWarning($"Failed : 사용할 수 없는 {type.Name} 형식으로, {typeof(BaseScene).Name} 형식만 사용 가능합니다.");
    //            return null;
    //        }

    //        BaseScene baseScene = GameObject.FindObjectOfType(type) as BaseScene;

    //        if (baseScene == null)
    //        {
    //            GameObject baseScene_obj = new GameObject();
    //            baseScene_obj.name = $"@{type.ToString()}";
    //            baseScene = baseScene_obj.AddComponent(type) as BaseScene;
    //        }

    //        Debug.Log($"Success : 현재 씬은 {baseScene.name}의 {baseScene.GetType().Name}입니다");
    //        return baseScene;
    //    }
    //}

    //static Action globalSceneUnloadEvent = null;
    //static Action globalSceneLoadEvent = null;

    protected override void OnInitialized() { }

    public override void OnReset()
    {
        PreSceneName = string.Empty;
        NextSceneName = string.Empty;
        IsBaseSceneLoading = false;
    }

    public void LoadBaseScene<TScene>() where TScene : BaseScene
    {
        if (IsBaseSceneLoading)
        {
            Debug.LogWarning($"Failed : 씬 로딩 중");
            return;
        }

        PreSceneName = ActiveSceneName;
        NextSceneName = GetBaseSceneToString<TScene>();

        string loadSceneName = typeof(LoadScene).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public bool IsActiveScene<TScene>() where TScene : BaseScene
    {
        string sceneName = GetBaseSceneToString<TScene>();
        return sceneName == ActiveSceneName;
    }

    string GetBaseSceneToString<TScene>() where TScene : BaseScene
    {
        return typeof(TScene).Name;
    }

    public void AddSceneLoadedEvent(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> pSceneLoadedEvent)
    {
        if (pSceneLoadedEvent == null)
        {
            Debug.LogWarning("");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= pSceneLoadedEvent;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += pSceneLoadedEvent;
    }

    //BaseScene GetCurrentBaseScene()
    //{
    //    Type type = Type.GetType(GetActiveSceneName());

    //    if (type == null || typeof(BaseScene) != type.BaseType)
    //    {
    //        Debug.LogError($"사용할 수 없는 {type.Name} 형식으로, {typeof(BaseScene).Name} 형식만 사용 가능합니다.");
    //        return null;
    //    }

    //    BaseScene baseScene = FindObjectOfType(type) as BaseScene;

    //    if (baseScene == null)
    //    {
    //        GameObject baseScene_obj = new GameObject();
    //        baseScene_obj.name = $"@{type.ToString()}";
    //        baseScene = baseScene_obj.AddComponent(type) as BaseScene;
    //    }

    //    Debug.Log($"Success : 현재 씬은 {baseScene.name}의 {baseScene.GetType().Name}입니다");
    //    return baseScene;
    //}

    //public static void SetSceneManager(Action pSceneUnloadEvent, Action pSceneLoadEvent)
    //{
    //    //globalSceneUnloadEvent = pSceneUnloadEvent;
    //    //globalSceneLoadEvent = pSceneLoadEvent;
    //}

    //public void AddSceneLoaderEvent(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene> pSceneUnloaded, UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> pSceneLoadedEvent)
    //{
    //    if (pSceneLoadedEvent == null)
    //    {
    //        Debug.LogWarning("");
    //        return;
    //    }

    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded += pSceneLoadedEvent;
    //    UnityEngine.SceneManagement.SceneManager.sceneUnloaded += pSceneUnloaded;
    //}

    //public void RemoveSceneLoaderEvent(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene> pSceneUnloaded, UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> pSceneLoadedEvent)
    //{
    //    if (pSceneLoadedEvent == null)
    //    {
    //        Debug.LogWarning("");
    //        return;
    //    }

    //    UnityEngine.SceneManagement.SceneManager.sceneLoaded -= pSceneLoadedEvent;
    //    UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= pSceneUnloaded;
    //}

    //public void UnloadedScene()
    //{
    //    //if (! IsBaseSceneLoading)
    //    //{
    //    //    Debug.LogWarning($"Failed :");
    //    //    return;
    //    //}

    //    //if (globalSceneUnloadEvent != null)
    //    //{
    //    //    globalSceneUnloadEvent.Invoke();
    //    //}
    //}

    //public void LoadedScene()
    //{
    //    //if (! IsBaseSceneLoading)
    //    //{
    //    //    Debug.LogWarning($"Failed :");
    //    //    return;
    //    //}

    //    //if (globalSceneLoadEvent != null)
    //    //{
    //    //    globalSceneLoadEvent.Invoke();
    //    //}
    //}
}
