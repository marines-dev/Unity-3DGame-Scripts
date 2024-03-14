using System;
using System.Text;
using UnityEngine;
using BackEnd;
using ServerData;

namespace ServerData
{
    public class UserData
    {
        // Player (�� 7��)
        public int spawnerID = 0;
        public Define.Prefabs prefabType = Define.Prefabs.None;
        public int  characterID = 0;
        public int  levelValue  = 0;
        public int  expValue    = 0;
        public int  coinValue   = 0;
        public int  hpValue     = 0;
        public int  weaponID    = 0;
    }
}


public class UserManager : Manager
{
    // Backend
    string InData { get { return BackendMng.GetInData(); } }
    string NickName { get { return BackendMng.GetNickname(); } }

    // ServerData.UserData
    public ServerData.UserData UserData { get; private set; } = new ServerData.UserData();
    //public int SpawnerID { get { return UserData.spawnerID; } }
    //public Define.Prefabs PrefabType { get { return UserData.prefabType; } }
    //public int CharacterID { get { return UserData.characterID; } }
    //public int LevelValue { get { return UserData.levelValue; } }
    //public int ExpValue { get { return UserData.expValue; } }
    //public int CoinValue { get { return UserData.coinValue; } }
    //public int HpValue { get { return UserData.hpValue; } }
    //public int WeaponID { get { return UserData.weaponID; } }


    protected override void OnInitialized()
    {
        if (UserData == null)
            UserData = new ServerData.UserData();
    }

    public override void OnRelease() { }

    // Debug.Log(ToString())
    [Obsolete("�׽�Ʈ : ������ �����")]
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        // User
        result.AppendLine($"inData : {InData.ToString()}");
        result.AppendLine($"nickName : {NickName.ToString()}");

        // Player
        result.AppendLine($"spawnerID : {UserData.spawnerID.ToString()}");
        result.AppendLine($"characterID : {UserData.characterID.ToString()}");
        result.AppendLine($"currentLevel : {UserData.levelValue.ToString()}");
        result.AppendLine($"currentExp : {UserData.expValue.ToString()}");
        result.AppendLine($"currentCoin : {UserData.coinValue.ToString()}");
        result.AppendLine($"currentHP : {UserData.hpValue.ToString()}");
        result.AppendLine($"weaponID : {UserData.weaponID.ToString()}");

        //result.AppendLine($"inventory");
        //foreach (var itemKey in test_inventory.Keys)
        //{
        //    result.AppendLine($"| {itemKey} : {test_inventory[itemKey]}��");
        //}
        //result.AppendLine($"equipment");
        //foreach (var equip in test_equipment)
        //{
        //    result.AppendLine($"| {equip}");
        //}

