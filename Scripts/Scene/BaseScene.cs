using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDestroy_();


    protected virtual void Awake()
    {
        //if (! InitScene.IsInitSceneLoaded && GlobalScene.SceneMng.currentSceneType != Define.Scene.InitScene)
        //{
        //    Debug.LogWarning($"게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다.");

        //    string sceneName = Define.Scene.InitScene.ToString();
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        //    return;
        //}

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnDestroy()
    {
        OnDestroy_();
    }
}
