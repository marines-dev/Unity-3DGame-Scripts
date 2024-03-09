using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public interface IBaseTable
    {
    }

    public interface IBaseUI
    {
        public void Open();
        public void Close();
        public void DestroySelf();
    }

    public interface ISpawner
    {
        public void SetWorldSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction);
    }
}
