using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interface;

public static class TableLoader
{
    public static TTable CreateBaseTable<TTable>() where TTable : class, IBaseTable, new()
    {
        TTable baseTable = null;
        string tableName = typeof(TTable).Name;
        string path = $"Data/Table/{tableName}";
        TextAsset textAsset = ResourceLoader.Load<TextAsset>(path);

        baseTable = new TTable();
        baseTable.Initialized(textAsset);
        return baseTable;
    }
}

public abstract class BaseTable<TData> : IBaseTable where TData : BaseTable<TData>.BaseData
{
    [Serializable]
    public abstract class BaseData
    {
        public TData Copy() { return this.MemberwiseClone() as TData; }
    }

    private TextAsset textAsset = null;

    /// <id, BaseData>
    protected Dictionary<int, TData> baseData_dic = new Dictionary<int, TData>();

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
        /// ClearTableDataDic
        if (baseData_dic != null && baseData_dic.Count > 0)
        {
            baseData_dic.Clear();
        }

        /// New
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

    //void ClearTableDataDic() { }


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
        return tableDatas.Select(x => x.Copy()).ToArray();
    }
}
