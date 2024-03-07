using System;
using UnityEngine;

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class ResourceManager : BaseManager
{
    protected override void InitDataProcess() { }
    protected override void ResetDataProcess() { }

    public T LoadResource<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            //GameObject go = Managers.Pool.GetOriginal(name);
            //if (go != null)
                //return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject InstantiateResource(string path, Transform parent = null)
    {
        GameObject original = LoadResource<GameObject>(path);
        if (original == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        //if (original.GetComponent<Poolable>() != null)
            //return Managers.Pool.Pop(original, res_storage_obj.transform).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = original.name;
        Debug.Log($"Success to load prefab : {path}");
        return go;
    }

    public GameObject CreateGameObject(string pGo_name = "", Transform pTran = null)
    {
        GameObject go = new GameObject();
        
        if(string.IsNullOrEmpty(pGo_name) == false)
            go.name = pGo_name;

        //임시
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

        //임시
        go.transform.SetParent(pTran);
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.SetActive(true);

        return go.AddComponent<T>();
    }

    public void DestroyGameObject(GameObject go)
    {
        if (go == null)
        {
            Debug.Log("");
            return;
        }

        GameObject.Destroy(go);
        go = null;
    }
}
