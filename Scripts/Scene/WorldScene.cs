using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;


public class WorldScene : BaseScene
{
    static WorldScene instance;
    public static WorldScene Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// MainUI
    /// </summary>
    private JoystickUI worldUI = null;
    //public BaseInput BaseInput { get; }

    /// <summary>
    /// Input
    /// </summary>
    //public BaseInput BaseInput { get; }

    #region World

    public bool IsGamePlay
    {
        get
        {
            bool isWorldScene = SceneManager.Instance.IsActiveScene<WorldScene>();
            bool isSpawnPlayer = temp_player != null;
            bool isLivePlayer = temp_player != null && temp_player.BaseAnimType != Define.BaseAnim.Die;

            //
            bool isGamePlay = isWorldScene && isSpawnPlayer && isLivePlayer;
            if (isGamePlay)
            {
                return true;
            }
            else
            {
                Debug.Log("Failed : 게임을 플레이할 수 없습니다.");
                return false;
            }

        }
    }


    Player temp_player = null;
    public IControllHndl_Temp PlayerCtrl
    {
        get
        {
            if (! IsGamePlay)
            {
                Debug.LogWarning("Failed : ");
                return null;
            }
            return temp_player;
        }
    }

    /// <summary>
    /// Spawner
    /// </summary>
    HashSet<ISpawner> worldSpawner_hashSet = new HashSet<ISpawner>();


    #endregion World

    protected override void OnAwake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        /// CreateTable
        {
            TableManager.Instance.CreateOrGetBaseTable<Table.SpawnerTable>();
            TableManager.Instance.CreateOrGetBaseTable<Table.CharacterTable>();
        }

        /// CreateUI
        {
            // Joystick
            worldUI = UIManager.Instance.CreateOrGetBaseUI<JoystickUI>();
            worldUI.Close();
        }

        // Spawner
        {
            // Player
            UserManager.Instance.LoadUserData();
            temp_player = CharacterSpawner.CreateCharacter(Define.WorldObject.Character, 1, DespawnPlayerEvent); //임시

            // Enemy
            Table.SpawnerTable.Data[] spawnerDatas = TableManager.Instance.CreateOrGetBaseTable<Table.SpawnerTable>().GetTableDatasAll();
            foreach (Table.SpawnerTable.Data spawnerData in spawnerDatas)
            {
                ISpawner worldSpawner = CharacterSpawner.CreateSpawner(spawnerData.id, SpawnEnemyEvent, DespawnEnemyEvent);
                //worldSpawner.SetWorldSpawner(spawnerData.id, SpawnEnemyEvent, DespawnEnemyEvent);
                
                ///
                worldSpawner_hashSet.Add(worldSpawner);
            }
        }

        // Controller
        //GlobalScene.GUIMng.SetWorldSceneController();

        /// Camera
        CameraManager.Instance.SetQuarterViewCamMode(temp_player.transform);
    }

    protected override void OnStart()
    {
        // Spawn
        {
            ///Player
            Vector3 spawnPos = new Vector3(7.5f, 0f, 1f); //임시
            Vector3 spawnRot = Quaternion.identity.eulerAngles; //임시
            temp_player.Spawn(spawnPos, spawnRot);

            /// Enemy
            PlaySpawnersPooling(true);
        }

        worldUI.Open();

        /// Camera
        CameraManager.Instance.PlayQuarterViewCam(true);

        //GlobalScene.GUIMng.StartJoystickController();
    }

    protected override void OnDestroy_()
    {
        if(instance != null && instance.gameObject != null)
        {
            ResourceManager.Instance.DestroyGameObject(instance.gameObject);
        }

        instance = null;
    }

    #region Spawner

    //public static ISpawner CreateWorldSpawner<TSpawner>() where TSpawner : Component, ISpawner
    //{
    //    return Util.CreateGameObject<TSpawner>();
    //}

    void SpawnEnemyEvent(GameObject pGO, Define.Actor pAactorType, int actorID)
    {
        //
    }

    void DespawnEnemyEvent(GameObject pGO)
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

    void DespawnPlayerEvent(GameObject pGO)
    {
        /// Respawn
        UserManager.Instance.UpdateUserData();
        SceneManager.Instance.LoadBaseScene<WorldScene>(); // WorldScene을 재로드 합니다.
    }

    public void PlaySpawnersPooling(bool pSwitch)
    {
        foreach(ISpawner spawner in worldSpawner_hashSet)
        {
            if(spawner != null)
            {
                if(pSwitch)
                    spawner.Play();
                else
                    spawner.Stop();
            }
        }
    }

    #endregion Spawner

    public void SetPlayerDead()
    {
        worldUI.Close();
        CameraManager.Instance.PlayQuarterViewCam(false);
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
}
