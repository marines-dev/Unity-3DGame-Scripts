using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;

//public interface ILoader<TKey, TValue>
//{
//    Dictionary<TKey, TValue> MakeDict();
//}

namespace Table
{
    public abstract class BaseTable<TData> : ITableLoader where TData : BaseTable<TData>.BaseData
    {
        [Serializable]
        public abstract class BaseData
        {
            public TData Copy()
            {
                return this.MemberwiseClone() as TData;
            }
        }

        private TextAsset textAsset = null;

        // <id, BaseData>
        protected Dictionary<int, TData> baseData_dic = new Dictionary<int, TData>();

        public BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터를 생성하였습니다.");
            //LoadTableDataDic();
        }

        ~BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터 삭제");
            ClearTableDataDic();
        }

        public void Initialized(TextAsset pTextAsset)
        {
            if (pTextAsset == null)
            {
                Util.LogError();
                return;
            }

            textAsset = pTextAsset;
            LoadTableDataDic();
        }

        protected abstract void LoadTableDataDic();

        protected TData[] LoadTableDatas()
        {
            ClearTableDataDic();

            //string tableName    = this.GetType().Name;
            //string path         = $"Data/Table/{tableName}";
            //textAsset = ResourceManager.Instance.Load<TextAsset>(path);
            if (textAsset == null)
            {
                Util.LogError();
                return null;
            }

            TData[] _baseDatas = CSVSerializer.DeserializeData<TData>(textAsset.text);
            if (_baseDatas == null || _baseDatas.Length <= 0)
            {
                Util.LogError();
                return null;
            }

            return _baseDatas;
        }

        void ClearTableDataDic()
        {
            if (baseData_dic != null && baseData_dic.Count > 0)
            {
                baseData_dic.Clear();
            }
        }


        public TData GetTableData(int pID)
        {
            if (baseData_dic.ContainsKey(pID) == false)
            {
                Util.LogWarning();
                return null;
            }

            return baseData_dic[pID].Copy();
        }

        public TData[] GetTableDatasAll()
        {
            TData[] tableDatas = baseData_dic.Values.ToArray();
            TData[] newTableDatas = new TData[tableDatas.Length];
            for(int i = 0; i < newTableDatas.Length; ++i)
            {
                newTableDatas[i] = tableDatas[i].Copy();
            }

            return newTableDatas;
        }
    }

    public class SpawnerTable : BaseTable<SpawnerTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id           = 0;
            public int poolAmount   = 0;
            public int keepCount    = 0;
            public float minSpawnTime   = 0f;
            public float maxSpawnTime   = 0f;
            public float spawnRadius    = 0f;
            public Vector3 spawnPos = Vector3.zero;
            public Vector3 spawnRot = Vector3.zero;
            public bool poolExpand = false;
            public Define.WorldObject worldObjType = Define.WorldObject.None; //임시
            public int worldObjID = 0; //임시
            public Define.Actor actorType = Define.Actor.None;
            public int actorID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);
            }
        }
    }

    public class CharacterTable : BaseTable<CharacterTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public Define.Character characterType = Define.Character.None;
            public string characterName         = string.Empty;
            public string prefabName            = string.Empty;
            public string animator = string.Empty;
            public string avatar = string.Empty;
            public string upperReadyClip = string.Empty;
            public string upperAttackClip = string.Empty;
            public int statID = 0;
            public int weaponID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);
            }
        }
    }

    public class StatTable : BaseTable<StatTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public int maxHp = 0;
            public int maxHp_levelUp_rate = 0;
            public int attack = 0;
            public int attack_levelUp_rate = 0;
            public int defense = 0;
            public int defense_levelUp_rate = 0;
            public float moveSpeed = 0f;
            public int moveSpeed_levelUp_rate = 0;
            public int maxExp = 0;
            public int maxExp_levelUp_rate = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);
            }
        }
    }

    public class EquipmentTable : BaseTable<EquipmentTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public Define.WeaponType weaponType = Define.WeaponType.Gun;
            public string prefabName = string.Empty;
            public string equipParentName = string.Empty;
            public Vector3 readyPosition = Vector3.zero;
            public Vector3 readyRotation = Vector3.zero;
            public Vector3 attackPosition = Vector3.zero;
            public Vector3 attackRotation = Vector3.zero;
            public string sfxPrefabName = string.Empty;
            public Vector3 sfxPosition = Vector3.zero;
            public Vector3 sfxRotation = Vector3.zero;
            public int attackValue = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);
            }
        }
    }

    //public class SampleTable : BaseTable<SampleTable.Data>
    //{
    //    [Serializable]
    //    public class Data : BaseData
    //    {
    //        public int id = 0;
    //        public float key1 = 0f;
    //        public string key2 = string.Empty;
    //    }

    //    protected override void LoadTableDataDic()
    //    {
    //        Data[] datas = LoadTableDatas();
    //        foreach (Data data in datas)
    //        {
    //            baseData_dic.Add(data.id, data);
    //        }
    //    }

    //    //public Data GetTableData(int pID)
    //    //{
    //    //    Data baseData = GetBaseData(pID);
    //    //    if (baseData == null)
    //    //    {
    //    //        Debug.LogWarning($"Failed : ");
    //    //        return null;
    //    //    }

    //    //    Data data = baseData.Copy() as Data; //객체 복사
    //    //    return data;
    //    //}
    //}

    //public class ActorTable : BaseTable<ActorTable.Data>
    //{
    //    [Serializable]
    //    public class Data : BaseData
    //    {
    //        public int id = 0;
    //        public Define.Actor actorType = Define.Actor.None;
    //        public string animatorController = string.Empty;
    //        public string animatorAvatar = string.Empty;
    //        public Define.BaseAnim initStateType = Define.BaseAnim.Idle;
    //        //public int level = 0;
    //        //public int coin = 0;
    //        //public int statID = 0;
    //        public int weaponID = 0;
    //    }

    //    protected override void LoadTableDataDic()
    //    {
    //        Data[] datas = LoadTableDatas();
    //        foreach (Data data in datas)
    //        {
    //            baseData_dic.Add(data.id, data);
    //        }
    //    }
    //}

    //public class SpawningTable_Legacy : BaseTable<SpawningTable_Legacy.Data>
    //{
    //    [Serializable]
    //    public class Data : BaseData
    //    {
    //        public int id = 0;
    //        public int spawnerID = 0;
    //        public Define.Prefabs prefabType = Define.Prefabs.None;
    //        public int prefabID = 0;
    //    }

    //    protected override void LoadTableDataDic()
    //    {
    //        Data[] datas = LoadTableDatas();
    //        foreach (Data data in datas)
    //        {
    //            baseData_dic.Add(data.id, data);

    //            Debug.Log($"Success({this.GetType()})\n"
    //            + $"id : {data.id}\n"
    //            + $"spawnerID : {data.spawnerID}\n"
    //            + $"prefabType : {data.prefabType}\n"
    //            + $"prefabID : {data.prefabID}\n");
    //        }
    //    }
    //}
}