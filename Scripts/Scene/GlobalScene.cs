using System;
using ServerData;
using Table;
using UnityEngine;

[Obsolete("юс╫ц")]
public class GlobalScene : BaseScene<GlobalScene>
{
    /// <summary>
    /// Table
    /// </summary>
    public SpawnerTable SpawnerTable { get { return Manager.TableMng.CreateOrGetBaseTable<SpawnerTable>(); } }
    public EnemyTable EnemyTable { get { return Manager.TableMng.CreateOrGetBaseTable<EnemyTable>(); } }
    public CharacterTable CharacterTable { get { return Manager.TableMng.CreateOrGetBaseTable<CharacterTable>(); } }
    public StatTable StatTable { get { return Manager.TableMng.CreateOrGetBaseTable<StatTable>(); } }
    public WeaponTable WeaponTable { get { return Manager.TableMng.CreateOrGetBaseTable<WeaponTable>(); } }

    /// <summary>
    /// UserData
    /// </summary>
    public UserData UserData { get { return Manager.UserMng.UserData; } }

    protected override void OnAwake()
    {
        RegisteredGlobalObjects();
    }
    protected override void OnStart() { }
    protected override void OnDestroy_() { }

    #region Resource

    public TResource LoadResource<TResource>(string path) where TResource : UnityEngine.Object
    {
        return Manager.ResourceMng.Load<TResource>(path);
    }

    public GameObject InstantiateResource(string pPath, Transform pParent = null)
    {
        return Manager.ResourceMng.Instantiate(pPath, pParent);
    }

    public void DestroyGameObject(GameObject pGameObject)
    {
        Manager.ResourceMng.DestroyGameObject(pGameObject);
    }

    #endregion Resource

    //#region UserData

    //public int GetUserExpValue()
    //{
    //    return Manager.UserMng.userData.ExpValue;
    //}

    //public int GetUserLevelValue()
    //{
    //    return Manager.UserMng.userData.LevelValue;
    //}

    //public int GetUserHpValue()
    //{
    //    return Manager.UserMng.userData.CurrHP;
    //}

    public void UpdateUserData_CurrHPValue(int pHP)
    {
        Manager.UserMng.UpdateUserData_CurrHPValue(pHP);
    }

    //#endregion UserData
}
