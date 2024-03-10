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
    public abstract class BaseTable<TData> : IBaseTable where TData : BaseTable<TData>.BaseData
    {
        [Serializable]
        public abstract class BaseData
        {
            public TData Copy()
            {
                return this.MemberwiseClone() as TData;
            }
        }

        // <id, BaseData>
        protected Dictionary<int, TData> baseData_dic = new Dictionary<int, TData>();

        public BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터 로드");
            LoadTableDataDic();
        }

        ~BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터 삭제");
            ClearTableDataDic();
        }

        protected abstract void LoadTableDataDic();

        protected TData[] LoadTableDatas()
        {
            ClearTableDataDic();

            string tableName    = this.GetType().Name;
            string path         = $"Data/Table/{tableName}";
            TextAsset textAsset = GlobalScene.ResourceMng.Load<TextAsset>(path);
            if (textAsset == null)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }

            TData[] _baseDatas = CSVSerializer.DeserializeData<TData>(textAsset.text);
            if (_baseDatas == null || _baseDatas.Length <= 0)
            {
                Debug.LogWarning("Failed : ");
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
                Debug.LogWarning("Failed : ");
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

        //protected TData LoadBaseData(int pId)
        //{
        //    if (baseData_dic.ContainsKey(pId) == false)
        //    {
        //        Debug.LogWarning("Failed : ");
        //        return null;
        //    }

        //    return baseData_dic[pId].Copy();
        //}
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

                Debug.Log($"Success({this.GetType()})\n"
                        + $"id : {data.id}\n"
                        + $"poolAmount : {data.poolAmount}\n"
                        + $"keepCount : {data.keepCount}\n"
                        + $"minSpawnTime : {data.minSpawnTime}\n"
                        + $"maxSpawnTime : {data.maxSpawnTime}\n"
                        + $"spawnRadius : {data.spawnRadius}\n"
                        + $"spawnPos : {data.spawnPos}\n"
                        + $"spawnRot : {data.spawnRot}\n"
                        + $"poolExpand : {data.poolExpand}\n"
                        + $"worldObjType : {data.worldObjType}\n"
                        + $"worldObjID : {data.worldObjID}\n"
                        + $"actorType : {data.actorType}\n"
                        + $"actorID : {data.actorID}\n");
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
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);

                Debug.Log($"Success({this.GetType()})\n"
                    + $"id : {data.id}\n"
                    + $"characterType : {data.characterType}\n"
                    + $"characterName : {data.characterName}\n"
                    + $"prefabName : {data.prefabName}\n");
            }
        }
    }

    public class ActorTable : BaseTable<ActorTable.Data>
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public Define.Actor actorType = Define.Actor.None;
            public string animatorController = string.Empty;
            public string animatorAvatar = string.Empty;
            public Define.BaseAnim initStateType = Define.BaseAnim.Idle;
            //public int level = 0;
            //public int coin = 0;
            //public int statID = 0;
            public int weaponID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadTableDatas();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);

                Debug.Log($"Success({this.GetType()})\n"
                    + $"id : {data.id}\n"
                    + $"animatorController : {data.animatorController}\n"
                    + $"animatorAvatar : {data.animatorAvatar}\n"
                    + $"initStateType : {data.initStateType}\n"
                    + $"weaponID : {data.weaponID}\n");
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

                Debug.Log($"Success({this.GetType()})\n"
                    + $"id : {data.id}\n"
                    + $"maxHp : {data.maxHp}\n"
                    + $"maxHp_levelUp_rate : {data.maxHp_levelUp_rate}\n"
                    + $"attack : {data.attack}\n"
                    + $"attack_levelUp_rate : {data.attack_levelUp_rate}\n"
                    + $"defense : {data.defense}\n"
                    + $"defense_levelUp_rate : {data.defense_levelUp_rate}\n"
                    + $"moveSpeed : {data.moveSpeed}\n"
                    + $"moveSpeed_levelUp_rate : {data.moveSpeed_levelUp_rate}\n"
                    + $"maxExp : {data.maxExp}\n"
                    + $"maxExp_levelUp_rate : {data.maxExp_levelUp_rate}\n");
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

                Debug.Log($"Success({this.GetType()})\n"
                    + $"id : {data.id}\n"
                    + $"weaponType : {data.weaponType}\n"
                    + $"prefabName : {data.prefabName}\n"
                    + $"equipParentName : {data.equipParentName}\n"
                    + $"readyPosition : {data.readyPosition}\n"
                    + $"readyRotation : {data.readyRotation}\n"
                    + $"attackPosition : {data.attackPosition}\n"
                    + $"attackRotation : {data.attackRotation}\n"
                    + $"sfxPrefabName : {data.sfxPrefabName}\n"
                    + $"sfxPosition : {data.sfxPosition}\n"
                    + $"sfxRotation : {data.sfxRotation}\n"
                    + $"attackValue : {data.attackValue}\n");
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