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
        UIManager.Instance.CloseBaseUIAll();
        loadUI = UIManager.Instance.CreateOrGetBaseUI<LoadingUI>();
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
        loadUI?.Close();
        ClearLoadingProcess();

        /// Complete
        Debug.Log($"Test : {SceneManager.Instance.ActiveSceneName} �� �ε带 �Ϸ��߽��ϴ�.");
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

        preSceneName = SceneManager.Instance.PreSceneName;
        nextSceneName = SceneManager.Instance.NextSceneName;
        yield return null;

        yield return UnloadSceneAsync(preSceneName);
        ManagerLoader.ResetManagers();
        yield return null;

        /// �޸� ����(�ӽ�)
        Resources.UnloadUnusedAssets();
        GC.Collect();
        yield return null;

        yield return LoadSceneAsync(nextSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        SetActiveScene(nextSceneName);
        yield return null;

        Global.RegisteredGlobalObjects();
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
