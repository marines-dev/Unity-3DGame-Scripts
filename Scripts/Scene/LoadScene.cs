using System;
using System.Collections;
using Interface;
using UnityEngine;

public class LoadScene : BaseScene<LoadScene, LoadUI>
{
    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    private static string preSceneName  = string.Empty;
    private static string nextSceneName = string.Empty;

    private IEnumerator loadingProcessCoroutine = null;


    public static void SetLoadScene(string pPreSceneName, string pNextSceneName)
    {
        if(!SceneLoader.IsSceneLoading)
        {
            Util.LogError();
            return;
        }

        preSceneName  = pPreSceneName;
        nextSceneName = pNextSceneName;
    }

    protected override void OnAwake()
    {
        //CloseBaseUIAll();
    }

    protected override void OnStart() 
    {
        string loadSceneName = typeof(LoadScene).Name;
        SetActiveScene(loadSceneName);

        ///
        LoadingProcess();
    }

    protected override void onDestroy() 
    {
        ClearLoadingProcess();
        preSceneName  = string.Empty;
        nextSceneName = string.Empty;

        SceneLoader.CompleteSceneLoading();
        Util.LogSuccess($"{SceneLoader.ActiveSceneName} 씬 로드를 완료했습니다.");
    }

    void LoadingProcess()
    {
        ClearLoadingProcess();

        loadingProcessCoroutine = LoadingProcessCoroutine();
        StartCoroutine(loadingProcessCoroutine);
    }

    void ClearLoadingProcess()
    {
        if (loadingProcessCoroutine != null)
        {
            StopCoroutine(loadingProcessCoroutine);
            loadingProcessCoroutine = null;
        }
    }

    IEnumerator LoadingProcessCoroutine()
    {
        yield return null;

        yield return UnloadSceneAsync(preSceneName);
        //Manager_Legacy.ReleaseManagers();
        //yield return null;

        Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return new WaitUntil(() => MainUI.IsLoadingUI_AnimationCompleted);

        yield return LoadSceneAsync(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        SetActiveScene(nextSceneName);

        string loadSceneName = typeof(LoadScene).Name;
        yield return UnloadSceneAsync(loadSceneName);
    }

    void SetActiveScene(string pSceneName)
    {
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(pSceneName);
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
    }

    AsyncOperation UnloadSceneAsync(string pSceneName)
    {
        return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(pSceneName);
    }

    AsyncOperation LoadSceneAsync(string pSceneName, UnityEngine.SceneManagement.LoadSceneMode pLoadSceneMode)
    {
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(pSceneName, pLoadSceneMode);
    }
}
