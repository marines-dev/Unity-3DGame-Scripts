using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;


public class WorldScene : BaseScene
{
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
    
    Character temp_player = null;

    /// <summary>
    /// Spawner
    /// </summary>
    HashSet<ISpawner> worldSpawner_hashSet = new HashSet<ISpawner>();


    #endregion World
    protected override void OnAwake()
    {
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

        //// Controller
        //GlobalScene.GUIMng.SetWorldSceneController();
        //yield return null;

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
            SwitchSpawnersPooling(true);
        }

        /// Camera
        CameraManager.Instance.SwitchQuarterViewCamPlay(true);

        //GlobalScene.GUIMng.StartJoystickController();
    }

    protected override void OnDestroy_()
    {
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
        //SetDespawnWorldObj(pGO);

        //// Respawn
        //RespawnPlayer();
    }

    public void SwitchSpawnersPooling(bool pSwitch)
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

}
