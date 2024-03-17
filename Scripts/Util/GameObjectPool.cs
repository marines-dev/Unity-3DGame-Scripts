using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameObjectPool : MonoBehaviour
{
    string prefabPath = string.Empty;
    bool poolExpand = true;

    Dictionary<GameObject, ExistenceState> objectPool_dic = new Dictionary<GameObject, ExistenceState>();


    public void SetObjectPool(string pPrefabPath, int pSpawner_poolAmount, bool pPoolExpand, Action<GameObject> pInitGameObjectEvent = null)
    {
        prefabPath = pPrefabPath;
        poolExpand = pPoolExpand;

        GameObject go = null;
        for (int j = 0; j < pSpawner_poolAmount; j++)
        {
            go = CreateObject();
            DespawnGameObject(go);

            if (pInitGameObjectEvent != null)
                pInitGameObjectEvent.Invoke(go);
        }
    }

    GameObject CreateObject()
    {
        GameObject go = GlobalScene.Instance.InstantiateResource(prefabPath, transform);

        if (!objectPool_dic.ContainsKey(go))
        {
            objectPool_dic.Add(go, ExistenceState.Despawn);
        }
        else
        {
            objectPool_dic[go] = ExistenceState.Despawn;
        }

        return go;
    }

    public GameObject SpawnGameObject()
    {
        GameObject go = null;
        foreach (KeyValuePair<GameObject, ExistenceState> pair in objectPool_dic)
        {
            if (pair.Value == ExistenceState.Despawn)
            {
                go = pair.Key;
                break;
            }
        }

        if (go != null)
        {
            objectPool_dic[go] = ExistenceState.Spawn;
            go.gameObject.SetActive(true);
            go.transform.parent = null;
            return go;
        }

        if (go == null && poolExpand)
        {
            go = CreateObject();
            objectPool_dic[go] = ExistenceState.Spawn;
            go.gameObject.SetActive(true);
            go.transform.parent = null;
            return go;
        }

        Util.LogWarning();
        return null;
    }

    public void DespawnGameObject(GameObject pObj)
    {
        bool despawn = false;

        if (pObj == null)
        {
            Util.LogWarning();
            return;
        }

        if (!objectPool_dic.ContainsKey(pObj) || objectPool_dic[pObj] == ExistenceState.Despawn)
        {
            Util.LogWarning();
            return;
        }

        objectPool_dic[pObj] = ExistenceState.Despawn;
        pObj.gameObject.SetActive(false);
        pObj.transform.parent = transform;
    }
}

//public class ResourcesPool : MonoBehaviour { }