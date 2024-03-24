using System;
using UnityEngine;


public class Define
{
    #region Title

    public enum TitleProcess
    {
        InitData,
        //Patch,
        LogIn,
        LoadUserData,
        LoadGameScene,
        Complete,
    }

    public enum LogInProcess
    {
        None,
        //InitLogInState,
        //UpdateNickname,
        Auth,
        LogIn,
        LogOut,
    }

    public enum Account
    {
        None,
        Guest,
        Google
    }

    #endregion Title

    public enum Spawning
    {
        None,
        Player,
        Enemy,
        Item,
    }

    public enum WorldObject
    {
        None,
        Character,
        Item,
    }

    [Obsolete("임시")]
    public enum Character
    {
        None,
        Human,
        Monster,
    }

    public enum Actor
    {
        None,
        Player,
        Enemy,
        //NPC,
    }

    public enum ExistenceState
    {
        Despawn,
        Spawn,
    }

    public enum SurvivalState
    {
        Dead,
        Alive,
    }

    public enum BaseAnim
    {
        None,
        Die,
        Idle,
        Run,
        //Attack,
    }

    public enum UpperAnim
    {
        None,
        Ready,
        Attack,
    }

    public enum WeaponType
    {
        None,
        Hand,
        Sword,
        Gun,
    }

    public enum Layer
    {
        Monster = 8,
        Ground = 9,
        Block = 10,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    //public enum CameraMode
    //{
    //    None,
    //    Defualt,
    //    QuarterView,
    //}

    //public enum Prefab
    //{
    //    None,
    //    Character,
    //    Weapon,
    //}

    #region User

    public static readonly string User_TableName = "UserData";
    public static readonly int User_MaxExp_init  = 500;
    public static ServerData.UserData UserDataValue
    {
        get
        {
            ServerData.UserData newUserData = new ServerData.UserData();
            newUserData.CharacterID = 1;
            newUserData.StatID      = 1;
            newUserData.WeaponID    = 2;

            StatTable.Data statData = GlobalScene.Instance.StatTable.GetTableData(newUserData.StatID); //임시 
            newUserData.CurrHP            = statData.MaxHp;
            newUserData.LevelValue        = 1;
            newUserData.ExpValue          = 0;
            newUserData.CoinValue         = 1000;
            newUserData.SpawnPos          = new Vector3(7.5f, 0f, 1f);
            newUserData.SpawnRot          = Quaternion.identity.eulerAngles;

            return newUserData;
        }
    }

    #endregion User 

    #region Camera

    public static readonly Vector3 cam_initPos        = new Vector3(0f, 1f, -10f);
    public static readonly Vector3 cam_initRot        = new Vector3(0f, 0f, 0f);
    public static readonly Vector3 cam_initScale      = new Vector3(1f, 1f, 1f);
    public static readonly Color cam_backgroundColor  = Color.black;
    public static readonly Vector3 followCam_deltaPos = new Vector3(0.0f, 8.0f, -5.0f);

    #endregion Camera
}
