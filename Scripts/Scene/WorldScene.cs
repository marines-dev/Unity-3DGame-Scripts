
using System;
using System.Collections;
using UnityEngine;

public class WorldScene : BaseScene
{
    protected override void OnAwake()
    {
        //// LoadTable
        //{
        //    GlobalScene.TableMng.LoadTable<Table.Spawning>();
        //    GlobalScene.TableMng.LoadTable<Table.Spawner>();
        //    GlobalScene.TableMng.LoadTable<Table.Character>();
        //    GlobalScene.TableMng.LoadTable<Table.Stat>();
        //}

        //// SetUI
        //{
        //    // Joystick
        //    GlobalScene.UIMng.LoadUI<JoystickUI>();
        //    JoystickUI joystickUI = GlobalScene.UIMng.GetBaseUI<JoystickUI>();
        //    joystickUI.CloseUI();
        //}

        //// Cursor
        ////gameObject.GetOrAddComponent<CursorController>();

        //// Player

        ////Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
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

    //IEnumerator SpawnPlayerProcessRoutine()
    //{
    //    //Dictionary<int, Data.StatData> dict = Managers.Data.StatDict;
    //    Managers.Spawn.SetPlayerSpawner();
    //    Managers.Spawn.SpawnCharacter(Managers.User.SpawnerID);
    //    yield return null;
    //}
}
