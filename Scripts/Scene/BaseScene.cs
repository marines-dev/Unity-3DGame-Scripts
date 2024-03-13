using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnDestroy_();


    protected virtual void Awake()
    {
        if (!InitScene.IsInitSceneLoaded)
        {
            string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string initSceneName = typeof(InitScene).Name;
            if(activeSceneName != initSceneName)
            {
                Util.LogWarning($"���� �ʱ�ȭ�� ���� {typeof(InitScene).Name} ������ �̵��մϴ�.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(initSceneName);
                return;
            }
        }

        OnAwake();
    }

    protected virtual void Start()
    {
        OnStart();
    }

    private void OnDestroy()
    {
        OnDestroy_();
    }
}
