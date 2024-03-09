
using System;
using System.Collections;
using UnityEngine;

public class WorldScene : BaseScene
{
    /// <summary>
    /// Table
    /// </summary>
    Table.Spawning spawningTable = null;
    Table.Spawner spawnerTable = null;
    Table.Character characterTable = null;
    Table.Stat statTable = null;

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
        // LoadTable
        {
            spawningTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Spawning>();
            spawnerTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Spawner>();
            characterTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Character>();
            statTable = GlobalScene.TableMng.CreateOrGetBaseTable<Table.Stat>();
        }

        // SetUI
        {
            // Joystick
            worldUI = GlobalScene.UIMng.CreateOrGetBaseUI<JoystickUI>();
            worldUI.Close();
        }

        //// Player
        // Player
        //GlobalScene.UserMng.LoadUserData();
        //LoadSpawner(spawnerData.id, InitPlayerEvent, SpawnPlayerEvent, DespawnPlayerEvent);

        //GlobalScene.UserMng.LoadUserData();
        //GlobalScene.SpawnMng.SetPlayerSpawner();
        //GlobalScene.SpawnMng.SpawnCharacter(GlobalScene.UserMng.SpawnerID);
        //yield return new WaitUntil(() => GlobalScene.GameMng.IsGamePlay); // 플레이어가 저장되면 다음 구문으로 이동

        //// Controller
        //GlobalScene.GUIMng.SetWorldSceneController();
        //yield return null;

        //// Camera
        //GlobalScene.CameraMng.SetWorldSceneCamera();
        //yield return null;

        //// Monster
        //int tempSpawnerID = 1;
        //GlobalScene.SpawnMng.SetMonsterSpawner(tempSpawnerID);
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
