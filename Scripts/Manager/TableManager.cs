using System;
using System.Collections.Generic;
using UnityEngine;


[Obsolete("Managers ���� : �Ϲ� Ŭ�������� ����� �� �����ϴ�. Managers�� �̿��� �ּ���.")]
public class TableManager : BaseManager
{
    /// <summary>
    /// Table
    /// </summary>
    Dictionary<string, Table.BaseTable> table_dic = new Dictionary<string, Table.BaseTable>();

    /// <summary>
    /// Json
    /// </summary>
    // public Dictionary<int, Data.StatData> StatDict { get; private set; } = new Dictionary<int, Data.StatData>();


    protected override void InitDataProcess()
    {
        //spawning = new Table.Spawning();
        //spawner = new Table.Spawner();
        //character = new Table.Character();
        //stat = new Table.Stat();

        //StatDict = LoadJson<Data.Stat, int, Data.StatData>("StatData_Test").MakeDict();
    }

    protected override void ResetDataProcess()
    {
        DeleteTable<Table.Spawning>();
        DeleteTable<Table.Spawner>();
        DeleteTable<Table.Character>();
        DeleteTable<Table.Stat>();
    }

    public void LoadTable<T>() where T : Table.BaseTable, new()
    {
        if(ContainsTable<T>())
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        // T ���̺� Ŭ�������� ���̺� �����͸� �ڵ� ���� �մϴ�.
        string tableName = typeof(T).Name;
        T table = new T();

        table_dic.Add(tableName, table);
    }

    void DeleteTable<T>() where T : Table.BaseTable, new()
    {
        if (ContainsTable<T>() == false)
            return;

        // Ŭ�������� ���̺� �����͸� �ڵ� ���� �մϴ�.
        string tableName = typeof(T).Name;
        table_dic.Remove(tableName);
    }

    bool ContainsTable<T>() where T : Table.BaseTable, new()
    {
        string tableName = typeof(T).Name;
        bool containsKey = table_dic != null && table_dic.ContainsKey(tableName);
        return containsKey;
    }

    public T GetTable<T>() where T : Table.BaseTable, new()
    {
        if (ContainsTable<T>() == false)
        {
            Debug.LogWarning("Failed : ");
            return default(T);
        }

        string tableName = typeof(T).Name;
        T talbe = table_dic[tableName] as T;
        return talbe;
    }

    //TILoader LoadJson<TILoader, TKey, TValue>(string path) where TILoader : ILoader<TKey, TValue>
    //{
    //    TextAsset textAsset = Managers.Resource.LoadResource<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<TILoader>(textAsset.text);
    //}
}