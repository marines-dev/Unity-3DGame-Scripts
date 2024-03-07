using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObj : MonoBehaviour
{
    public int  spawnerID = 0;
    public int  orderedID = 0;
    public bool isSpawn = false;
}

public class Spawner : MonoBehaviour
{
    Table.Spawner.Data spawnerData = null;
    Define.Prefabs prefabType = Define.Prefabs.None;
    int prefabID    = 0;
    int orderedID_  = 0; // ※사용 금지
    int orderedID
    {
        get
        {
            ++orderedID_;
            return orderedID_;
        }
    }
    int spawnCount   = 0;
    int reserveCount = 0;

    readonly Dictionary<int, PooledObj> pooledObj_dic = new Dictionary<int, PooledObj>(); // <spawningID, spawning>

    SpawningPool spawningPool = null;
    IEnumerator updatePoolingCoroutine = null;

    /// <summary>
    /// 스폰한 오브젝트를 외부 함수로 처리할 때 사용 합니다.
    /// </summary>
    Action<PooledObj, int, int> spawnEventAction = null; 

    void Start()
    {
        spawningPool = gameObject.GetOrAddComponent<SpawningPool>();

        //spawnEventAction -= AddMonsterCount;
        //spawnEventAction += AddMonsterCount;
    }

    public void SetSpawner(int pSpawnerID, Define.Prefabs pPrefabType, int pPrefabID, Action<PooledObj, int, int> pSpawnEventAction = null)
    {
        spawningPool = gameObject.GetOrAddComponent<SpawningPool>();

        if (pSpawnerID <= 0 || pPrefabType == Define.Prefabs.None || pPrefabID <= 0)
        {
            Debug.LogWarning("");
            return;
        }

        spawnerData = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        prefabType = pPrefabType;
        prefabID = pPrefabID;
        if (pSpawnEventAction != null)
            spawnEventAction = pSpawnEventAction;

        //
        spawningPool.SetSpawingPool(spawnerData.poolAmount, prefabType, prefabID);

        //
        if (spawnerData.isPooled)
            UpdatePooling();
    }

    void UpdatePooling()
    {
        //
        if (updatePoolingCoroutine != null)
        {
            StopCoroutine(updatePoolingCoroutine);
            updatePoolingCoroutine = null;
        }
        updatePoolingCoroutine = UpdatePoolingCoroutine();
        StartCoroutine(updatePoolingCoroutine);
    }

    IEnumerator UpdatePoolingCoroutine()
    {
        spawnCount = 0;
        reserveCount = 0;
        yield return null;

        while (true)
        {
            if (reserveCount + spawnCount < spawnerData.keepCount)
            {
                SpawnCharacter();
            }
            yield return null;
        }
    }

    public void SpawnCharacter()
    {
        IEnumerator reserveCharacterCoroutine = ReserveCharacterCoroutine();
        StartCoroutine(reserveCharacterCoroutine);
    }

    IEnumerator ReserveCharacterCoroutine()
    {
        // Reserve
        reserveCount++;
        yield return new WaitForSeconds(UnityEngine.Random.Range(spawnerData.minSpawnTime, spawnerData.maxSpawnTime));

        // Spawn
        PooledObj pooledObj = spawningPool.SpawnPooledObject(spawnerData.spawnPos, Quaternion.identity).GetOrAddComponent<PooledObj>();
        pooledObj.spawnerID = spawnerData.id;
        pooledObj.orderedID = orderedID;
        pooledObj.isSpawn = true;
        pooledObj_dic.Add(pooledObj.orderedID, pooledObj);

        // SetSpawning
        if (spawnEventAction != null)
            spawnEventAction.Invoke(pooledObj, spawnerData.id, prefabID);

        //
        reserveCount--;
        spawnCount++;
    }

    public void DespawnCharacter(int pOrderedID)
    {
        PooledObj pooledObj = FindPooledObj(pOrderedID);
        pooledObj.isSpawn = false;

        spawningPool.DespawnPooledObject(pooledObj.gameObject);
        spawnCount--;

        //if (spawnEventAction != null)
        //spawnEventAction.Invoke(-1);
    }

    [Obsolete("수정 필요")]
    PooledObj FindPooledObj(int pOrderedID)
    {
        if (pooledObj_dic.ContainsKey(pOrderedID) == false)
        {
            Debug.LogWarning("");
            return null;
        }

        PooledObj pooledObj = pooledObj_dic[pOrderedID];
        if (pooledObj == null)
        {
            Debug.LogWarning("");
            return null;
        }

        return pooledObj;
    }
    //void AddSpawnCount(int value) { spawnCount += value; }
    //void SetKeepCount(int count) { keepCount = count; }
}

[Obsolete("Managers 전용 : 일반 클래스에서 사용할 수 없습니다. Managers를 이용해 주세요.")]
public class SpawnManager : BaseManager
{
    Dictionary<int, Spawner> spawner_dic = new Dictionary<int, Spawner>();


    protected override void InitDataProcess() { }
    protected override void ResetDataProcess()
    {
        DestroyAllSpawner();
    }

    public void SetPlayerSpawner()
    {
        SetCharacterSpawner(Managers.User.SpawnerID, Managers.User.PrefabType, Managers.User.CharacterID, OnSpawnedPlayerAction);
    }

