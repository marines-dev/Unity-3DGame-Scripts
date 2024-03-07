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
                Debug.LogWarning($"Failed : 게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(initSceneName);
                return;
            }
        }

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
