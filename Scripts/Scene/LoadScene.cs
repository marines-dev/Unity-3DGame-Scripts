using System;
using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene
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
        GlobalScene.UIMng.CloseBaseUIAll();
        loadUI = GlobalScene.UIMng.CreateOrGetBaseUI<LoadingUI>();
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

        if(loadUI != null)
        {
            loadUI.Close();
        }
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

        /// Init
        {
            string loadSceneName = typeof(LoadScene).Name;
            SetActiveScene(loadSceneName);

            preSceneName = GlobalScene.SceneMng.PreSceneName;
            nextSceneName = GlobalScene.SceneMng.NextSceneName;
            yield return null;
        }

        /// UnloadScene
        {
            GlobalScene.SceneMng.UnloadedScene();
            yield return UnloadSceneAsync(preSceneName);
            yield return null;

            /// 메모리 정리(임시)
            Resources.UnloadUnusedAssets();
            GC.Collect();
            yield return null;
        }

        /// LoadScene
        {
            yield return LoadSceneAsync(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            SetActiveScene(nextSceneName);
            //GlobalScene.SceneMng.GetOrCreateActiveScene();
            yield return null;

            GlobalScene.SceneMng.LoadedScene();
            yield return null;

            string loadSceneName = typeof(LoadScene).Name;
            yield return UnloadSceneAsync(loadSceneName);
        }


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
