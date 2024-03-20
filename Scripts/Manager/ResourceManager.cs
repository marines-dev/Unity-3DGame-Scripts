using System;
using UnityEngine;


public class ResourceManager : Manager//BaseManager<ResourceManager>
{
    protected override void OnInitialized() { }
    public override void OnRelease() { }

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string  name    = path;
            int     index   = name.LastIndexOf('/');
            if (index >= 0) { name = name.Substring(index + 1); }
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>(path);
        if (original == null)
        {
            Util.LogWarning($"Instantiate prefab : {path}");
            return null;
        }

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;
        //Util.LogSuccess($"Instantiate prefab : {path}");
        return go;
    }

    public GameObject CreateGameObject(string pGo_name = "", Transform pTran = null)
    {
        GameObject go = new GameObject();
        
        if(string.IsNullOrEmpty(pGo_name) == false)
            go.name = pGo_name;

        go.transform.SetParent(pTran);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        return go;
    }

    public T CreateComponentObject<T>(string pGo_name = "", Transform pTran = null) where T : Component
    {
        GameObject go = new GameObject();

        if (string.IsNullOrEmpty(pGo_name) == false)
            go.name = pGo_name;

        go.transform.SetParent(pTran);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        return go.AddComponent<T>();
    }

    public void DestroyGameObject(GameObject go)
    {
        if (go == null)
            return;

        GameObject.Destroy(go);
        go = null;
    }
}
