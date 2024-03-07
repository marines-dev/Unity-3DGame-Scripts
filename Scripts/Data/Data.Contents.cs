using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    //[Serializable]
    //public class Spawner
    //{
    //    public int spawnerID = 0;

    //    public int poolAmount = 0;
    //    public int keepCount = 0;
    //    public float minSpawnTime = 0f;
    //    public float maxSpawnTime = 0f;
    //    public float spawnRadius = 0f;
    //    public Vector3 spawnPos = Vector3.zero;
    //    public Quaternion spawnRot = Quaternion.identity;
    //    public bool isPooled = false;

    //    public int characterID = 0;
    //}

    //[Serializable]
    //public class Character
    //{
    //    public int characterID = 0;

    //    public Define.Character characterType = Define.Character.Monster;
    //    public string characterName = string.Empty;
    //    public string prefabName = string.Empty;
    //    public Define.State initState = Define.State.None;
    //    public int level = 0;
    //    public int coin = 0;
    //    public int statID = 0;
    //    public Data.Stat stat = new Data.Stat();
    //}

    //[Obsolete][Serializable]
    //public class Stat : ILoader<int, StatData>
    //{
    //    public List<StatData> stats = new List<StatData>();

    //    public Dictionary<int, StatData> MakeDict()
    //    {
    //        Dictionary<int, StatData> dict = new Dictionary<int, StatData>();
    //        foreach (StatData stat in stats)
    //            dict.Add(stat.Level, stat);
    //        return dict;
    //    }
    //}

    //[Serializable]
    //public class Stat
    //{
    //    //public int statID = 0;
    //    public int maxHp = 0;
    //    //public int hp = 0;
    //    public int attack = 0;
    //    public int defense = 0;
    //    public float moveSpeed = 0;
    //    public int maxExp = 0;
    //    //public int exp = 0;
    //}

    [Serializable]
    public class Equipment
    {
        public int id = 0;
    }
}
