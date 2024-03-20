using System;
using Interface;
using Table;
using UnityEngine;
using static Define;


public class WorldSpawner : MonoBehaviour
{
    public bool SwitchPooling { private get; set; }

    private int spawnerID = 0;

    /// <summary>
    /// StateData
    /// </summary>
    private int spawnCount   = 0;
    private int reserveCount = 0;
    private float delayTime  = 0f; // 임시
    private bool isSpawn     = false;

    GameObjectPool gameObjectPool = null;


    public static GameObject CreateGameObject(Spawning pSpawningType, int pSpawningID, Vector3 pPos, Vector3 pRot, float pSpawnerRadius = 0f)
    {
        string path     = FindSpawningPath(pSpawningType, pSpawningID);
        GameObject go   = GlobalScene.Instance.InstantiateResource(path);
        SetGameObjectRandomPos(go.transform, pPos, pRot);

        return go;
    }

    public static WorldSpawner CreateSpawner(int pSpawnerID)
    {
        WorldSpawner spawner = Util.CreateGameObject<WorldSpawner>();
        spawner.SetSpawner(pSpawnerID);
        return spawner;
    }

    private void Awake()
    {
        gameObjectPool = gameObject.GetOrAddComponent<GameObjectPool>();
    }

    private void Update()
    {
        if (!SwitchPooling)
            return;

        /// UpdatePooling
        if (isSpawn == false)
        {
            SpawnerTable.Data spawnerData = GlobalScene.Instance.SpawnerTable.GetTableData(spawnerID);
            bool isSpawn = reserveCount + spawnCount < spawnerData.KeepCount;
            if (isSpawn)
            {
                this.isSpawn = true;
                reserveCount++;
                delayTime = UnityEngine.Random.Range(spawnerData.MinSpawnTime, spawnerData.MaxSpawnTime);
            }
        }
        else
        {
            Invoke("Spawn", delayTime);
            isSpawn = false;
        }
    }

    private void SetSpawner(int pSpawnerID)
    {
        spawnerID = pSpawnerID;

        SpawnerTable.Data spawnerData = GlobalScene.Instance.SpawnerTable.GetTableData(spawnerID);
        if (spawnerData == null)
        {
            Util.LogError();
            GlobalScene.Instance.DestroyGameObject(gameObject);
            return;
        }

        string path = FindSpawningPath(spawnerData.SpawningType, spawnerData.SpawningID);
        gameObjectPool.SetObjectPool(path, spawnerData.PoolAmount, spawnerData.PoolExpand);

        ///
        spawnCount      = 0;
        reserveCount    = 0;
        delayTime       = 0f;
        isSpawn         = false;

        SwitchPooling = false;
    }

    void Spawn()
    {
        SpawnerTable.Data spawnerData = GlobalScene.Instance.SpawnerTable.GetTableData(spawnerID);

        GameObject go = gameObjectPool.SpawnGameObject();
        SetGameObjectRandomPos(go.transform, spawnerData.SpawnPos, spawnerData.SpawnRot, spawnerData.SpawnRadius);
        ///
        delayTime = 0f;
        --reserveCount;
        spawnCount++;

        ///
        WorldScene.Instance.SetSpawnSpawning(go, spawnerID);
    }

    public void Despawn(GameObject pGO)
    {
        gameObjectPool.DespawnGameObject(pGO);

        ///
        spawnCount--;
    }

    private static void SetGameObjectRandomPos(Transform pTrans, Vector3 pSpawnerPos, Vector3 pSpawnerRot, float pSpawnerRadius = 0f)
    {
        if (pTrans == null)
        {
            Util.LogError();
            return;
        }

        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, pSpawnerRadius);
        randDir.y = 0;
        
        Vector3 randPos = pSpawnerPos + randDir;
        Vector3 randRot = pSpawnerRot;

        pTrans.position = randPos;
        pTrans.rotation = Quaternion.Euler(randRot);
    }

    [Obsolete("임시")]
    private static string FindSpawningPath(Define.Spawning pSpawningType, int pSpawningID)
    {
        switch (pSpawningType)
        {
            case Define.Spawning.Player:
                {
                    CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(pSpawningID);
                    return $"Prefabs/WorldObject/Character/{characterData.PrefabName}";
                }
            case Define.Spawning.Enemy:
                {
                    EnemyTable.Data enemyData = GlobalScene.Instance.EnemyTable.GetTableData(pSpawningID);
                    CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(enemyData.CharacterID);
                    return $"Prefabs/WorldObject/Character/{characterData.PrefabName}";
                }
                //case Define.Spawning.Item:
                //    {
                //        ItemTable.Data itamData = GlobalScene.Instance.ItemTable.GetTableData(pPrefabID);
                //        return $"Prefabs/WorldObject/Item/{characterData.PrefabName}";
                //    }
        }

        Util.LogWarning();
        return string.Empty;
    }
}
