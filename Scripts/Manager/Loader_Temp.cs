using System;
using Interface;
using UnityEngine;

public static class SceneLoader
{
    public static string ActiveSceneName { get { return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name; } }
    public static bool   IsSceneLoading { get; private set; } = false;

    [Obsolete]
    public static void CompleteSceneLoading()
    {
        if (IsSceneLoading)
            IsSceneLoading = false;
    }

    public static void LoadBaseScene<TScene>() where TScene : IBaseScene
    {
        IsSceneLoading = true;
        string preSceneName  = ActiveSceneName;
        string nextSceneName = GetBaseSceneToString<TScene>();

        LoadScene.SetLoadScene(preSceneName, nextSceneName);
        string loadSceneName = typeof(LoadScene).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public static bool IsActiveScene<TScene>() where TScene : IBaseScene
    {
        string sceneName = GetBaseSceneToString<TScene>();
        return sceneName == ActiveSceneName;
    }

    public static void LoadInitScene()
    {
        Util.LogWarning($"게임 초기화를 위해 {typeof(InitScene).Name} 씬으로 이동합니다.");

        string sceneName = typeof(InitScene).Name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private static string GetBaseSceneToString<TScene>() where TScene : IBaseScene
    {
        return typeof(TScene).Name;
    }
}

public static class ResourceLoader
{
    public static TRes Load<TRes>(string path) where TRes : UnityEngine.Object
    {
        TRes res = Resources.Load<TRes>(path);
        if (res == null)
        {
            Util.LogWarning($"Load resource : {path}");
        }
        return res;
    }

    public static GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject res = Load<GameObject>(path);
        if (res == null) { return null; }

        GameObject go = UnityEngine.Object.Instantiate(res, parent);
        go.name = res.name;
        //Util.LogSuccess($"Instantiate prefab : {path}");
        return go;
    }

    public static TComp CreateGameObject<TComp>(string pGo_name = "", Transform pParent = null) where TComp : Component
    {
        GameObject go = new GameObject();

        if (string.IsNullOrEmpty(pGo_name) == false)
            go.name = pGo_name;

        go.transform.SetParent(pParent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        return go.AddComponent<TComp>();
    }

    public static void DestroyGameObject(GameObject go)
    {
        if (go == null)
            return;

        GameObject.Destroy(go);
        go = null;
    }
}