    public void SetMonsterSpawner(int pSpawningID)
    {
        Table.Spawning.Data spawningData = Managers.Table.GetTable<Table.Spawning>().GetTableData(pSpawningID);

        SetCharacterSpawner(spawningData.spawnerID, spawningData.prefabType, spawningData.prefabID, OnSpawnedMonsterAction);
    }

    void SetCharacterSpawner(int pSpawnerID, Define.Prefabs pPrefabType, int pPrefabID = 0, Action<PooledObj, int, int> pSpawnedCharacterAction = null)
    {
        Spawner spawner = CreateSpawner(pSpawnerID);
        spawner.SetSpawner(pSpawnerID, pPrefabType, pPrefabID, pSpawnedCharacterAction);
    }

    public void SpawnCharacter(int pSpawnerID )
    {
        Spawner spawner = FindSpawner(pSpawnerID);
        if (spawner == null)
        {
            Debug.LogWarning($"Failed : 생성되지 않은 스포너({pSpawnerID})입니다.\n스포너를 생성한 후 스폰을 시도해주세요.");
            return;
        }

        spawner.SpawnCharacter();
    }

    public void DespawnCharacter(GameObject pDespawnedObj)
    {
        PooledObj pooledObj = GetPooledObj(pDespawnedObj);

        Spawner spawner = FindSpawner(pooledObj.spawnerID);
        spawner.DespawnCharacter(pooledObj.orderedID);
    }

    void OnSpawnedMonsterAction(PooledObj pPooledObj, int pSpawnerID, int pPrefabID)
    {
        if(pPooledObj == null)
        {
            Debug.LogWarning("");
            return;
        }

        Table.Spawner.Data   spawnerData    = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        Table.Character.Data characterData  = Managers.Table.GetTable<Table.Character>().GetTableData(pPrefabID);

        Type type = Type.GetType(characterData.characterType.ToString());
        BaseCharacter character = pPooledObj.gameObject.GetComponent(type) as BaseCharacter;
        if (character == null)
        {
            character = pPooledObj.gameObject.AddComponent(type) as BaseCharacter;
        }

        character.SpawnCharacter(characterData.id);

        // Monster
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(1, 3);
        randDir.y = 0;

        Vector3 randPos = spawnerData.spawnPos + randDir;

        character.transPosition = randPos;
        character.transRotation = Quaternion.identity;

        //while (true)
        //{
        //    randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        //    randDir.y = 0;
        //    randPos = spawnerData.spawnPos + randDir;

        //    // 갈 수 있나
        //    if (character.CalculateNavMeshPath(randPos))
        //        break;
        //}
        //character.transform.localPosition = randPos;
        //character.transform.localRotation = Quaternion.identity;
    }

    void OnSpawnedPlayerAction(PooledObj pPooledObj, int pSpawnerID, int pPrefabID)
    {
        if (pPooledObj == null)
        {
            Debug.LogWarning("");
            return;
        }

        Table.Spawner.Data   spawnerData    = Managers.Table.GetTable<Table.Spawner>().GetTableData(pSpawnerID);
        Table.Character.Data characterData  = Managers.Table.GetTable<Table.Character>().GetTableData(pPrefabID);

        Type type = Type.GetType(characterData.characterType.ToString());
        BaseCharacter character = pPooledObj.gameObject.GetComponent(type) as BaseCharacter;
        if (character == null)
        {
            character = pPooledObj.gameObject.AddComponent(type) as BaseCharacter;
        }

        character.SpawnCharacter(characterData.id);

        // Player
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0, spawnerData.spawnRadius);
        randDir.y = 0;
        Vector3 randPos = spawnerData.spawnPos + randDir;

        character.transPosition = randPos;
        character.transRotation = Quaternion.identity;

        // GameManager : 플레이어 등록
        Managers.Game.SetPalyer(character as GamePlayer);
    }

    Spawner CreateSpawner(int pSpawnerID)
    {
        DestroySpawner(pSpawnerID);

        string go_name = $"@Spawner{pSpawnerID}";
        Spawner spawner = Managers.Resource.CreateComponentObject<Spawner>(go_name);

        spawner_dic.Add(pSpawnerID, spawner);
        return spawner;
    }

    void DestroySpawner(int pSpawnerID)
    {
        Spawner spawner = FindSpawner(pSpawnerID);
        if (spawner == null)
            return;
        //
        Managers.Resource.DestroyGameObject(spawner.gameObject);
        spawner_dic.Remove(pSpawnerID);
    }

    void DestroyAllSpawner()
    {
        foreach (KeyValuePair<int, Spawner> kv in spawner_dic)
        {
            DestroySpawner(kv.Key);
        }

        spawner_dic.Clear();
    }

    Spawner FindSpawner(int pSpawnerID)
    {
        if (spawner_dic.ContainsKey(pSpawnerID) == false)
        {
            Debug.LogWarning("");
            return null;
        }

        Spawner spawner = spawner_dic[pSpawnerID];
        if (spawner == null)
        {
            Debug.LogWarning("");
            return null;
        }

        return spawner;
    }

    PooledObj GetPooledObj(GameObject pDespawnedObj)
    {
        if (pDespawnedObj == null)
        {
            Debug.LogWarning("");
            return null;
        }

        PooledObj pooledObj = pDespawnedObj.GetComponent<PooledObj>();
        if (pooledObj == null)
        {
            Debug.LogWarning("");
            return null;
        };

        return pooledObj;
    }
}
