using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public interface IBaseScene
    {
    }

    public interface IBaseManager
    {
        public void OnRelease();
    }

    public interface IBaseUI
    {
        public void Open();
        public void Close();
        public void DestroySelf();
    }

    public interface ITableLoader
    {
        public void Initialized(TextAsset pTextAsset);
    }

    public interface ISpawner
    {
        public bool SwitchPooling { set; }
        //public void Play();
        //public void Stop();
        //public void SetWorldSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction);
    }
}
