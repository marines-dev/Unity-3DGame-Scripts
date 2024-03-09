
using System;
using System.Collections;
using UnityEngine;

public class WorldScene : BaseScene
{
    /// <summary>
    /// Table
    /// </summary>
    private Table.Spawning spawningTable = null;
    private Table.Spawner spawnerTable = null;
    private Table.Character characterTable = null;
    private Table.Stat statTable = null;

    /// <summary>
    /// MainUI
    /// </summary>
    private JoystickUI worldUI = null;
    //public BaseInput BaseInput { get; }

    /// <summary>
    /// Input
    /// </summary>
        //public BaseInput BaseInput { get; }

    protected override void OnAwake()
    {
        /// CreateTable
        {
            spawningTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Spawning>();
            spawnerTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Spawner>();
            characterTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Character>();
            statTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Stat>();
        }

        /// SetUI
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
            Table.Spawner.Data[] spawnerDatas = spawnerTable.GetTableDatasAll();
            //foreach (Table.Spawner.Data spawnerData in spawnerDatas)
            //{
            //    LoadSpawner(spawnerData.id, InitEnemyEvent, SpawnEnemyEvent, DespawnEnemyEvent);
            //}
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
        GlobalScene.GUIMng.StartJoystickController();
    }

    protected override void OnDestroy_()
    {
    }

    //public void LoadSpawner(int pSpawnerID, Action<GameObject> pInitEvent, Action<GameObject, int> pSpawnEvent, Action<GameObject> pDespawnEvent) where T : BaseWorldObj
    //{
    //    Spawner worldSpawner = Global.LoadHandler_GameObject<Spawner>(transform);
    //    worldSpawner.SetSpawner(pSpawnerID, pInitEvent, pSpawnEvent, pDespawnEvent);

    //    spawner_dic.Add(pSpawnerID, worldSpawner);
    //}
}
