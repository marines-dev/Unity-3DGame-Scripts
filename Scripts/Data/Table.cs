using System;
using System.Collections.Generic;
using UnityEngine;

//public interface ILoader<TKey, TValue>
//{
//    Dictionary<TKey, TValue> MakeDict();
//}

namespace Table
{
    public abstract class BaseTable
    {
        [Serializable]
        public abstract class BaseData
        {
            public BaseData Copy()
            {
                return (BaseData)this.MemberwiseClone();
            }
        }

        // <id, BaseData>
        protected Dictionary<int, BaseData> baseData_dic = new Dictionary<int, BaseData>();

        public BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터 로드");
            LoadTableDataDic();
        }
        ~BaseTable()
        {
            Debug.Log($"Success : {this.GetType().Name} 테이블 데이터 삭제");
            ClearBaseDataDic();
        }

        protected abstract void LoadTableDataDic();

        protected T[] LoadBaseDatas<T>() where T : BaseData
        {
            ClearBaseDataDic();

            string tableName    = this.GetType().Name;
            string path         = $"Data/Table/{tableName}";
            TextAsset textAsset = GlobalScene.ResourceMng.Load<TextAsset>(path);
            if (textAsset == null)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }

            T[] _baseDatas = CSVSerializer.DeserializeData<T>(textAsset.text);
            if (_baseDatas == null || _baseDatas.Length <= 0)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }

            return _baseDatas;
        }
        void ClearBaseDataDic()
        {
            if (baseData_dic != null && baseData_dic.Count > 0)
            {
                baseData_dic.Clear();
            }
        }

        protected BaseData GetBaseData(int pId)
        {
            if (baseData_dic == null || baseData_dic.ContainsKey(pId) == false)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }

            return baseData_dic[pId];
        }
    }

    //public class Sample : BaseTable
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
    //        Data[] datas = LoadBaseDatas<Data>();
    //        foreach (Data data in datas)
    //        {
    //            baseData_dic.Add(data.id, data);
    //        }
    //    }

    //    public Data GetTableData(int pID)
    //    {
    //        BaseData baseData = GetBaseData(pID);
    //        if (baseData == null)
    //        {
    //            Debug.LogWarning($"Failed : ");
    //            return null;
    //        }

    //        Data data = baseData.Copy() as Data; //객체 복사
    //        return data;
    //    }
    //}

    public class Spawning : BaseTable
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id           = 0;
            public int spawnerID    = 0;
            public Define.Prefabs prefabType = Define.Prefabs.None;
            public int prefabID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadBaseDatas<Data>();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);

                Debug.Log($"Success({this.GetType()})\n"
                + $"id : {data.id}\n"
                + $"spawnerID : {data.spawnerID}\n"
                + $"prefabType : {data.prefabType}\n"
                + $"prefabID : {data.prefabID}\n");
            }
        }

        public Data GetTableData(int pID)
        {
            BaseData baseData = GetBaseData(pID);
            if (baseData == null)
            {
                Debug.LogWarning($"Failed : ");
                return null;
            }

            Data data = baseData.Copy() as Data; //객체 복사
            return data;
        }
    }
    public class Spawner : BaseTable
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
            public Vector3      spawnPos = Vector3.zero;
            public Quaternion   spawnRot = Quaternion.identity;
            public bool isPooled = false;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadBaseDatas<Data>();
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
                        + $"isPooled : {data.isPooled}\n");
            }
        }

        public Data GetTableData(int pID)
        {
            BaseData baseData = GetBaseData(pID);
            if (baseData == null)
            {
                Debug.LogWarning($"Failed : ");
                return null;
            }

            Data data = baseData.Copy() as Data; //객체 복사
            return data;
        }
    }
    public class Character : BaseTable
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public Define.Character characterType = Define.Character.None;
            public string characterName         = string.Empty;
            public string prefabName            = string.Empty;
            public string animatorController    = string.Empty;
            public string animatorAvatar        = string.Empty;
            public Define.BaseState initStateType = Define.BaseState.None;
            public int level    = 0;
            public int coin     = 0;
            public int statID   = 0;
            public int weaponID = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadBaseDatas<Data>();
            foreach (Data data in datas)
            {
                baseData_dic.Add(data.id, data);

                Debug.Log($"Success({this.GetType()})\n"
                    + $"id : {data.id}\n"
                    + $"characterType : {data.characterType}\n"
                    + $"characterName : {data.characterName}\n"
                    + $"prefabName : {data.prefabName}\n"
                    + $"animatorController : {data.animatorController}\n"
                    + $"animatorAvatar : {data.animatorAvatar}\n"
                    + $"initStateType : {data.initStateType}\n"
                    + $"level : {data.level}\n"
                    + $"coin : {data.coin}\n"
                    + $"statID : {data.statID}\n"
                    + $"weaponID : {data.weaponID}\n");
            }
        }

        public Data GetTableData(int pID)
        {
            BaseData baseData = GetBaseData(pID);
            if (baseData == null)
            {
                Debug.LogWarning($"Failed : ");
                return null;
            }

            Data data = baseData.Copy() as Data; //객체 복사
            return data;
        }
    }
    public class Stat : BaseTable
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id                       = 0;
            public int maxHp                    = 0;
            public int maxHp_levelUp_rate       = 0;
            public int attack                   = 0;
            public int attack_levelUp_rate      = 0;
            public int defense                  = 0;
            public int defense_levelUp_rate     = 0;
            public float moveSpeed              = 0f;
            public int moveSpeed_levelUp_rate   = 0;
            public int maxExp                   = 0;
            public int maxExp_levelUp_rate      = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadBaseDatas<Data>();
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

        public Data GetTableData(int pID)
        {
            BaseData baseData = GetBaseData(pID);
            if (baseData == null)
            {
                Debug.LogWarning($"Failed : ");
                return null;
            }

            Data data = baseData.Copy() as Data; //객체 복사
            return data;
        }
    }
    public class Equipment : BaseTable
    {
        [Serializable]
        public class Data : BaseData
        {
            public int id = 0;
            public Define.WeaponType weaponType = Define.WeaponType.Gun;
            public string prefabName        = string.Empty;
            public string equipParentName   = string.Empty;
            public Vector3 readyPosition    = Vector3.zero;
            public Vector3 readyRotation    = Vector3.zero;
            public Vector3 attackPosition   = Vector3.zero;
            public Vector3 attackRotation   = Vector3.zero;
            public string sfxPrefabName = string.Empty;
            public Vector3 sfxPosition = Vector3.zero;
            public Vector3 sfxRotation = Vector3.zero;
            public int attackValue = 0;
        }

        protected override void LoadTableDataDic()
        {
            Data[] datas = LoadBaseDatas<Data>();
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

        public Data GetTableData(int pID)
        {
            BaseData baseData = GetBaseData(pID);
            if (baseData == null)
            {
                Debug.LogWarning($"Failed : ");
                return null;
            }

            Data data = baseData.Copy() as Data; //객체 복사
            return data;
        }
    }
}