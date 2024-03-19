using System;
using System.Text;
using UnityEngine;
using BackEnd;
using ServerData;
using System.Linq;


namespace ServerData
{
    public class UserData
    {
        public int CharacterID = 0;
        public int StatID = 0;
        public int WeaponID = 0;
        public int CurrHP = 0;
        public int LevelValue = 0;
        public int ExpValue = 0;
        public int CoinValue = 0;
        public Vector3 SpawnPos = Vector3.zero;
        public Vector3 SpawnRot = Vector3.zero;

        public UserData Copy()
        {
            return this.MemberwiseClone() as UserData;
        }
    }
}

public class UserManager : Manager
{
    // Backend
    private string InData { get { return BackendMng.GetInData(); } }
    private string NickName { get { return BackendMng.GetNickname(); } }

    private UserData userData = new UserData();
    public UserData UserData {  get { return userData.Copy(); } }

    protected override void OnInitialized() { }
    public override void OnRelease() { }


    /// <summary>
    // 변경할 CurrHPValue 값을 검사하여 유저 CurrHPValue 스탯에 저장합니다. 반환된 값으로 다시 사용해주세요.
    /// </summary>
    public int UpdateUserData_CurrHPValue(int pCurrHP)
    {
        if (pCurrHP == userData.CurrHP)
        {
            Util.LogWarning($"변경하려는 Exp({pCurrHP}) 값이 유저의 현재 Exp({userData.CurrHP})와 같습니다.");
            return userData.CurrHP;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(userData.CharacterID);
        Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(userData.StatID);
        if (pCurrHP > statData.MaxHp)
        {
            Util.LogWarning($"HP({pCurrHP}) 값 초과로 MaxHP({statData.MaxHp})으로 저장합니다.");
            userData.CurrHP = statData.MaxHp;
        }
        else if (pCurrHP < 0)
        {
            Util.LogWarning($"HP({pCurrHP}) 값 초과로 0으로 저장합니다.");
            userData.CurrHP = 0;
        }
        else
        {
            userData.CurrHP = pCurrHP;
        }

        //
        UpdateUserData();

        //
        Util.LogSuccess($"유저의 변경된 hpValue는 {userData.CurrHP}입니다.");
        return userData.CurrHP;
    }

    /// <summary>
    // 변경할 Exp 값을 검사하여 유저 Exp 스탯에 저장합니다. 반환된 값으로 다시 사용해주세요.
    /// </summary>
    public int UpdateUserData_ExpValue(int pExpValue)
    {
        if (pExpValue <= userData.ExpValue)
        {
            Util.LogWarning($"변경하려는 ExpValue({pExpValue}) 값이 유저의 현재 ExpValue({userData.ExpValue})와 작거나 같으므로 변경할 수 없습니다.");
            return userData.ExpValue;
        }

        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(userData.CharacterID);
        //Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1); //임시
        int maxExp = Config.User_MaxExp_init;
        if (pExpValue > maxExp)
        {
            Util.LogSuccess($"ExpValue({pExpValue}) 값 초과로 MaxExp({maxExp})으로 저장합니다.\n유저의 레벨을 증가해 주세요!");
            userData.ExpValue = maxExp;
        }
        else if (pExpValue < 0)
        {
            Util.LogWarning($"ExpValue({pExpValue}) 값은 0 미만일 수 없습니다.");
            userData.ExpValue = 0;
        }
        else
        {
            userData.ExpValue = pExpValue;
        }

        //
        UpdateUserData();

        //
        Util.LogSuccess($"유저의 변경된 ExpValue는 {userData.ExpValue}입니다.");
        return userData.ExpValue;
    }

    [Obsolete("임시")]
    public int UpdateUserData_LevelUp()
    {
        Table.CharacterTable.Data characterData = TableMng.CreateOrGetBaseTable<Table.CharacterTable>().GetTableData(userData.CharacterID);
        //Table.StatTable.Data statData = TableMng.CreateOrGetBaseTable<Table.StatTable>().GetTableData(1);
        int maxExp = 500;
        if (userData.ExpValue < maxExp)
        {
            Util.LogWarning($"유저의 ExpValue({userData.ExpValue})가 MaxExp({maxExp})보다 작아 LevelUp할 수 없습니다.");
            return userData.LevelValue;
        }

        userData.LevelValue += 1;
        userData.ExpValue = 0;

        //
        UpdateUserData();

        //
        Util.LogSuccess($"유저의 변경된 LevelValue {userData.LevelValue}입니다.");
        Util.LogSuccess($"유저의 ExpValue {userData.ExpValue}으로 초기화되었습니다. 변경된 값으로 적용해 주세요.");
        return userData.LevelValue;
    }

    public void UpdateUserData_WeaponID(int pWeaponID)
    {
        if (pWeaponID < 1) // Table 범위 검사
        {
            Util.LogWarning();
            return;
        }

        userData.WeaponID = pWeaponID;

        //
        UpdateUserData();
    }

    //[Obsolete("임시")]
    //public void UpdateUserData_CoinValue(int pValue)
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

    public void LoadUserData() // Backend 데이터를 불러옵니다.
    {
        LitJson.JsonData gameDataJson = BackendMng.LoadBackendData(Config.User_TableName);
        if (gameDataJson == null)
        {
            Util.LogWarning($"유저 데이터가 존재하지 않습니다. 새로운 유저 데이터를 생성합니다.");

            /// CreateUserData
            {
                // ServerData.UserData
                userData.CharacterID = Config.UserDataValue.CharacterID;
                userData.StatID = Config.UserDataValue.StatID;
                userData.WeaponID = Config.UserDataValue.WeaponID;
                userData.CurrHP = Config.UserDataValue.CurrHP;
                userData.LevelValue = Config.UserDataValue.LevelValue;
                userData.ExpValue = Config.UserDataValue.ExpValue;
                userData.CoinValue = Config.UserDataValue.CoinValue;
                userData.SpawnPos = Config.UserDataValue.SpawnPos;
                userData.SpawnRot = Config.UserDataValue.SpawnRot;

                /// SaveUserData
                Param param = AddServerData();
                BackendMng.SaveBackendData(Config.User_TableName, ref param);

                /// Reload
                gameDataJson = BackendMng.LoadBackendData(Config.User_TableName);
                if (gameDataJson == null)
                {
                    Util.LogError();
                    return;
                }
            }
        }

        ///
        userData.CharacterID = StringToInt(gameDataJson[0]["CharacterID"].ToString());
        userData.StatID = StringToInt(gameDataJson[0]["StatID"].ToString());
        userData.WeaponID = StringToInt(gameDataJson[0]["WeaponID"].ToString());
        userData.CurrHP = StringToInt(gameDataJson[0]["CurrHP"].ToString());
        userData.LevelValue = StringToInt(gameDataJson[0]["LevelValue"].ToString());
        userData.ExpValue = StringToInt(gameDataJson[0]["ExpValue"].ToString());
        userData.CoinValue = StringToInt(gameDataJson[0]["CoinValue"].ToString());
        userData.SpawnPos = StringToVector3(gameDataJson[0]["SpawnPos"].ToString());
        userData.SpawnRot = StringToVector3(gameDataJson[0]["SpawnRot"].ToString());

        //userData.info = gameDataJson[0]["info"].ToString();
        //foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
        //{
        //    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
        //}
        //foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
        //{
        //    userData.equipment.Add(equip.ToString());
        //}

        Util.LogSuccess($"유저 데이터 로드를 성공했습니다. \n{ToString()}");
    }

    private void UpdateUserData() // Backend 데이터에 저장합니다.
    {
        Param param = AddServerData();

        /// UpdateBackend
        BackendMng.UpdateBackendData(Config.User_TableName, ref param);
    }

    private Param AddServerData()
    {
        Param param = new Param();

        param.Add("CharacterID", userData.CharacterID.ToString());
        param.Add("StatID", userData.StatID.ToString());
        param.Add("WeaponID", userData.WeaponID.ToString());
        param.Add("CurrHP", userData.CurrHP.ToString());
        param.Add("LevelValue", userData.LevelValue.ToString());
        param.Add("ExpValue", userData.ExpValue.ToString());
        param.Add("CoinValue", userData.CoinValue.ToString());
        param.Add("SpawnPos", userData.SpawnPos.ToString());
        param.Add("SpawnRot", userData.SpawnRot.ToString());

        return param;
    }

    #endregion ServerLoad

    #region Parse

    public int StringToInt(string pStr)
    {
        return int.Parse(pStr);
    }

    public Vector3 StringToVector3(string pStr)
    {
        pStr = pStr.Replace("(", "");
        pStr = pStr.Replace(")", "");

        Vector3 vec = Vector3.zero;
        float[] elements = pStr.Split(',').Select(x => float.Parse(x)).ToArray();
        if (elements != null && elements.Length == 3)
        {
            vec = new Vector3(elements[0], elements[1], elements[2]);
        }

        return vec;
    }

    #endregion Parse

    // Debug.Log(ToString())
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        // User
        result.AppendLine($"inData : {InData.ToString()}");
        result.AppendLine($"nickName : {NickName.ToString()}");

        // Player
        result.AppendLine($"CharacterID : {userData.CharacterID.ToString()}");
        result.AppendLine($"StatID : {userData.StatID.ToString()}");
        result.AppendLine($"WeaponID : {userData.WeaponID.ToString()}");
        result.AppendLine($"CurrHP : {userData.CurrHP.ToString()}");
        result.AppendLine($"LevelValue : {userData.LevelValue.ToString()}");
        result.AppendLine($"CoinValue : {userData.CoinValue.ToString()}");
        result.AppendLine($"SpawnPos : {userData.SpawnPos.ToString()}");
        result.AppendLine($"SpawnRot : {userData.SpawnRot.ToString()}");

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
}

