using System;
using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene<LoadScene, LoadUI>
{
    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    string preSceneName = string.Empty;
    string nextSceneName = string.Empty;

    private IEnumerator loadingProcessCoroutine = null;


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

        /// Complete
        Util.LogSuccess($"{Manager.SceneMng.ActiveSceneName} 씬 로드를 완료했습니다.");
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

        preSceneName  = Manager.SceneMng.PreSceneName;
        nextSceneName = Manager.SceneMng.NextSceneName;
        yield return null;

        yield return UnloadSceneAsync(preSceneName);
        Manager.ReleaseManagers();
        yield return null;

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
