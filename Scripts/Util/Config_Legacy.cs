//using UnityEngine;

//public static class Config_Legacy
//{
//    #region User

//    public static readonly string User_TableName = "UserData";
//    public static readonly int User_MaxExp_init = 500;
//    public static ServerData.UserData UserDataValue
//    {
//        get
//        {
//            ServerData.UserData newUserData = new ServerData.UserData();
//            newUserData.CharacterID = 1;
//            newUserData.StatID = 1;
//            newUserData.WeaponID = 2;

//            Table.StatTable.Data statData = GlobalScene.Instance.StatTable.GetTableData(newUserData.StatID); //юс╫ц 
//            newUserData.CurrHP = statData.MaxHp;
//            newUserData.LevelValue = 1;
//            newUserData.ExpValue = 0;
//            newUserData.CoinValue = 1000;
//            newUserData.SpawnPos = new Vector3(7.5f, 0f, 1f);
//            newUserData.SpawnRot = Quaternion.identity.eulerAngles;

//            return newUserData;
//        }
//    }

//    #endregion User 

//    #region Camera

//    public static readonly Vector3 cam_initPos = new Vector3(0f, 1f, -10f);
//    public static readonly Vector3 cam_initRot = new Vector3(0f, 0f, 0f);
//    public static readonly Vector3 cam_initScale = new Vector3(1f, 1f, 1f);
//    public static readonly Color cam_backgroundColor = Color.black;
//    public static readonly Vector3 followCam_deltaPos = new Vector3(0.0f, 8.0f, -5.0f);

//    #endregion Camera
//}