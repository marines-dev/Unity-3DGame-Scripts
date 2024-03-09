//using System;
//using System.Collections.Generic;
//using UnityEngine;


//public class WorldSpawner : MonoBehaviour // 인터페이스로 변경 고려
//{
//    [Serializable]
//    public struct Data
//    {
//        public int spawnerID;
//        public string prefabPath;
//        public int poolAmount;
//        public int keepCount;
//        public float minSpawnTime;
//        public float maxSpawnTime;
//        public float spawnRadius;
//        public Vector3 spawnPos;
//        public Quaternion spawnRot;
//        public bool poolExpand;
//    }
//    Data data;

//    int spawnCount = 0;
//    int reserveCount = 0;
//    float delayTime = 0f; // 임시
//    bool isPooling = false;
//    bool doSpawn = false;

//    private Dictionary<int, WorldSpawner> worldObjectPool_dic = new Dictionary<int, WorldSpawner>();

//    private Action<GameObject> initAction = null;
//    private Action<GameObject, int> spawnAction = null; /// <GameObject, spawnerID>
//    private Action<GameObject> despawnAction = null;

//    private GameObjectPool gameObjectPool = null;


//    public static WorldSpawner CreateWorldSpawner(int pSpawnerID, Action<GameObject> pInitAction, Action<GameObject, int> pSpawnAction, Action<GameObject> pDespawnAction)
//    {
//        if (pSpawnerID <= 0)
//        {
//            Debug.LogWarning($"Failed : ");
//            return null;
//        }

//        WorldSpawner worldSpawner = Util.CreateGameObject<WorldSpawner>();
//        /// SetSpawner
//        {
//            Table.Spawner.Data spawnerData = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Spawner>().GetTableData(pSpawnerID);
//            worldSpawner.spawnerID = ;
//            worldSpawner.data.prefabPath = ;
//            worldSpawner.data.poolAmount = spawnerData.poolAmount;
//            worldSpawner.data.keepCount = spawnerData.keepCount;
//            worldSpawner.data.minSpawnTime = spawnerData.minSpawnTime;
//            worldSpawner.data.maxSpawnTime = spawnerData.maxSpawnTime;
//            worldSpawner.data.spawnRadius = spawnerData.spawnRadius;
//            worldSpawner.data.spawnPos = spawnerData.spawnPos;
//            worldSpawner.data.spawnRot = spawnerData.spawnRot;
//            worldSpawner.data.poolExpand = spawnerData.poolExpand;

//            ///
//            worldSpawner.spawnCount = 0;
//            worldSpawner.reserveCount = 0;
//            worldSpawner.delayTime = 0f;
//            worldSpawner.doSpawn = false;

//            ///
//            worldSpawner.initAction = pInitAction;
//            worldSpawner.spawnAction = pSpawnAction;
//            worldSpawner.despawnAction = pDespawnAction;

//            ///
//            worldSpawner.gameObjectPool.SetGameObjectPool(data.prefabType, data.prefabName, data.poolAmount, data.poolExpand, pInitAction);
//        }

//        return worldSpawner;
//    }

//    private void Awake()
//    {
//        gameObjectPool = gameObject.GetOrAddComponent<GameObjectPool>();
//    }

//    private void Update()
//    {
//        if (isPooling == false)
//            return;

//        /// UpdatePooling
//        if (doSpawn == false)
//        {
//            bool isSpawn = reserveCount + spawnCount < data.keepCount;
//            if (isSpawn)
//            {
//                doSpawn = true;
//                reserveCount++;
//                delayTime = UnityEngine.Random.Range(data.minSpawnTime, data.maxSpawnTime);
//            }
//        }
//        else
//        {
//            Invoke("Spawn", delayTime);
//            doSpawn = false;
//        }
//    }

//    void Spawn()
//    {
//        GameObject go = gameObjectPool.SpawnGameObject();

//        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, data.spawnRadius);
//        randDir.y = 0;
//        Vector3 pos = data.spawnPos + randDir;
//        Vector3 rot = data.spawnRot;

//        go.transform.localPosition = pos;
//        go.transform.localRotation = Quaternion.Euler(rot);

//        delayTime = 0f;
//        --reserveCount;
//        spawnCount++;

//        //
//        if (spawnAction != null)
//        {
//            spawnAction.Invoke(go, spawnID);
//        }
//    }

//    public void Despawn(GameObject pGO)
//    {
//        if (pGO == null)
//        {
//            Debug.LogWarning("Failed : ");
//            return;
//        }

//        gameObjectPool.Despawn(pGO);

//        spawnCount--;

//        if (despawnAction != null)
//        {
//            despawnAction.Invoke(pGO);
//        }
//    }

//    //public void SetSpawner(int pSpawnerID, Action<GameObject> pInitAction, Action<GameObject, int> pSpawnAction, Action<GameObject> pDespawnAction)
//    //{
//    //    if (pSpawnerID <= 0)
//    //    {
//    //        Debug.LogWarning($"Failed : ");
//    //        return;
//    //    }

//    //    Table.Spawner.Data spawnerData = Manager_Legacy.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
//    //    data.prefabType = GetResourceType(spawnerData.spawningType);
//    //    data.prefabName = GetResourceName(spawnerData.spawningType, spawnerData.spawningID);
//    //    data.poolAmount = spawnerData.poolAmount;
//    //    data.keepCount = spawnerData.keepCount;
//    //    data.minSpawnTime = spawnerData.minSpawnTime;
//    //    data.maxSpawnTime = spawnerData.maxSpawnTime;
//    //    data.spawnRadius = spawnerData.spawnRadius;
//    //    data.spawnPos = spawnerData.spawnPos;
//    //    data.spawnRot = spawnerData.spawnRot;
//    //    data.pooled = spawnerData.pooled;
//    //    data.poolExpand = spawnerData.poolExpand;

//    //    //
//    //    spawnCount = 0;
//    //    reserveCount = 0;
//    //    delayTime = 0f;
//    //    doSpawn = false;

//    //    initAction = pInitAction;
//    //    spawnAction = pSpawnAction;
//    //    despawnAction = pDespawnAction;

//    //    gameObjectPool.SetGameObjectPool(data.prefabType, data.prefabName, data.poolAmount, data.poolExpand, pInitAction);
//    //}

//    //public void StartPooling()
//    //{
//    //    if (pooled == false)
//    //    {
//    //        Debug.LogWarning($"Failed : ");
//    //        return;
//    //    }

//    //    isPooling = data.pooled;
//    //}

//    //public void ReadySpawn()
//    //{
//    //    if (pooled)
//    //    {
//    //        Debug.LogWarning($"Failed : ");
//    //        return;
//    //    }

//    //    reserveCount++;
//    //    delayTime = UnityEngine.Random.Range(data.minSpawnTime, data.maxSpawnTime);
//    //}
//}
