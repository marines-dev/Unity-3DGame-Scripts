using System;
using System.Text;
using UnityEngine;
using BackEnd;
using ServerData;

namespace ServerData
{
    public class UserData
    {
        // Player (총 7개)
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
    [Obsolete("테스트 : 데이터 디버깅")]
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
        //    result.AppendLine($"| {itemKey} : {test_inventory[itemKey]}개");
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
    // 변경할 Hp 값을 검사하여 유저 Hp 스탯에 저장합니다. 반환된 값으로 다시 사용해주세요.
    /// </summary>
    public int SetUserHP(int pHpValue)
    {
        if (pHpValue == UserData.hpValue)
        {
            Debug.LogWarning($"Faild : 변경하려는 Exp({pHpValue}) 값이 유저의 현재 Exp({UserData.hpValue})와 같으므로 변경할 수 없습니다.");
            return UserData.hpValue;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1);
        if (pHpValue > statData.maxHp)
        {
            Debug.Log($"Warning : HP({pHpValue}) 값 초과로 MaxHP({statData.maxHp})으로 저장합니다.");
            UserData.hpValue  = statData.maxHp;
        }
        else if(pHpValue < 0)
        {
            Debug.Log($"Warning : HP({pHpValue}) 값 초과로 0으로 저장합니다.");
            UserData.hpValue = 0;
        }
        else
        {
            UserData.hpValue = pHpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"Success : 유저의 변경된 hpValue는 {UserData.hpValue}입니다.");
        return UserData.hpValue;
    }

    /// <summary>
    // 변경할 Exp 값을 검사하여 유저 Exp 스탯에 저장합니다. 반환된 값으로 다시 사용해주세요.
    /// </summary>
    public int SetUserExp(int pExpValue)
    {
        if (pExpValue <= UserData.expValue)
        {
            Debug.LogWarning($"Faild : 변경하려는 Exp({pExpValue}) 값이 유저의 현재 Exp({UserData.expValue})와 작거나 같으므로 변경할 수 없습니다.");
            return UserData.expValue;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1); //임시
        if (pExpValue > statData.maxExp)
        {
            Debug.Log($"Warning : exp({pExpValue}) 값 초과로 MaxExp({statData.maxExp})으로 저장합니다.");
            Debug.Log($"Warning : 유저의 레벨을 증가해 주세요!");
            UserData.expValue = statData.maxExp;
        }
        else if (pExpValue < 0)
        {
            Debug.Log($"Warning : exp({pExpValue}) 값 초과로 0으로 저장합니다.");
            UserData.expValue = 0;
        }
        else
        {
            UserData.expValue = pExpValue;
        }

        //
        UpdateUserData();

        //
        Debug.Log($"Success : 유저의 변경된 Exp는 {UserData.expValue}입니다.");
        return UserData.expValue;
    }

    [Obsolete("임시")]
    public int SetUserLevelUp()
    {
        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(UserData.characterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1);
        if (UserData.expValue < statData.maxExp)
        {
            Debug.LogWarning($"Failed : 유저의 Exp({UserData.expValue})가 MaxExp({statData.maxExp})보다 작아 LevelUp할 수 없습니다.");
            return UserData.levelValue;
        }

        UserData.levelValue += 1;
        UserData.expValue = 0;

        //
        UpdateUserData();

        //
        Debug.Log($"Success : 유저의 변경된 levelValue는 {UserData.levelValue}입니다.");
        Debug.Log($"Success : 유저의 expValue가 {UserData.expValue}으로 초기화되었습니다. 변경된 값으로 적용해 주세요.");
        return UserData.levelValue;
    }

    [Obsolete("임시")]
    public void SetUserWeaponID(int pWeaponID)
    {
        if (pWeaponID < 0) // Table 범위 검사 필요
        {
            Debug.LogWarning("Failed : ");
            return;
        }

        UserData.weaponID = pWeaponID;

        //
        UpdateUserData();
    }

    //[Obsolete("임시")]
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

    [Obsolete("임시")]
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

    [Obsolete("테스트 필요 : 서버 저장 방식 확인")]
    public void SaveUserData() // Backend 데이터에 저장합니다.
    {
        if (UserData == null)
        {
            Debug.LogError("Failed : 서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Init 혹은 Load를 통해 데이터를 생성해주세요.");
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
        //userData.info = "친추는 언제나 환영입니다.";
        //userData.equipment.Add("전사의 투구");
        //userData.inventory.Add("빨간포션", 1);
        //Managers.Backend.SaveBackendData<string>("level", test_info);
        //Managers.Backend.SaveBackendData<Dictionary<string, int>>("level", test_inventory);
        //Managers.Backend.SaveBackendData<List<string>>("level", test_equipment);

        // SaveBackend
        BackendMng.SaveBackendData(Config.user_tableName, ref param);
    }

    [Obsolete("테스트 필요 : 서버 저장 방식 확인")]
    public void UpdateUserData() // Backend 데이터에 저장합니다.
    {
        if (UserData == null)
        {
            Debug.LogError($"Failed : 서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Init 혹은 Load를 통해 데이터를 생성해주세요.");
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

    public bool LoadUserData() // Backend 데이터를 불러옵니다.
    {
        LitJson.JsonData gameDataJson = BackendMng.LoadBackendData(Config.user_tableName);
        if (gameDataJson == null)
        {
            Debug.LogWarning($"유저 데이터가 존재하지 않습니다.");
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

