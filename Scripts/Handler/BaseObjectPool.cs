using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPool : MonoBehaviour
{
    public enum SpawnState
    {
        Despawn,
        Spawn,
    }

    protected string prefabPath = string.Empty;
    protected bool poolExpand = true;

    Dictionary<GameObject, SpawnState> objectPool_dic = new Dictionary<GameObject, SpawnState>();


    protected virtual void SetObjectPool(string pPrefabPath, int pSpawner_poolAmount, bool pPoolExpand)
    {
        prefabPath = pPrefabPath;
        poolExpand = pPoolExpand;

        GameObject go = null;
        for (int j = 0; j < pSpawner_poolAmount; j++)
        {
            go = CreateObject();
            DespawnObject(go);
        }
    }

    protected virtual GameObject CreateObject()
    {
        GameObject go = GlobalScene.ResourceMng.Instantiate(prefabPath, transform);

        if (!objectPool_dic.ContainsKey(go))
        {
            objectPool_dic.Add(go, SpawnState.Despawn);

        }
        else
        {
            objectPool_dic[go] = SpawnState.Despawn;
        }

        return go;
    }

    public virtual GameObject SpawnObject()
    {
        GameObject go = null;
        foreach (KeyValuePair<GameObject, SpawnState> pair in objectPool_dic)
        {
            if (pair.Value == SpawnState.Despawn)
            {
                go = pair.Key;
            }
        }

        if (go != null)
        {
            objectPool_dic[go] = SpawnState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        if (go == null && poolExpand)
        {
            go = CreateObject();
            objectPool_dic[go] = SpawnState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        Debug.LogWarning($"Failed : ");
        return null;
    }

    public virtual void DespawnObject(GameObject pObj)
    {
        if (pObj == null)
        {
            Debug.LogWarning($"Failed : ");
            return;
        }

        if (!objectPool_dic.ContainsKey(pObj) || objectPool_dic[pObj] == SpawnState.Despawn)
        {
            Debug.LogWarning($"Failed : ");
        }

        objectPool_dic[pObj] = SpawnState.Despawn;
        pObj.gameObject.SetActive(false);
    }
}