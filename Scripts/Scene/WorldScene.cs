using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;


public class WorldScene : BaseScene<WorldScene>
{
    /// <summary>
    /// Table
    /// </summary>
    Table.SpawnerTable spawnerTable = null;
    public Table.SpawnerTable SpawnerTable => spawnerTable ?? (spawnerTable = Manager.TableMng.CreateOrGetBaseTable<Table.SpawnerTable>());

    Table.CharacterTable characterTable = null;
    public Table.CharacterTable CharacterTable => characterTable ?? (characterTable = Manager.TableMng.CreateOrGetBaseTable<Table.CharacterTable>());

    Table.StatTable statTable = null;
    public Table.StatTable StatTable => statTable ?? (statTable = Manager.TableMng.CreateOrGetBaseTable<Table.StatTable>());

    /// <summary>
    /// MainUI
    /// </summary>
    private WorldUI worldUI = null;

    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    #region World

    private Player player = null;
    private NullPlayer nullPlayer = new NullPlayer();
    public IPlayerCtrl PlayerCtrl
    {
        get
        {
            if (player == null || player.ExistenceStateType == Define.ExistenceState.Despawn || player.SurvivalStateType == SurvivalState.Dead)
            {
                Util.LogWarning($"플레이어를 사용할 수 없으므로 {nullPlayer.GetType()}를 반환합니다.");
                return nullPlayer;
            }

            return player;
        }
    }

    /// <summary>
    /// Spawner
    /// </summary>
    HashSet<ISpawner> worldSpawner_hashSet = new HashSet<ISpawner>();


    #endregion World

    protected override void OnAwake()
    {
        /// CreateTable
        {
            spawnerTable = Manager.TableMng.CreateOrGetBaseTable<Table.SpawnerTable>();
            //characterTable = Manager.TableMng.CreateOrGetBaseTable<Table.CharacterTable>();
            statTable = Manager.TableMng.CreateOrGetBaseTable<Table.StatTable>();
        }

        /// CreateUI
        {
            // Joystick
            worldUI = Manager.UIMng.CreateOrGetBaseUI<WorldUI>(MainCanvas);
            worldUI.Close();
        }

        // Spawner
        {
            // Player
            Manager.UserMng.LoadUserData();
            player = CharacterSpawner.CreateCharacter(Define.WorldObject.Character, 1, OnDespawnPlayerEvent); //임시
            player.SetPlayerDeadEvent(OnPlayerDeadEvent);

            // Enemy
            Table.SpawnerTable.Data[] spawnerDatas = Manager.TableMng.CreateOrGetBaseTable<Table.SpawnerTable>().GetTableDatasAll();
            foreach (Table.SpawnerTable.Data spawnerData in spawnerDatas)
            {
                ISpawner worldSpawner = CharacterSpawner.CreateSpawner(spawnerData.id, OnSpawnEnemyEvent, OnDespawnEnemyEvent);
                //worldSpawner.SetWorldSpawner(spawnerData.id, SpawnEnemyEvent, DespawnEnemyEvent);
                
                ///
                worldSpawner_hashSet.Add(worldSpawner);
            }
        }
    }

    protected override void OnStart()
    {
        // Spawn
        {
            ///Player
            Vector3 spawnPos = new Vector3(7.5f, 0f, 1f); //임시
            Vector3 spawnRot = Quaternion.identity.eulerAngles; //임시
            player.Spawn(spawnPos, spawnRot);

            /// Enemy
            PlaySpawnersPooling(true);
        }

        worldUI.Open();
        CamCtrl.SwitchQuarterViewMoed = true;
    }

    protected override void OnDestroy_()
    {
        //if(instance != null && instance.gameObject != null)
        //{
        //    ResourceManager.Instance.DestroyGameObject(instance.gameObject);
        //}

        //instance = null;
    }

    #region Spawner

    //public static ISpawner CreateWorldSpawner<TSpawner>() where TSpawner : Component, ISpawner
    //{
    //    return Util.CreateGameObject<TSpawner>();
    //}

    void OnSpawnEnemyEvent(GameObject pGO, Define.Actor pAactorType, int actorID)
    {
        //
    }

    void OnDespawnEnemyEvent(GameObject pGO)
    {
        //SetDespawnWorldObj(pGO);
    }

    //void InitPlayerEvent(GameObject pGO)
    //{
    //    Player player = InitWorldObj<Player>(pGO);
    //    SetSpawnPlayer(player);
    //}

    //void InitEnemyEvent(GameObject pGO)
    //{
    //    Enemy enemy = InitWorldObj<Enemy>(pGO);
    //}

    //public T InitWorldObj<T>(GameObject pGO) where T : BaseWorldObj
    //{
    //    if (pGO == null)
    //    {
    //        Debug.LogError($"Failed : ");
    //        return null;
    //    }

    //    T worldObj = pGO.GetOrAddComponent<T>();
    //    worldObj.Initialize(pPrefabType, pPrefabID, OnDespawnEvent);

    //    return worldObj;
    //}

    //void SpawnPlayerEvent(ICharacter pCharacter, int pSpawnerID)
    //{
    //    // ICharacter에서 Actor을 가져온다.
    //    Player player = SetSpawnWorldObj<Player>(pGO, pSpawnerID);

    //    SetSpawnPlayer();
    //}

    void OnDespawnPlayerEvent(GameObject pGO)
    {
        /// Respawn
        Manager.UserMng.UpdateUserData();
        Manager.SceneMng.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
    }

    private void PlaySpawnersPooling(bool pSwitch)
    {
        foreach(ISpawner spawner in worldSpawner_hashSet)
        {
            if(spawner != null)
            {
                spawner.SwitchPooling = pSwitch;
            }
        }
    }

    #endregion Spawner

    private void OnPlayerDeadEvent()
    {
        worldUI.Close();
        CamCtrl.SwitchQuarterViewMoed = false;
        PlaySpawnersPooling(false);
    }

    public ITargetHandler_Temp GetTargetCharacter(GameObject pTarget)
    {
        if (pTarget == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        Character baseCharacter = pTarget.GetComponent<Character>();
        if (baseCharacter == null)
        {
            Debug.LogWarning("Falied : ");
            return null;
        }

        return baseCharacter;
    }

    public int GetUserExpValue()
    {
        return Manager.UserMng.UserData.expValue;
    }

    public int GetUserLevelValue()
    {
        return Manager.UserMng.UserData.levelValue;
    }

    public int GetUserHpValue()
    {
        return Manager.UserMng.UserData.hpValue;
    }

    public int SetGetUserHP(int pHP)
    {
        return Manager.UserMng.SetUserHP(pHP);
    }

    [Obsolete("임시")]
    public TSpaceUI CreateBaseSpaceUI<TSpaceUI>(Transform pParent = null) where TSpaceUI : Component, IBaseUI
    {
        return Manager.UIMng.CreateBaseSpaceUI<TSpaceUI>(pParent);
    }
}
