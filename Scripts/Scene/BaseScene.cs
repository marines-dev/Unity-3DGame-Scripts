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
        //    Debug.LogWarning($"���� �ʱ�ȭ�� ���� {typeof(InitScene).Name} ������ �̵��մϴ�.");

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
