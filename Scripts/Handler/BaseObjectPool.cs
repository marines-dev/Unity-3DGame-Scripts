using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPool : BaseHandler
{
    protected string prefabPath = string.Empty;
    protected bool poolExpand = true;

    Dictionary<GameObject, Define.ExistenceState> objectPool_dic = new Dictionary<GameObject, Define.ExistenceState>();


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
        GameObject go = GlobalScene.Instance.InstantiateResource(prefabPath, transform);

        if (!objectPool_dic.ContainsKey(go))
        {
            objectPool_dic.Add(go, Define.ExistenceState.Despawn);

        }
        else
        {
            objectPool_dic[go] = Define.ExistenceState.Despawn;
        }

        return go;
    }

    public virtual GameObject SpawnObject()
    {
        GameObject go = null;
        foreach (KeyValuePair<GameObject, Define.ExistenceState> pair in objectPool_dic)
        {
            if (pair.Value == Define.ExistenceState.Despawn)
            {
                go = pair.Key;
            }
        }

        if (go != null)
        {
            objectPool_dic[go] = Define.ExistenceState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        if (go == null && poolExpand)
        {
            go = CreateObject();
            objectPool_dic[go] = Define.ExistenceState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        Util.LogWarning();
        return null;
    }

    public virtual void DespawnObject(GameObject pObj)
    {
        if (pObj == null)
        {
            Util.LogWarning();
            return;
        }

        if (!objectPool_dic.ContainsKey(pObj) || objectPool_dic[pObj] == Define.ExistenceState.Despawn)
        {
            Util.LogWarning();
        }

        objectPool_dic[pObj] = Define.ExistenceState.Despawn;
        pObj.gameObject.SetActive(false);
    }
}