        return result.ToString();
    }

    //ServerData.UserData GetUserData() 
    //{
    //    ServerData.UserData _userData = new ServerData.UserData();

    //    // GamePlayer
    //    _userData.spawnerID = userData.spawnerID;
    //    _userData.prefabType = userData.prefabType;
    //    _userData.characterID = userData.characterID;
    //    _userData.levelValue = userData.levelValue;
    //    _userData.expValue = userData.expValue;
    //    _userData.coinValue = userData.coinValue;
    //    _userData.hpValue = userData.hpValue;

    //    return _userData; 
    //}

    /// <summary>
    // ������ Hp ���� �˻��Ͽ� ���� Hp ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public int SetUserHP(int pHpValue)
    {
        if (pHpValue == UserData.hpValue)
        {
            Debug.LogWarning($"Faild : �����Ϸ��� Exp({pHpValue}) ���� ������ ���� Exp({UserData.hpValue})�� �����Ƿ� ������ �� �����ϴ�.");
            return UserData.hpValue;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1);
        if (pHpValue > statData.maxHp)
        {
            Debug.Log($"Warning : HP({pHpValue}) �� �ʰ��� MaxHP({statData.maxHp})���� �����մϴ�.");
            UserData.hpValue  = statData.maxHp;
        }
        else if(pHpValue < 0)
        {
            Debug.Log($"Warning : HP({pHpValue}) �� �ʰ��� 0���� �����մϴ�.");
            UserData.hpValue = 0;
        }
        else
        {
            UserData.hpValue = pHpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"Success : ������ ����� hpValue�� {UserData.hpValue}�Դϴ�.");
        return UserData.hpValue;
    }

    /// <summary>
    // ������ Exp ���� �˻��Ͽ� ���� Exp ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public int SetUserExp(int pExpValue)
    {
        if (pExpValue <= UserData.expValue)
        {
            Debug.LogWarning($"Faild : �����Ϸ��� Exp({pExpValue}) ���� ������ ���� Exp({UserData.expValue})�� �۰ų� �����Ƿ� ������ �� �����ϴ�.");
            return UserData.expValue;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1); //�ӽ�
        if (pExpValue > statData.maxExp)
        {
            Debug.Log($"Warning : exp({pExpValue}) �� �ʰ��� MaxExp({statData.maxExp})���� �����մϴ�.");
            Debug.Log($"Warning : ������ ������ ������ �ּ���!");
            UserData.expValue = statData.maxExp;
        }
        else if (pExpValue < 0)
        {
            Debug.Log($"Warning : exp({pExpValue}) �� �ʰ��� 0���� �����մϴ�.");
            UserData.expValue = 0;
        }
        else
        {
            UserData.expValue = pExpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"Success : ������ ����� Exp�� {UserData.expValue}�Դϴ�.");
        return UserData.expValue;
    }

    [Obsolete("�ӽ�")]
    public int SetUserLevelUp()
    {
        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1);
        if (UserData.expValue < statData.maxExp)
        {
            Debug.LogWarning($"Failed : ������ Exp({UserData.expValue})�� MaxExp({statData.maxExp})���� �۾� LevelUp�� �� �����ϴ�.");
            return UserData.levelValue;
        }

        UserData.levelValue += 1;
        UserData.expValue = 0;

        //
        UpdateUserData();

        //
        Debug.Log($"Success : ������ ����� levelValue�� {UserData.levelValue}�Դϴ�.");
        Debug.Log($"Success : ������ expValue�� {UserData.expValue}���� �ʱ�ȭ�Ǿ����ϴ�. ����� ������ ������ �ּ���.");
        return UserData.levelValue;
    }

    [Obsolete("�ӽ�")]
    public void SetUserWeaponID(int pWeaponID)
    {
        if (pWeaponID < 0) // Table ���� �˻� �ʿ�
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        UserData.weaponID = pWeaponID;

        //
        UpdateUserData();
    }

    //[Obsolete("�ӽ�")]
    //public void SetUserCoin(int pValue)
    //{
    //    //
    //    int coinValue = userData.coinValue + pValue;
    //    if (coinValue < 0)
    //    {
    //        return;
    //    }

    //    userData.coinValue = coinValue;

    //    //
    //    UpdateUserData();
    //}

    #region ServerLoad

    [Obsolete("�ӽ�")]
    public void CreateUserData()
    {
        // ServerData.UserData
        UserData.spawnerID      = Config.gamePlayer_spawnerID;
        UserData.prefabType     = Config.gamePlayer_prefabType;
        UserData.characterID    = Config.gamePlayer_characterID;
        UserData.levelValue     = Config.gamePlayer_levelUpCount;
        UserData.expValue       = Config.gamePlayer_expValue;
        UserData.coinValue      = Config.gamePlayer_coinValue;
        UserData.hpValue        = Config.gamePlayer_hpValue;
        UserData.weaponID       = Config.gamePlayer_weaponID;

        //
        SaveUserData();
    }

    [Obsolete("�׽�Ʈ �ʿ� : ���� ���� ��� Ȯ��")]
    public void SaveUserData() // Backend �����Ϳ� �����մϴ�.
    {
        if (UserData == null)
        {
            Debug.LogError("Failed : �������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Init Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param();

        // ServerData.UserData
        param.Add("spawnerID", UserData.spawnerID.ToString());
        param.Add("prefabType", UserData.prefabType.ToString());
        param.Add("characterID", UserData.characterID.ToString());
        param.Add("levelUpCount", UserData.levelValue.ToString());
        param.Add("expValue", UserData.expValue.ToString());
        param.Add("coinValue", UserData.coinValue.ToString());
        param.Add("hpValue", UserData.hpValue.ToString());
        param.Add("weaponID", UserData.weaponID.ToString());

        // Test
        //userData.info = "ģ�ߴ� ������ ȯ���Դϴ�.";
        //userData.equipment.Add("������ ����");
        //userData.inventory.Add("��������", 1);
        //Managers.Backend.SaveBackendData<string>("level", test_info);
        //Managers.Backend.SaveBackendData<Dictionary<string, int>>("level", test_inventory);
        //Managers.Backend.SaveBackendData<List<string>>("level", test_equipment);

        // SaveBackend
        BackendMng.SaveBackendData(Config.user_tableName, ref param);
    }

    [Obsolete("�׽�Ʈ �ʿ� : ���� ���� ��� Ȯ��")]
    public void UpdateUserData() // Backend �����Ϳ� �����մϴ�.
    {
        if (UserData == null)
        {
            Debug.LogError($"Failed : �������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Init Ȥ�� Load�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param();

        // ServerData.UserData
        param.Add("spawnerID", UserData.spawnerID.ToString());
        param.Add("prefabType", UserData.prefabType.ToString());
        param.Add("characterID", UserData.characterID.ToString());
        param.Add("levelUpCount", UserData.levelValue.ToString());
        param.Add("expValue", UserData.expValue.ToString());
        param.Add("coinValue", UserData.coinValue.ToString());
        param.Add("hpValue", UserData.hpValue.ToString());
        param.Add("weaponID", UserData.weaponID.ToString());

        // UpdateBackend
        BackendMng.UpdateBackendData(Config.user_tableName, ref param);
    }

    public bool LoadUserData() // Backend �����͸� �ҷ��ɴϴ�.
    {
        LitJson.JsonData gameDataJson = BackendMng.LoadBackendData(Config.user_tableName);
        if (gameDataJson == null)
        {
            Debug.LogWarning($"���� �����Ͱ� �������� �ʽ��ϴ�.");
            return false;
        }

        UserData.spawnerID = int.Parse(gameDataJson[0]["spawnerID"].ToString());
        UserData.prefabType = (Define.Prefabs)Enum.Parse(typeof(Define.Prefabs), gameDataJson[0]["prefabType"].ToString());
        UserData.characterID = int.Parse(gameDataJson[0]["characterID"].ToString());
        UserData.levelValue = int.Parse(gameDataJson[0]["levelUpCount"].ToString());
        UserData.expValue = int.Parse(gameDataJson[0]["expValue"].ToString());
        UserData.coinValue = int.Parse(gameDataJson[0]["coinValue"].ToString());
        UserData.hpValue = int.Parse(gameDataJson[0]["hpValue"].ToString());
        UserData.weaponID = int.Parse(gameDataJson[0]["weaponID"].ToString());

        //Test
        //userData.info = gameDataJson[0]["info"].ToString();
        //foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
        //{
        //    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
        //}
        //foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
        //{
        //    userData.equipment.Add(equip.ToString());
        //}

        //
        Debug.Log(ToString());
        return true;
    }

    #endregion ServerLoad
}

