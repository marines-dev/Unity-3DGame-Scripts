using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    public enum SpawnState
    {
        Despawn,
        Spawn,
    }

    string prefabPath = string.Empty;
    string resourceName = string.Empty;
    bool poolExpand = true;

    Dictionary<GameObject, SpawnState> objectPool_dic = new Dictionary<GameObject, SpawnState>();


    public void SetGameObjectPool(string pPrefabPath, int pSpawner_poolAmount, bool pPoolExpand, Action<GameObject> pInitGameObjectEvent = null)
    {
        prefabPath = pPrefabPath;
        poolExpand = pPoolExpand;

        GameObject go = null;
        for (int j = 0; j < pSpawner_poolAmount; j++)
        {
            go = CreateGameObject();
            Despawn(go);

            if (pInitGameObjectEvent != null)
                pInitGameObjectEvent.Invoke(go);
        }
    }

    GameObject CreateGameObject()
    {
        GameObject go = GlobalScene.ResourceMng.Instantiate(prefabPath, transform);
        
        if(! objectPool_dic.ContainsKey(go)) 
        {
            objectPool_dic.Add(go, SpawnState.Despawn);
            
        }
        else
        {
            objectPool_dic[go] = SpawnState.Despawn;
        }

        return go;
    }

    public GameObject SpawnGameObject()
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
            go = CreateGameObject();
            objectPool_dic[go] = SpawnState.Spawn;
            go.gameObject.SetActive(true);
            return go;
        }

        Debug.LogWarning($"Failed : ");
        return null;
    }

    public void Despawn(GameObject pObj)
    {
        bool despawn = false;

        if (pObj == null)
        {
            Debug.LogWarning($"Failed : ");
            return;
        }

        if (! objectPool_dic.ContainsKey(pObj) || objectPool_dic[pObj] == SpawnState.Despawn)
        {
            Debug.LogWarning($"Failed : ");
            return;
        }

        objectPool_dic[pObj] = SpawnState.Despawn;
        pObj.gameObject.SetActive(false);
    }
}

//public class ResourcesPool : MonoBehaviour { }