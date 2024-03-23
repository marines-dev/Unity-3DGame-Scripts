using System;
using Interface;
using ServerData;
using UnityEngine;

[Obsolete("юс╫ц")]
public class GlobalScene : BaseScene<GlobalScene, GlobalUI>
{
    /// <summary>
    /// Table
    /// </summary>
    public SpawnerTable     SpawnerTable    { get { return CreateOrGetBaseTable<SpawnerTable>(); } }
    public EnemyTable       EnemyTable      { get { return CreateOrGetBaseTable<EnemyTable>(); } }
    public CharacterTable   CharacterTable  { get { return CreateOrGetBaseTable<CharacterTable>(); } }
    public StatTable        StatTable       { get { return CreateOrGetBaseTable<StatTable>(); } }
    public WeaponTable      WeaponTable     { get { return CreateOrGetBaseTable<WeaponTable>(); } }

    /// <summary>
    /// UserData
    /// </summary>
    public UserData UserData { get { return Manager.UserMng.UserData; } }

    protected override void OnAwake()
    {
        RegisteredGlobalObjects();
    }
    protected override void OnStart() { }
    protected override void onDestroy() { }

    #region Resource

    //public TResource LoadResource<TResource>(string path) where TResource : UnityEngine.Object
    //{
    //    return ResourceLoader.Load<TResource>(path);
    //}

    //public GameObject InstantiateResource(string pPath, Transform pParent = null)
    //{
    //    return ResourceLoader.Instantiate(pPath, pParent);
    //}

    //public void DestroyGameObject(GameObject pGameObject)
    //{
    //    ResourceLoader.DestroyGameObject(pGameObject);
    //}

    #endregion Resource

    //#region Scene

    //public void LoadScene<TScene>() where TScene : IBaseScene
    //{
    //    Manager.SceneMng.LoadBaseScene<TScene>();
    //}

    //#endregion Scene

    #region UserData

    public void UpdateUserData_CurrHPValue(int pHP)
    {
        Manager.UserMng.UpdateUserData_CurrHPValue(pHP);
    }

    public void UpdateUserData_ExpValue(int pExpValue)
    {
        Manager.UserMng.UpdateUserData_ExpValue(pExpValue);
    }

    public void UpdateUserData_LevelUp()
    {
        Manager.UserMng.UpdateUserData_LevelUp();
    }

    #endregion UserData
}
