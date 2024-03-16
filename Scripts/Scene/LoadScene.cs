using System;
using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene<LoadScene>
{
    /// <summary>
    /// Table
    /// </summary>

    /// <summary>
    /// MainUI
    /// </summary>
    private LoadingUI loadUI = null;

    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    string preSceneName = string.Empty;
    string nextSceneName = string.Empty;

    private IEnumerator loadingProcessCoroutine = null;


    protected override void OnAwake()
    {
        /// LoadUI
        Manager.UIMng.CloseBaseUIAll();
        loadUI = Manager.UIMng.CreateOrGetBaseUI<LoadingUI>(MainCanvas);
        loadUI.Close();
    }

    protected override void OnStart() 
    {
        string loadSceneName = typeof(LoadScene).Name;
        SetActiveScene(loadSceneName);

        loadUI.Open();

        ///
        LoadingProcess();
    }

    protected override void OnDestroy_() 
    {
        ClearLoadingProcess();

        loadUI.Close();
        /// Complete
        Debug.Log($"Success : {Manager.SceneMng.ActiveSceneName} 씬 로드를 완료했습니다.");
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

        preSceneName = Manager.SceneMng.PreSceneName;
        nextSceneName = Manager.SceneMng.NextSceneName;
        yield return null;

        yield return UnloadSceneAsync(preSceneName);
        Manager.ReleaseManagers();
        yield return null;

        /// 메모리 정리(임시)
        Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return null;

        yield return LoadSceneAsync(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        SetActiveScene(nextSceneName);
        yield return null;

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
