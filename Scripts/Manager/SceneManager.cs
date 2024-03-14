using Interface;
using UnityEngine;


public class SceneManager : Manager
{
    public string PreSceneName { get; private set; } = string.Empty;
    public string NextSceneName { get; private set; } = string.Empty;

    public string ActiveSceneName { get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; } }


    protected override void OnInitialized() { }

    public override void OnRelease()
    {
        PreSceneName = string.Empty;
        NextSceneName = string.Empty;
    }

    public void LoadBaseScene<TScene>() where TScene : IBaseScene
    {
        PreSceneName = ActiveSceneName;
        NextSceneName = GetBaseSceneToString<TScene>();

        string loadSceneName = typeof(LoadScene).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public bool IsActiveScene<TScene>() where TScene : IBaseScene
    {
        string sceneName = GetBaseSceneToString<TScene>();
        return sceneName == ActiveSceneName;
    }

    string GetBaseSceneToString<TScene>() where TScene : IBaseScene
    {
        return typeof(TScene).Name;
    }

    public void AddSceneLoadedEvent(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode> pSceneLoadedEvent)
    {
        if (pSceneLoadedEvent == null)
        {
            Debug.LogWarning("");
            return;
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= pSceneLoadedEvent;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += pSceneLoadedEvent;
    }

    public void LoadInitScene()
    {
        Util.LogWarning($"게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다.");

        string sceneName = typeof(InitScene).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
