using System;
using System.Collections;
using UnityEngine;


public class SceneManager : BaseManager
{
    public string PreSceneName { get; private set; } = string.Empty;
    public string NextSceneName { get; private set; } = string.Empty;

    public string ActiveSceneName { get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; } }
    //public string LoadSceneName { get { return typeof(LoadScene).Name; } }

    //public bool IsBaseSceneLoading { get; private set; } = false;
    bool IsBaseSceneLoading  = false;

    //string loadingSceneType = string.Empty;

    BaseScene currentScene
    {
        get
        {
            Type type = Type.GetType(ActiveSceneName);

            if (type == null || typeof(BaseScene) != type.BaseType)
            {
                Debug.LogWarning($"Failed : 사용할 수 없는 {type.Name} 형식으로, {typeof(BaseScene).Name} 형식만 사용 가능합니다.");
                return null;
            }

            BaseScene baseScene = FindObjectOfType(type) as BaseScene;

            if (baseScene == null)
            {
                GameObject baseScene_obj = new GameObject();
                baseScene_obj.name = $"@{type.ToString()}";
                baseScene = baseScene_obj.AddComponent(type) as BaseScene;
            }

            Debug.Log($"Success : 현재 씬은 {baseScene.name}의 {baseScene.GetType().Name}입니다");
            return baseScene;
        }
    }

    static Action globalSceneUnloadEvent = null;
    static Action globalSceneLoadEvent = null;


    public static void SetSceneManager(Action pSceneUnloadEvent, Action pSceneLoadEvent)
    {
        globalSceneUnloadEvent = pSceneUnloadEvent;
        globalSceneLoadEvent = pSceneLoadEvent;
    }

    protected override void OnAwake() { }

    public override void OnReset()
    {
        PreSceneName = string.Empty;
        NextSceneName = string.Empty;
        IsBaseSceneLoading = false;
    }

    public void UnloadedScene()
    {
        //if (! IsBaseSceneLoading)
        //{
        //    Debug.LogWarning($"Failed :");
        //    return;
        //}

        if (globalSceneUnloadEvent != null)
        {
            globalSceneUnloadEvent.Invoke();
        }
    }

    public void LoadedScene()
    {
        //if (! IsBaseSceneLoading)
        //{
        //    Debug.LogWarning($"Failed :");
        //    return;
        //}

        if (globalSceneLoadEvent != null)
        {
            globalSceneLoadEvent.Invoke();
        }
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

    public bool IsActiveScene<TScene>() where TScene : BaseScene
    {
        string sceneName = GetBaseSceneToString<TScene>();
        return sceneName == ActiveSceneName;
    }

    string GetBaseSceneToString<TScene>() where TScene : BaseScene
    {
        return typeof(TScene).Name;
    }

    //public void LoadScene<T>() where T : BaseScene
    //{
    //    if (isIoading)
    //    {
    //        Debug.LogWarning("Failed : 씬 로딩 중");
    //        return;
    //    }

    //    //if (CurrentScene != null && typeof(T) == CurrentScene.GetType())
    //    //{
    //    //    Debug.LogWarning($"Failed : 로드할 {typeof(T).Name} 씬이 현재 씬과 같습니다.");
    //    //    return;
    //    //}

    //    LoadingProcess<T>();
    //}

    //void LoadingProcess<T>()
    //{
    //    ClearLoadingProcess();

    //    loadingProcessCoroutine = LoadingProcessCoroutine(typeof(T));
    //    StartCoroutine(loadingProcessCoroutine);
    //}

    //void ClearLoadingProcess()
    //{
    //    if (loadingProcessCoroutine != null)
    //    {
    //        StopCoroutine(loadingProcessCoroutine);
    //        loadingProcessCoroutine = null;
    //    }

    //    if(loadSceneAsyncRoutine != null)
    //    {
    //        StopCoroutine(loadSceneAsyncRoutine);
    //        loadSceneAsyncRoutine = null;
    //    }
    //}

    //[Obsolete("테스트 중")]
    //IEnumerator LoadingProcessCoroutine(Type _type)
    //{
    //    // Start
    //    isIoading     = true;
    //    nextSceneName = _type.Name;

    //    GlobalScene.UIMng.CloseBaseUIAll();
    //    loadingUI.OpenUI();
    //    yield return null;

    //    // LoadScene
    //    loadingSceneType = LoadSceneName;
    //    string laodingSceneName = loadingSceneType.ToString();
    //    yield return LoadSceneAsyncRoutine(laodingSceneName);

    //    // NextScene
    //    loadingSceneType = nextSceneName;
    //    laodingSceneName = loadingSceneType.ToString();
    //    yield return LoadSceneAsyncRoutine(laodingSceneName);

    //    // Complete
    //    Debug.Log($"Success : {currentScene.GetType().Name} 씬 로드를 완료했습니다.");
    //    GlobalScene.UIMng.CloseBaseUI<LoadingUI>();

    //    // 로딩 완료 후 SceneManager 리셋
    //    Initialize();
    //}

    ///// <summary>
    ///// 비동기 방식 씬 로드 함수 입니다.
    ///// </summary>
    ///// <param name="pSceneName"></param>
    ///// <returns></returns>
    //IEnumerator LoadSceneAsyncRoutine(string pSceneName)
    //{
    //    //UnityEngine.SceneManagement.SceneManager.LoadScene(_type.Name);
    //    AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(pSceneName);
    //    asyncOperation.allowSceneActivation = false;
    //    yield return null;

    //    Debug.Log($"{pSceneName} | LoadingProgress(Start) : {asyncOperation.progress * 100}%");
    //    while (!asyncOperation.isDone)
    //    {
    //        if (asyncOperation.progress >= 0.9f)
    //            asyncOperation.allowSceneActivation = true;

    //        Debug.Log($"{pSceneName} | LoadProgress(Loading) : {asyncOperation.progress * 100}%");
    //        yield return null;
    //    }

    //    Debug.Log($"{pSceneName} | LoadProgress(Complete) : {asyncOperation.progress * 100}%");
    //}


    //Define.Scene GetStringToSceneType(string pSceneName)
    //{
    //    Define.Scene sceneType = Define.Scene.None;
    //    try
    //    {
    //        sceneType = (Define.Scene)Enum.Parse(typeof(Define.Scene), pSceneName);
    //    }
    //    catch
    //    {
    //        Debug.LogWarning($"현재 씬은 정의되지 않은 SceneType으로 {ActiveSceneName.ToString()}입니다.");
    //    }

    //    return sceneType;
    //}

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
}
