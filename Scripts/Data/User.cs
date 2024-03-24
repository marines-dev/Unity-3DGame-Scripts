using System;
using System.Text;
using UnityEngine;
using BackEnd;
using ServerData;
using System.Linq;
using Server;


namespace ServerData
{
    public class UserData
    {
        public int CharacterID  = 0;
        public int StatID       = 0;
        public int WeaponID     = 0;
        public int CurrHP       = 0;
        public int LevelValue   = 0;
        public int ExpValue     = 0;
        public int CoinValue    = 0;
        public Vector3 SpawnPos = Vector3.zero;
        public Vector3 SpawnRot = Vector3.zero;

        public UserData Copy() { return this.MemberwiseClone() as UserData; }
    }
}

public static class User
{
    // Backend
    //private static string InData   { get { return BackendManager.GetInData(); } }
    //private static string NickName { get { return BackendManager.GetNickname(); } }

    private static  UserData userData = new UserData();
    public static   UserData UserData {  get { return userData.Copy(); } }

    /// <summary>
    // ������ CurrHPValue ���� �˻��Ͽ� ���� CurrHPValue ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public static int UpdateUserData_CurrHPValue(int pCurrHP)
    {
        if (pCurrHP == userData.CurrHP)
        {
            Util.LogWarning($"�����Ϸ��� Exp({pCurrHP}) ���� ������ ���� Exp({userData.CurrHP})�� �����ϴ�.");
            return userData.CurrHP;
        }

        CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(userData.CharacterID);
        StatTable.Data      statData      = GlobalScene.Instance.StatTable.GetTableData(userData.StatID);
        if (pCurrHP > statData.MaxHp)
        {
            Util.LogWarning($"HP({pCurrHP}) �� �ʰ��� MaxHP({statData.MaxHp})���� �����մϴ�.");
            userData.CurrHP = statData.MaxHp;
        }
        else if (pCurrHP < 0)
        {
            Util.LogWarning($"HP({pCurrHP}) �� �ʰ��� 0���� �����մϴ�.");
            userData.CurrHP = 0;
        }
        else
        {
            userData.CurrHP = pCurrHP;
        }

        //
        UpdateUserData();

        //
        Util.LogSuccess($"������ ����� hpValue�� {userData.CurrHP}�Դϴ�.");
        return userData.CurrHP;
    }

    /// <summary>
    // ������ Exp ���� �˻��Ͽ� ���� Exp ���ȿ� �����մϴ�. ��ȯ�� ������ �ٽ� ������ּ���.
    /// </summary>
    public static int UpdateUserData_ExpValue(int pExpValue)
    {
        if (pExpValue <= userData.ExpValue)
        {
            Util.LogWarning($"�����Ϸ��� ExpValue({pExpValue}) ���� ������ ���� ExpValue({userData.ExpValue})�� �۰ų� �����Ƿ� ������ �� �����ϴ�.");
            return userData.ExpValue;
        }

        CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(userData.CharacterID);
        int maxExp = Define.User_MaxExp_init;
        if (pExpValue > maxExp)
        {
            Util.LogSuccess($"ExpValue({pExpValue}) �� �ʰ��� MaxExp({maxExp})���� �����մϴ�.\n������ ������ ������ �ּ���!");
            userData.ExpValue = maxExp;
        }
        else if (pExpValue < 0)
        {
            Util.LogWarning($"ExpValue({pExpValue}) ���� 0 �̸��� �� �����ϴ�.");
            userData.ExpValue = 0;
        }
        else
        {
            userData.ExpValue = pExpValue;
        }

        //
        UpdateUserData();

        //
        Util.LogSuccess($"������ ����� ExpValue�� {userData.ExpValue}�Դϴ�.");
        return userData.ExpValue;
    }

    [Obsolete("�ӽ�")]
    public static int UpdateUserData_LevelUp()
    {
        CharacterTable.Data characterData = GlobalScene.Instance.CharacterTable.GetTableData(userData.CharacterID);
        int maxExp = 500;
        if (userData.ExpValue < maxExp)
        {
            Util.LogWarning($"������ ExpValue({userData.ExpValue})�� MaxExp({maxExp})���� �۾� LevelUp�� �� �����ϴ�.");
            return userData.LevelValue;
        }

        userData.LevelValue += 1;
        userData.ExpValue   = 0;

        //
        UpdateUserData();

        //
        Util.LogSuccess($"������ ����� LevelValue {userData.LevelValue}�Դϴ�.");
        Util.LogSuccess($"������ ExpValue {userData.ExpValue}���� �ʱ�ȭ�Ǿ����ϴ�. ����� ������ ������ �ּ���.");
        return userData.LevelValue;
    }

    public static void UpdateUserData_WeaponID(int pWeaponID)
    {
        if (pWeaponID < 1) // Table ���� �˻�
        {
            Util.LogWarning();
            return;
        }

        userData.WeaponID = pWeaponID;

        ///
        UpdateUserData();
    }

