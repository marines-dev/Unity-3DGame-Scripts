using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPool : BaseHandler
{
    protected string prefabPath = string.Empty;
    protected bool poolExpand = true;

    Dictionary<GameObject, Define.SpawnState> objectPool_dic = new Dictionary<GameObject, Define.SpawnState>();


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
        GameObject go = ResourceManager.Instance.Instantiate(prefabPath, transform);

        if (!objectPool_dic.ContainsKey(go))
        {
            objectPool_dic.Add(go, Define.SpawnState.Despawn);

        }
        else
        {
            objectPool_dic[go] = Define.SpawnState.Despawn;
        }

        return go;
    }

    public virtual GameObject SpawnObject()
    {
        GameObject go = null;
        foreach (KeyValuePair<GameObject, Define.SpawnState> pair in objectPool_dic)
        {
            if (pair.Value == Define.SpawnState.Despawn)
            {
                go = pair.Key;
            }
        }

        if (go != null)
        {
            objectPool_dic[go] = Define.SpawnState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        if (go == null && poolExpand)
        {
            go = CreateObject();
            objectPool_dic[go] = Define.SpawnState.Spawn;
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

        if (!objectPool_dic.ContainsKey(pObj) || objectPool_dic[pObj] == Define.SpawnState.Despawn)
        {
            Debug.LogWarning($"Failed : ");
        }

        objectPool_dic[pObj] = Define.SpawnState.Despawn;
        pObj.gameObject.SetActive(false);
    }
}