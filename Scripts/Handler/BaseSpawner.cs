using System;
using Interface;
using Table;
using UnityEngine;

public class CharacterSpawner : BaseSpawner<Character> , ISpawner
{
    public static Character CreateCharacter(Define.WorldObject pWorldObjType, int pWorldObjID, Action<GameObject> pDespawnAction)
    {
        string findPrefabPath = FindPrefabPath(pWorldObjType, pWorldObjID);
        Character character = GlobalScene.ResourceMng.Instantiate(findPrefabPath).GetOrAddComponent<Character>();
        character.SetWorldObject(pWorldObjType, pWorldObjID, pDespawnAction);

        return character;
    }

    public static ISpawner CreateSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction)
    {
        CharacterSpawner spawner = Util.CreateGameObject<CharacterSpawner>();
        spawner.SetWorldSpawner(pSpawnerID, pSpawnAction, pDespawnAction);
        return spawner;
    }
}

public abstract class BaseSpawner<TWorldObj> : BaseObjectPool where TWorldObj : BaseWorldObject
{
    public bool switchPlay = false;
    public bool SwitchPooling 
    { 
        private get { return switchPlay; }
        set
        {
            if(value ==  switchPlay) 
                return;

            switchPlay = value;
            if (switchPlay)
            {
                //OnPlay();
            }
            else
            {
                //OnStop();
            }
        }
    }

    /// <summary>
    /// TableData
    /// </summary>
    SpawnerTable.Data spawnerData = null;
    //public int spawnerID;
    //public int poolAmount;
    //public int keepCount;
    //public float minSpawnTime;
    //public float maxSpawnTime;
    //public float spawnRadius;
    //public Vector3 spawnPos;
    //public Quaternion spawnRot;
    //public bool poolExpand;

    /// <summary>
    /// StateData
    /// </summary>
    int spawnCount = 0;
    int reserveCount = 0;
    float delayTime = 0f; // 임시
    bool switchSpawn = false;

    //private Action<GameObject> initAction = null;
    private Action<GameObject, Define.Actor, int> spawnAction = null; /// <GameObject, actorType, actorID>
    private Action<GameObject> despawnAction = null;

    private void Update()
    {
        if (!SwitchPooling)
            return;

        /// UpdatePooling
        if (switchSpawn == false)
        {
            bool isSpawn = reserveCount + spawnCount < spawnerData.keepCount;
            if (isSpawn)
            {
                switchSpawn = true;
                reserveCount++;
                delayTime = UnityEngine.Random.Range(spawnerData.minSpawnTime, spawnerData.maxSpawnTime);
            }
        }
        else
        {
            Invoke("Spawn", delayTime);
            switchSpawn = false;
        }
    }

    public void SetWorldSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction)
    {
        spawnerData = GlobalScene.TableMng.CreateOrGetBaseTable<SpawnerTable>().GetTableData(pSpawnerID);
        if(spawnerData == null)
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        string findPrefabPath = FindPrefabPath(spawnerData.worldObjType, spawnerData.worldObjID);
        SetObjectPool(findPrefabPath, spawnerData.poolAmount, spawnerData.poolExpand);

        ///
        spawnCount = 0;
        reserveCount = 0;
        delayTime = 0f;
        switchSpawn = false;

        ///
        //initAction = pInitAction;
        spawnAction = pSpawnAction;
        despawnAction = pDespawnAction;

        SwitchPooling = false;
    }

    protected override void SetObjectPool(string pPrefabPath, int pSpawner_poolAmount, bool pPoolExpand)
    {
        prefabPath = pPrefabPath;
        poolExpand = pPoolExpand;

        GameObject go = null;
        for (int j = 0; j < pSpawner_poolAmount; j++)
        {
            TWorldObj worldObj = CreateObject().GetOrAddComponent<TWorldObj>();
            worldObj.SetWorldObject(spawnerData.worldObjType, spawnerData.worldObjID, DespawnEvent);

            ///
            DespawnObject(go);
        }
    }

    void Spawn()
    {
        /// Pos
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        randDir.y = 0;
        Vector3 pos = spawnerData.spawnPos + randDir;
        Vector3 rot = spawnerData.spawnRot;

        TWorldObj worldObj = SpawnObject().GetOrAddComponent<TWorldObj>();
        worldObj.Spawn(pos, rot);
        
        ///
        delayTime = 0f;
        --reserveCount;
        spawnCount++;

        ///
        if (spawnAction != null)
        {
            spawnAction.Invoke(worldObj.gameObject, spawnerData.actorType, spawnerData.actorID);
        }
    }

    public void DespawnEvent(GameObject pGO)
    {
        DespawnObject(pGO);

        ///
        spawnCount--;

        ///
        if (despawnAction != null)
        {
            despawnAction.Invoke(pGO);
        }
    }

    protected static string FindPrefabPath(Define.WorldObject pWorldObjType, int pWorldObjectID)
    {
        switch (pWorldObjType)
        {
            case Define.WorldObject.Character:
                {
                    CharacterTable.Data characterData = GlobalScene.TableMng.CreateOrGetBaseTable<CharacterTable>().GetTableData(pWorldObjectID);
                    return $"Prefabs/WorldObject/{pWorldObjType.ToString()}/{characterData.prefabName}";
                }
            //case Define.WorldObject.Item:
            //    {
            //        ItemTable.Data itamData = GlobalScene.TableMng.CreateOrGetBaseTable<ItemTable>().GetTableData(pWorldObjectID);
            //        return $"Prefabs/{pWorldObjType.ToString()}/{itamData.prefabName}";
            //    }
        }

        Debug.LogWarning($"Failed : ");
        return string.Empty;
    }

    //public void StartPooling()
    //{
    //    if (pooled == false)
    //    {
    //        Debug.LogWarning($"Failed : ");
    //        return;
    //    }

    //    isPooling = data.pooled;
    //}

    //public void ReadySpawn()
    //{
    //    if (pooled)
    //    {
    //        Debug.LogWarning($"Failed : ");
    //        return;
    //    }

    //    reserveCount++;
    //    delayTime = UnityEngine.Random.Range(data.minSpawnTime, data.maxSpawnTime);
    //}

    //[Obsolete("테스트")]
    //Vector3 RandomPos()
    //{
    //    Table.Spawner.Data spawnerData = Manager.Table.GetTable<Table.Spawner>().GetTableData(2);
    //    Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
    //    randDir.y = 0;
    //    Vector3 randPos = spawnerData.spawnPos + randDir;

    //    return randPos;
    //}
}
