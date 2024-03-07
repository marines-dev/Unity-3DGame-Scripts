using System;
using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    static bool doInitSceneLoad = false;
    bool        doLoadProcess   = false;

    protected IEnumerator   loadingProcessRoutine   = null;
    IEnumerator             loadingProcessCoroutine = null;


    /// <summary>
    /// 씬에서 로드할 데이터들을 순서대로 진행하는 함수 입니다.
    /// </summary>
    protected abstract IEnumerator LoadingProcessRoutine();

    /// <summary>
    /// 씬 로드 후 BaseScene의 시작 시점 입니다.
    /// </summary>
    protected abstract void OpenScene();

    /// <summary>
    /// 씬 퇴장 시점에 초기화 합니다.
    /// </summary>
    protected abstract void CloseScene();


    void Awake()
    {
        doLoadProcess = false;

        if (doInitSceneLoad == false && Managers.Scene.currentSceneType != Define.Scene.InitScene)
        {
            Debug.LogWarning(string.Format($"게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다."));

            //
            //Managers.Scene.LoadScene<InitScene>();
            string sceneName = Define.Scene.InitScene.ToString();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            return;
        }

        LoadProcess();
    }

    void OnDestroy()
    {
        CloseScene();
    }

    [Obsolete("InitScene 전용 : InitScene에서 초기화 후 호출합니다.")]
    protected void CompleteInitSceneLoading()
    {
        if (Managers.Scene.currentSceneType != Define.Scene.InitScene)
        {
            Debug.LogWarning("Failed : ");
            return;
        }
        doInitSceneLoad = true;
    }

    [Obsolete("SceneManager 전용 : BaseScene의 LoadingProcess를 완료하면 SceneManager에서 호출하는 함수입니다.")] 
    public bool DoLoadProcess()
    {
        string baseSceneName    = this.GetType().Name;
        string loadingSceneName = Managers.Scene.loadingSceneType.ToString();
        if (Managers.Scene.isIoading == false || baseSceneName != loadingSceneName)
        {
            Debug.LogWarning($"Failed : {baseSceneName}는 로딩을 진행하고 있지 않습니다.");
            return false;
        }

        return doLoadProcess;
    }

    void LoadProcess()
    {
        // loadingProcessRoutine
        if (loadingProcessRoutine != null)
        {
            StopCoroutine(loadingProcessRoutine);
            loadingProcessRoutine = null;
        }

        // initProcessCoroutine
        if (loadingProcessCoroutine != null)
        {
            StopCoroutine(loadingProcessCoroutine);
            loadingProcessCoroutine = null;
        }

        loadingProcessCoroutine = LoadingProcessCoroutine();
        StartCoroutine(loadingProcessCoroutine);

        Debug.Log($"Success : {this.GetType().Name}의 LoadProcess 완료");
    }

    /// <summary>
    /// 씬 입장 시점에 로드를 진행하는 함수 입니다.
    /// </summary>
    IEnumerator LoadingProcessCoroutine()
    {
        loadingProcessRoutine = LoadingProcessRoutine();
        yield return loadingProcessRoutine;

        // Complete
        // ※데이터 로드 및 초기화 완료 시 호출하는 함수 입니다.
        //yield return new WaitForEndOfFrame();
        doLoadProcess = true;
        OpenScene();
    }
}