    //[Obsolete("�ӽ�")]
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

    public static void LoadUserData() // Backend �����͸� �ҷ��ɴϴ�.
    {
        LitJson.JsonData gameDataJson = BackendManager.LoadBackendData(Define.User_TableName);
        if (gameDataJson == null)
        {
            Util.LogWarning($"���� �����Ͱ� �������� �ʽ��ϴ�. ���ο� ���� �����͸� �����մϴ�.");

            /// CreateUserData
            {
                // ServerData.UserData
                userData.CharacterID = Define.UserDataValue.CharacterID;
                userData.StatID      = Define.UserDataValue.StatID;
                userData.WeaponID    = Define.UserDataValue.WeaponID;
                userData.CurrHP      = Define.UserDataValue.CurrHP;
                userData.LevelValue  = Define.UserDataValue.LevelValue;
                userData.ExpValue    = Define.UserDataValue.ExpValue;
                userData.CoinValue   = Define.UserDataValue.CoinValue;
                userData.SpawnPos    = Define.UserDataValue.SpawnPos;
                userData.SpawnRot    = Define.UserDataValue.SpawnRot;

                /// SaveUserData
                Param param = AddServerData();
                BackendManager.SaveBackendData(Define.User_TableName, ref param);

                /// Reload
                gameDataJson = BackendManager.LoadBackendData(Define.User_TableName);
                if (gameDataJson == null)
                {
                    Util.LogError();
                    return;
                }
            }
        }

        ///
        userData.CharacterID = StringToInt(gameDataJson[0]["CharacterID"].ToString());
        userData.StatID      = StringToInt(gameDataJson[0]["StatID"].ToString());
        userData.WeaponID    = StringToInt(gameDataJson[0]["WeaponID"].ToString());
        userData.CurrHP      = StringToInt(gameDataJson[0]["CurrHP"].ToString());
        userData.LevelValue  = StringToInt(gameDataJson[0]["LevelValue"].ToString());
        userData.ExpValue    = StringToInt(gameDataJson[0]["ExpValue"].ToString());
        userData.CoinValue   = StringToInt(gameDataJson[0]["CoinValue"].ToString());
        userData.SpawnPos    = StringToVector3(gameDataJson[0]["SpawnPos"].ToString());
        userData.SpawnRot    = StringToVector3(gameDataJson[0]["SpawnRot"].ToString());

        //userData.info = gameDataJson[0]["info"].ToString();
        //foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
        //{
        //    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
        //}
        //foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
        //{
        //    userData.equipment.Add(equip.ToString());
        //}

        Util.LogSuccess($"���� ������ �ε带 �����߽��ϴ�. \n{ToString()}");
    }

    private static void UpdateUserData() // Backend �����Ϳ� �����մϴ�.
    {
        Param param = AddServerData();

        /// UpdateBackend
        BackendManager.UpdateBackendData(Define.User_TableName, ref param);
    }

    private static Param AddServerData()
    {
        Param param = new Param();

        param.Add("CharacterID",    userData.CharacterID.ToString());
        param.Add("StatID",         userData.StatID.ToString());
        param.Add("WeaponID",       userData.WeaponID.ToString());
        param.Add("CurrHP",         userData.CurrHP.ToString());
        param.Add("LevelValue",     userData.LevelValue.ToString());
        param.Add("ExpValue",       userData.ExpValue.ToString());
        param.Add("CoinValue",      userData.CoinValue.ToString());
        param.Add("SpawnPos",       userData.SpawnPos.ToString());
        param.Add("SpawnRot",       userData.SpawnRot.ToString());

        return param;
    }

    #endregion ServerLoad

    #region Parse

    public static int StringToInt(string pStr)
    {
        return int.Parse(pStr);
    }

    public static Vector3 StringToVector3(string pStr)
    {
        pStr = pStr.Replace("(", "");
        pStr = pStr.Replace(")", "");

        Vector3 vec      = Vector3.zero;
        float[] elements = pStr.Split(',').Select(x => float.Parse(x)).ToArray();
        if (elements != null && elements.Length == 3)
        {
            vec = new Vector3(elements[0], elements[1], elements[2]);
        }

        return vec;
    }

    #endregion Parse

    /// Debug.Log(ToString())
    public static string ToString()
    {
        StringBuilder result = new StringBuilder();

        /// User
        //result.AppendLine($"inData : {InData.ToString()}");
        //result.AppendLine($"nickName : {NickName.ToString()}");

        /// Player
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
        //    result.AppendLine($"| {itemKey} : {test_inventory[itemKey]}��");
        //}
        //result.AppendLine($"equipment");
        //foreach (var equip in test_equipment)
        //{
        //    result.AppendLine($"| {equip}");
        //}

        return result.ToString();
    }
}

