using UnityEngine;

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
