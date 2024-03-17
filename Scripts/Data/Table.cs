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
    public abstract class BaseTable<TData> : ITableLoader, ITable where TData : BaseTable<TData>.BaseData
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
            public int ID           = 0;
            public int PoolAmount   = 0;
            public int KeepCount    = 0;
            public float MinSpawnTime   = 0f;
            public float MaxSpawnTime   = 0f;
            public float SpawnRadius    = 0f;
            public Vector3 SpawnPos = Vector3.zero;
            public Vector3 SpawnRot = Vector3.zero;
            public bool PoolExpand = false;
            public Define.Spawning SpawningType = Define.Spawning.None;
            public int SpawningID = 0;
            //public int EnemyID = 0;
            //public Define.WorldObject WorldObjType = Define.WorldObject.None; //임시
            //public int WorldObjID = 0; //임시
            //public Define.Actor ActorType = Define.Actor.None;
            //public int ActorID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.ID, data);
            }
        }
    }

    #region Actor

    public class EnemyTable : BaseTable<EnemyTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int ID = 0;
            public int CharacterID = 0;
            public int StatID = 0;
            public int WeaponID = 0;
            public int RewardExp = 0;
            public int RewardCoin = 0;
            //public Define.Actor actorType = Define.Actor.None;
            //public string animator = string.Empty;
            //public string avatar = string.Empty;
            //public int level = 0;
            //public int coin = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.ID, data);
            }
        }
    }

    #endregion Actor

    #region WorldObject

    public class CharacterTable : BaseTable<CharacterTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int ID = 0;
            public Define.Character CharacterType = Define.Character.None;
            public string CharacterName         = string.Empty;
            public string PrefabName            = string.Empty;
            public string Animator = string.Empty;
            public string Avatar = string.Empty;
            public string BaseIdleClip = string.Empty;
            public string BaseRunClip = string.Empty;
            public string UpperReadyClip = string.Empty;
            public string UpperAttackClip = string.Empty;
            public float RunSpeed_Temp = 0f;
            //public Define.BaseAnim initStateType = Define.BaseAnim.Idle;
            //public int statID = 0;
            //public int weaponID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.ID, data);
            }
        }
    }

    #endregion WorldObject

    public class StatTable : BaseTable<StatTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int ID = 0;
            public int MaxHp = 0;
            public int Attack = 0;
            public int Defense = 0;
            //public float moveSpeed = 0f;
            //public int maxExp = 0;
            //public int moveSpeed_levelUp_rate = 0;
            //public int defense_levelUp_rate = 0;
            //public int attack_levelUp_rate = 0;
            //public int maxHp_levelUp_rate = 0;
            //public int maxExp_levelUp_rate = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.ID, data);
            }
        }
    }

    public class WeaponTable : BaseTable<WeaponTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int ID = 0;
            public Define.WeaponType WeaponType = Define.WeaponType.Gun;
            public string PrefabName = string.Empty;
            public string WeaponParentName = string.Empty;
            public Vector3 ReadyPosition = Vector3.zero;
            public Vector3 ReadyRotation = Vector3.zero;
            public Vector3 AttackPosition = Vector3.zero;
            public Vector3 AttackRotation = Vector3.zero;
            public string SFXPrefabName = string.Empty;
            public Vector3 SFXPosition = Vector3.zero;
            public Vector3 SFXRotation = Vector3.zero;
            public int AttackValue = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.ID, data);
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

    //public class SpawningTable_Legacy : BaseTable<SpawningTable_Legacy.Data>
    //{
    //    [Serializable]
    //    public class Data : BaseData
    //    {
    //        public int ID = 0;
    //        //public Define.Spawning SpawningType = Define.Spawning.None;
    //        //public int SpawningID = 0;
    //        public int SpawnerID = 0;
    //        //public Define.Prefabs prefabType = Define.Prefabs.None;
    //        //public int prefabID = 0;
    //    }

    //    protected override void LoadTableDataDic()
    //    {
    //        Data[] datas = LoadTableDatas();
    //        foreach (Data data in datas)
    //        {
    //            baseData_dic.Add(data.ID, data);
    //        }
    //    }
    //}
}