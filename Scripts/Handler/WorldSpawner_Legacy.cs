//using System;
//using Interface;
//using Table;
//using UnityEngine;

////public class CharacterSpawner : CharacterSpawner<Enemy>
////{
////    public static Player CreateCharacter(Define.WorldObject pWorldObjType, int pWorldObjID, Action<GameObject> pDespawnAction)
////    {
////        string findPrefabPath = FindPrefabPath(pWorldObjType, pWorldObjID);
////        Player character = GlobalScene.Instance.InstantiateResource(findPrefabPath).GetOrAddComponent<Player>();
////        character.SetWorldObject(pWorldObjType, pWorldObjID, pDespawnAction);

////        return character;
////    }

////    public static ISpawner CreateSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction)
////    {
////        CharacterSpawner spawner = Util.CreateGameObject<CharacterSpawner>();
////        spawner.SetWorldSpawner(pSpawnerID, pSpawnAction, pDespawnAction);
////        return spawner;
////    }
////}

//public class WorldSpawner_Legacy : BaseObjectPool, ISpawner
//{
//    public bool SwitchPooling { private get; set; }

//    /// <summary>
//    /// TableData
//    /// </summary>
//    EnemySpawnerTable.Data spawnerData = null;

//    /// <summary>
//    /// StateData
//    /// </summary>
//    int spawnCount = 0;
//    int reserveCount = 0;
//    float delayTime = 0f; // 임시
//    bool isSpawn = false;

//    //private Action<GameObject> initAction = null;
//    private Action<GameObject, int> spawnAction = null; /// <GameObject, SpawnerID>
//    private Action<GameObject> despawnAction = null;


//    public static GameObject CreateWorldObject(Define.WorldObject pWorldObjType, int pWorldObjID, Vector3 pPos, Vector3 pRot, float pSpawnerRadius = 0f)
//    {
//        string findPrefabPath = FindPrefabPath(pWorldObjType, pWorldObjID);
//        GameObject go = GlobalScene.Instance.InstantiateResource(findPrefabPath);
//        //character.SetCharacter(pWorldObjType, pWorldObjID, pDespawnAction);
//        SetWorldObjectRandomPos(go.transform, pPos, pRot);

//        return go;
//    }

//    public static ISpawner CreateSpawner(int pSpawnerID, Action<GameObject, int> pSpawnAction, Action<GameObject> pDespawnAction)
//    {
//        WorldSpawner_Legacy spawner = Util.CreateGameObject<WorldSpawner_Legacy>();
//        spawner.SetWorldSpawner(pSpawnerID, pSpawnAction, pDespawnAction);
//        return spawner;
//    }

//    private void Update()
//    {
//        if (!SwitchPooling)
//            return;

//        /// UpdatePooling
//        if (isSpawn == false)
//        {
//            bool isSpawn = reserveCount + spawnCount < spawnerData.KeepCount;
//            if (isSpawn)
//            {
//                this.isSpawn = true;
//                reserveCount++;
//                delayTime = UnityEngine.Random.Range(spawnerData.MinSpawnTime, spawnerData.MaxSpawnTime);
//            }
//        }
//        else
//        {
//            Invoke("Spawn", delayTime);
//            isSpawn = false;
//        }
//    }

//    public void SetWorldSpawner(int pSpawnerID, Action<GameObject, int> pSpawnAction, Action<GameObject> pDespawnAction)
//    {
//        spawnerData = WorldScene.Instance.SpawnerTable.GetTableData(pSpawnerID);
//        if(spawnerData == null)
//        {
//            Util.LogWarning();
//            return;
//        }

//        string findPrefabPath = FindPrefabPath(spawnerData.WorldObjType, spawnerData.WorldObjID);
//        SetObjectPool(findPrefabPath, spawnerData.PoolAmount, spawnerData.PoolExpand);

//        ///
//        spawnCount = 0;
//        reserveCount = 0;
//        delayTime = 0f;
//        isSpawn = false;

//        ///
//        //initAction = pInitAction;
//        spawnAction = pSpawnAction;
//        despawnAction = pDespawnAction;

//        SwitchPooling = false;
//    }

//    protected override void SetObjectPool(string pPrefabPath, int pSpawner_poolAmount, bool pPoolExpand)
//    {
//        prefabPath = pPrefabPath;
//        poolExpand = pPoolExpand;

//        GameObject go = null;
//        for (int j = 0; j < pSpawner_poolAmount; j++)
//        {
//            //Character character = CreateObject().GetOrAddComponent<Character>();
//            //character.SetCharacter(spawnerData.worldObjType, spawnerData.worldObjID, DespawnEvent);

//            ///
//            DespawnObject(go);
//        }
//    }

//    void Spawn()
//    {
//        GameObject go = SpawnObject();
//        SetWorldObjectRandomPos(go.transform, spawnerData.SpawnPos, spawnerData.SpawnRot, spawnerData.SpawnRadius);

//        ///
//        delayTime = 0f;
//        --reserveCount;
//        spawnCount++;

//        ///
//        if (spawnAction != null)
//        {
//            spawnAction.Invoke(go, spawnerData.ID);
//        }
//    }

//    public void DespawnEvent(GameObject pGO)
//    {
//        DespawnObject(pGO);

//        ///
//        spawnCount--;

//        ///
//        if (despawnAction != null)
//        {
//            despawnAction.Invoke(pGO);
//        }
//    }

//    protected static string FindPrefabPath(Define.WorldObject pWorldObjType, int pWorldObjectID)
//    {
//        switch (pWorldObjType)
//        {
//            case Define.WorldObject.Character:
//                {
//                    CharacterTable.Data characterData = WorldScene.Instance.CharacterTable.GetTableData(pWorldObjectID);
//                    return $"Prefabs/WorldObject/{pWorldObjType.ToString()}/{characterData.PrefabName}";
//                }
//            //case Define.WorldObject.Item:
//            //    {
//            //        ItemTable.Data itamData = GlobalScene.TableMng.CreateOrGetBaseTable<ItemTable>().GetTableData(pWorldObjectID);
//            //        return $"Prefabs/{pWorldObjType.ToString()}/{itamData.prefabName}";
//            //    }
//        }

//        Util.LogWarning();
//        return string.Empty;
//    }

//    private static void SetWorldObjectRandomPos(Transform pTrans, Vector3 pSpawnerPos, Vector3 pSpawnerRot, float pSpawnerRadius = 0f)
//    {
//        if(pTrans == null)
//        {
//            Util.LogError();
//            return;
//        }

//        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, pSpawnerRadius);
//        randDir.y = 0;
//        Vector3 randPos = pSpawnerPos + randDir;
//        Vector3 randRot = pSpawnerRot;

//        pTrans.position = randPos;
//        pTrans.rotation = Quaternion.Euler(randRot);
//    }

//    //public void ReadySpawn()
//    //{
//    //    if (pooled)
//    //    {
//    //        Util.LogWarning();
//    //        return;
//    //    }

//    //    reserveCount++;
//    //    delayTime = UnityEngine.Random.Range(data.minSpawnTime, data.maxSpawnTime);
//    //}

//    //[Obsolete("테스트")]
//    //Vector3 RandomPos()
//    //{
//    //    Table.Spawner.Data spawnerData = Manager.Table.GetTable<Table.Spawner>().GetTableData(2);
//    //    Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
//    //    randDir.y = 0;
//    //    Vector3 randPos = spawnerData.spawnPos + randDir;

//    //    return randPos;
//    //}
//}
