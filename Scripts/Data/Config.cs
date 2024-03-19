using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
    #region User

    public static readonly string User_TableName = "UserData";
    public static readonly int User_MaxExp_init = 500;
    public static ServerData.UserData UserDataValue
    {
        get
        {
            ServerData.UserData newUserData = new ServerData.UserData();
            newUserData.CharacterID = 1;
            newUserData.StatID = 1;
            newUserData.WeaponID = 2;

            Table.StatTable.Data statData = GlobalScene.Instance.StatTable.GetTableData(newUserData.StatID); //юс╫ц 
            newUserData.CurrHP = statData.MaxHp;
            newUserData.LevelValue = 1;
            newUserData.ExpValue = 0;
            newUserData.CoinValue = 1000;
            newUserData.SpawnPos = new Vector3(7.5f, 0f, 1f);
            newUserData.SpawnRot = Quaternion.identity.eulerAngles;

            return newUserData;
        }
    }

    //public static readonly string UserDataKey_CharacterID = "CharacterID";
    //public static readonly string UserDataKey_StatID = "StatID";
    //public static readonly string UserDataKey_WeaponID = "WeaponID";
    //public static readonly string UserDataKey_CurrHP = "CurrHP";
    //public static readonly string UserDataKey_LevelValue = "LevelValue";
    //public static readonly string UserDataKey_ExpValue = "ExpValue";
    //public static readonly string UserDataKey_CoinValue = "CoinValue";
    //public static readonly string UserDataKey_SpawnPos = "SpawnPos";
    //public static readonly string UserDataKey_SpawnRot = "SpawnRot";

    #endregion User 

    //#region GamePlayer

    //public static readonly int gamePlayer_spawnerID = 1;
    //public static readonly Define.Prefab gamePlayer_prefabType = Define.Prefab.Character;
    //public static readonly int gamePlayer_characterID   = 1;
    //public static readonly int gamePlayer_levelUpCount  = 1;
    //public static readonly int gamePlayer_expValue      = 0;
    //public static readonly int gamePlayer_coinValue     = 1000;
    //public static readonly int gamePlayer_hpValue       = 300;
    //public static readonly int gamePlayer_weaponID      = 0;

    //#endregion GamePlayer

    #region Camera

    public static readonly Vector3 cam_initPos = new Vector3(0f, 1f, -10f);
    public static readonly Vector3 cam_initRot = new Vector3(0f, 0f, 0f);
    public static readonly Vector3 cam_initScale = new Vector3(1f, 1f, 1f);
    public static readonly Color cam_backgroundColor = Color.black;
    public static readonly Vector3 followCam_deltaPos = new Vector3(0.0f, 8.0f, -5.0f);

    #endregion Camera

    #region UI/GUI

    //public static readonly string ui_uiStorageName = "@UIStorage";
    //public static readonly string gui_controller = "@Controller";

    #endregion UI/GUI
}