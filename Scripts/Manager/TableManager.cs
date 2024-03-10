using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using UnityEngine;


public class TableManager : BaseManager
{
    /// <summary>
    /// Table
    /// </summary>
    private Dictionary<string, IBaseTable> baseTable_dic = new Dictionary<string, IBaseTable>();

    /// <summary>
    /// Json
    /// </summary>
    // public Dictionary<int, Data.StatData> StatDict { get; private set; } = new Dictionary<int, Data.StatData>();


    protected override void OnAwake()
    {
        //StatDict = LoadJson<Data.Stat, int, Data.StatData>("StatData_Test").MakeDict();
    }

    public override void OnReset()
    {
        // RemoveBaseTableAll
        {
            IBaseTable[] baseTable_arr = baseTable_dic.Values.ToArray();
            for (int i = 0; i < baseTable_arr.Length; ++i)
            {
                if (baseTable_arr[i] != null)
                {
                    baseTable_arr[i] = null;
                }
            }
            baseTable_dic.Clear();
        }
    }

    public TTable CreateOrGetBaseTable<TTable>() where TTable : class, IBaseTable, new()
    {
        TTable baseTable = null;
        string name = typeof(TTable).Name;
        /// Get
        {
            if (baseTable_dic.ContainsKey(name))
            {
                baseTable = baseTable_dic[name] as TTable;
                return baseTable;
            }
        }

        /// Create
        {
            string tableName = typeof(TTable).Name;
            baseTable = new TTable();

            baseTable_dic.Add(tableName, baseTable);
            return baseTable;
        }
    }

    //public TTable CreateOrGetBaseTable<TTable>() where TTable : class, IBaseTable, new()
    //{
    //    TTable baseTable = GetBaseTable(typeof(TTable)) as TTable;
    //    if (baseTable != null)
    //    {
    //        return baseTable;
    //    }

    //    // T 테이블 클래스에서 테이블 데이터를 자동 생성 합니다.
    //    string tableName = typeof(TTable).Name;
    //    TTable table = new TTable();

    //    baseTable_dic.Add(tableName, table);
    //    return table;
    //}

    //public Table.BaseTable CreateOrGetBaseTable(Type pType)
    //{
    //    if (pType == null || pType.BaseType != typeof(Table.BaseTable))
    //    {
    //        Debug.LogError("");
    //        return null;
    //    }

    //    Table.BaseTable baseTable = GetBaseTable(pType);
    //    if (baseTable != null)
    //    {
    //        return baseTable;
    //    }

    //    string tableName = typeof(Table.BaseTable).Name;
    //    baseTable = new Table.BaseTable(); // ※확인 필요

    //    string uiName = pType.Name;
    //    baseTable_dic.Add(uiName, baseTable);
    //    return baseTable;
    //}

    //TILoader LoadJson<TILoader, TKey, TValue>(string path) where TILoader : ILoader<TKey, TValue>
    //{
    //    TextAsset textAsset = Managers.Resource.LoadResource<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<TILoader>(textAsset.text);
    //}
}