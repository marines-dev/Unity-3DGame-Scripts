using System;
using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene
{
    LoadingUI loadUI = null;
    //public BaseInput BaseInput { get; }

    string preSceneName = string.Empty;
    string nextSceneName = string.Empty;

    IEnumerator loadingProcessCoroutine = null;

    protected override void OnAwake()
    {
        /// LoadUI
        GlobalScene.UIMng.CloseBaseUIAll();
        GlobalScene.UIMng.LoadUI<LoadingUI>();
        loadUI = GlobalScene.UIMng.GetBaseUI<LoadingUI>();
    }

    protected override void OnStart() 
    {
        loadUI.Open();

        ///
        LoadingProcess();
    }

    protected override void OnDestroy_() 
    {
        ClearLoadingProcess();

        loadUI.Close();
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

        string loadSceneName = typeof(LoadScene).Name;
        SetActiveScene(loadSceneName);

        preSceneName = GlobalScene.SceneMng.PreSceneName;
        nextSceneName = GlobalScene.SceneMng.NextSceneName;
        yield return null;

        yield return UnloadSceneAsync(preSceneName);

        GlobalScene.MngLoader.InitManagers();
        yield return null;

        /// 메모리 정리(임시)
        Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return null;

        yield return LoadSceneAsync(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        SetActiveScene(nextSceneName);
        //GlobalScene.SceneMng.GetOrCreateActiveScene();
        yield return null;

        yield return UnloadSceneAsync(loadSceneName);

        /// Complete
        Debug.Log($"Success : {GlobalScene.SceneMng.ActiveSceneName} 씬 로드를 완료했습니다.");
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
