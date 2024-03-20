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

    public interface ITable
    {

    }

    public interface ITableLoader
    {
        public void Initialized(TextAsset pTextAsset);
    }

    public interface IPlayerCtrl
    {
        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public void OnMove(Vector3 pEulerAngles);
        public void OnStop();
        public void OnAttack();
        public void OnReady();
        public void OnIncreaseExp(int pAddExpValue);
    }

    //public interface ISpawner
    //{
    //    public bool SwitchPooling { set; }
    //    //public void Play();
    //    //public void Stop();
    //    //public void SetWorldSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction);
    //}
}
