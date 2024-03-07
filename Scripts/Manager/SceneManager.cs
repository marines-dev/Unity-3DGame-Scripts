using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class SceneManager : BaseManager
{
    public Define.Scene currentSceneType { get { return GetStringToSceneType(GetActiveSceneName()); } }
    public Define.Scene loadingSceneType { get; private set; } = Define.Scene.None;
    Define.Scene nextSceneType = Define.Scene.None;

    public bool isIoading { get; private set; } = false;

    BaseScene currentScene
    {
        get
        {
            Type type = Type.GetType(GetActiveSceneName());

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
    LoadingUI loadingUI_ = null;
    LoadingUI loadingUI
    {
        get
        {
            if(loadingUI_ == null)
            {
                Managers.UI.LoadUI<LoadingUI>();
                loadingUI_ = Managers.UI.GetBaseUI<LoadingUI>();
            }
            return loadingUI_;
        }
    }

    IEnumerator loadingProcessCoroutine = null;
    IEnumerator loadSceneAsyncRoutine   = null;


    protected override void InitDataProcess() { }

    /// <summary>
    /// 예외 : SceneManager는 씬 로딩 완료 후에 리셋 합니다.
    /// </summary>
    protected override void ResetDataProcess()
    {
        ClearLoadingProcess();

        //
        nextSceneType    = Define.Scene.None;
        loadingSceneType = Define.Scene.None;
        isIoading = false;
    }

    public void LoadScene<T>() where T : BaseScene
    {
        if (isIoading)
        {
            Debug.LogWarning("Failed : 씬 로딩 중");
            return;
        }

        //if (CurrentScene != null && typeof(T) == CurrentScene.GetType())
        //{
        //    Debug.LogWarning($"Failed : 로드할 {typeof(T).Name} 씬이 현재 씬과 같습니다.");
        //    return;
        //}

        LoadingProcess<T>();
    }

    bool CompleteLoadProcess()
    {
        if (isIoading == false || loadingSceneType != currentSceneType || currentScene == null)
        {
            string loadingSceneName = loadingSceneType.ToString();
            Debug.LogWarning($"Failed : 현재 씬은 로딩 씬({loadingSceneName})이 아닙니다.");
            return false;
        }
        else
        {
            string loadingSceneName = loadingSceneType.ToString();
            string currentSceneName = currentScene.GetType().Name;
            if (loadingSceneName != currentScene.GetType().Name)
            {
                Debug.LogWarning($"Failed : 현재 씬은 로딩 씬({loadingSceneName})이 아닙니다.");
                return false;
            }
        }
        
        bool doLoadProcss = currentScene.DoLoadProcess();
        if(doLoadProcss)
        {
            Debug.Log($"Success : {currentScene.GetType().Name} 씬의 로딩이 완료되었습니다.");
            loadingSceneType = Define.Scene.None;
        }

        return doLoadProcss;
    }

    void LoadingProcess<T>()
    {
        ClearLoadingProcess();

        loadingProcessCoroutine = LoadingProcessCoroutine(typeof(T));
        StartCoroutine(loadingProcessCoroutine);
    }

    void ClearLoadingProcess()
    {
        if (loadingProcessCoroutine != null)
        {
            StopCoroutine(loadingProcessCoroutine);
            loadingProcessCoroutine = null;
        }

        if(loadSceneAsyncRoutine != null)
        {
            StopCoroutine(loadSceneAsyncRoutine);
            loadSceneAsyncRoutine = null;
        }
    }

    [Obsolete("테스트 중")]
    IEnumerator LoadingProcessCoroutine(Type _type)
    {
        // Start
        isIoading     = true;
        nextSceneType = GetStringToSceneType(_type.Name);

        Managers.UI.CloseBaseUIAll();
        loadingUI.OpenUI();
        yield return null;

        // LoadScene
        loadingSceneType        = GetStringToSceneType(typeof(LoadScene).Name);
        string laodingSceneName = loadingSceneType.ToString();
        yield return LoadSceneAsyncRoutine(laodingSceneName);
        yield return new WaitUntil(() => CompleteLoadProcess()); //  LaodScene 씬에서 로드 완료 시 true

        // NextScene
        loadingSceneType = nextSceneType;
        laodingSceneName = loadingSceneType.ToString();
        yield return LoadSceneAsyncRoutine(laodingSceneName);
        yield return new WaitUntil(() => CompleteLoadProcess()); // 다음 씬에서 로드 완료 시 true

        // Complete
        Debug.Log($"Success : {currentScene.GetType().Name} 씬 로드를 완료했습니다.");
        Managers.UI.CloseBaseUI<LoadingUI>();

        // 로딩 완료 후 SceneManager 리셋
        ResetData();
    }

    /// <summary>
    /// 비동기 방식 씬 로드 함수 입니다.
    /// </summary>
    /// <param name="pSceneName"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAsyncRoutine(string pSceneName)
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(_type.Name);
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(pSceneName);
        asyncOperation.allowSceneActivation = false;
        yield return null;

        Debug.Log($"{pSceneName} | LoadingProgress(Start) : {asyncOperation.progress * 100}%");
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
                asyncOperation.allowSceneActivation = true;

            Debug.Log($"{pSceneName} | LoadProgress(Loading) : {asyncOperation.progress * 100}%");
            yield return null;
        }

        Debug.Log($"{pSceneName} | LoadProgress(Complete) : {asyncOperation.progress * 100}%");
    }

    string GetActiveSceneName()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    Define.Scene GetStringToSceneType(string pSceneName)
    {
        Define.Scene sceneType = Define.Scene.None;
        try
        {
            sceneType = (Define.Scene)Enum.Parse(typeof(Define.Scene), pSceneName);
        }
        catch
        {
            Debug.LogWarning($"현재 씬은 정의되지 않은 SceneType으로 {currentSceneType.ToString()}입니다.");
        }

        return sceneType;
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
}
