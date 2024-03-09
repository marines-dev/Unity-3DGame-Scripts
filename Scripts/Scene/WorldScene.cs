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
    /// <summary>
    /// Spawner
    /// </summary>
    HashSet<ISpawner> worldSpawner_hashSet = new HashSet<ISpawner>();


    #endregion World
    protected override void OnAwake()
    {
        /// CreateTable
        {
            GlobalScene.TableMng.CreateOrGetBaseTable<Table.SpawnerTable>();
            GlobalScene.TableMng.CreateOrGetBaseTable<Table.CharacterTable>();
        }

        /// CreateUI
        {
            // Joystick
            worldUI = GlobalScene.UIMng.CreateOrGetBaseUI<JoystickUI>();
            worldUI.Close();
        }

        // Spawner
        {
            /// Player
            //GlobalScene.UserMng.LoadUserData();
            //LoadSpawner(spawnerData.id, InitPlayerEvent, SpawnPlayerEvent, DespawnPlayerEvent);

            // Enemy
            Table.SpawnerTable.Data[] spawnerDatas = GlobalScene.TableMng.CreateOrGetBaseTable<Table.SpawnerTable>().GetTableDatasAll();
            foreach (Table.SpawnerTable.Data spawnerData in spawnerDatas)
            {
                ISpawner worldSpawner = CreateWorldSpawner<CharacterSpawner>(spawnerData.id, SpawnEnemyEvent, DespawnEnemyEvent);
                
                ///
                worldSpawner_hashSet.Add(worldSpawner);
            }
        }

        //// Controller
        //GlobalScene.GUIMng.SetWorldSceneController();
        //yield return null;

        //// Camera
        //GlobalScene.CameraMng.SetWorldSceneCamera();
        //yield return null;
    }

    protected override void OnStart()
    {
        //GlobalScene.GUIMng.StartJoystickController();
    }

    protected override void OnDestroy_()
    {
    }

    public static ISpawner CreateWorldSpawner<TSpawner>(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction) where TSpawner : Component, ISpawner
    {
        ISpawner worldSpawner = Util.CreateGameObject<TSpawner>();
        worldSpawner.SetWorldSpawner(pSpawnerID, pSpawnAction, pDespawnAction);

        return worldSpawner;
    }

    void SpawnEnemyEvent(GameObject pGO, Define.Actor pAactorType, int actorID)
    {
        //
    }

    void DespawnEnemyEvent(GameObject pGO)
    {
        //SetDespawnWorldObj(pGO);
    }

    //public void LoadSpawner(int pSpawnerID, Action<GameObject> pInitEvent, Action<GameObject, int> pSpawnEvent, Action<GameObject> pDespawnEvent) where T : BaseWorldObj
    //{
    //    Spawner worldSpawner = Global.LoadHandler_GameObject<Spawner>(transform);
    //    worldSpawner.SetSpawner(pSpawnerID, pInitEvent, pSpawnEvent, pDespawnEvent);

    //    spawner_dic.Add(pSpawnerID, worldSpawner);
    //}

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

    //void DespawnPlayerEvent(GameObject pGO)
    //{
    //    SetDespawnWorldObj(pGO);

    //    // Respawn
    //    RespawnPlayer();
    //}
}
