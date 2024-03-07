using System.Collections;
using UnityEngine;

public class LoadScene : BaseScene
{
    protected override void OnAwake()
    {
        // Load UI
        {
        }
    }

    protected override void OnStart() 
    {
        // ResetManager
        {
            // Base
            //GlobalScene.SceneMng.Initialize();
            GlobalScene.GameMng.Initialize();
            GlobalScene.CameraMng.Initialize();
            GlobalScene.ResourceMng.Initialize();
            GlobalScene.TableMng.Initialize();
            GlobalScene.UIMng.Initialize();
            GlobalScene.GUIMng.Initialize();

            // Server
            GlobalScene.BackendMng.Initialize();
            GlobalScene.GPGSMng.Initialize();
            GlobalScene.LogInMng.Initialize();

            //
            GlobalScene.UserMng.Initialize();
            //Managers.Input.Init();
            GlobalScene.SpawnMng.Initialize();
        }
    }

    protected override void OnDestroy_() { }
}
