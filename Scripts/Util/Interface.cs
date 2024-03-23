using UnityEngine;
using static Define;

namespace Interface
{
    public interface IBaseScene { }

    public interface IBaseManager
    {
        public void OnRelease();
    }

    public interface IBaseUI
    {
        public void Open();
        public void Close();
        //public void DestroySelf();
    }

    public interface IMainUI : IBaseUI { }

    public interface IBaseTable 
    {
        public void Initialized(TextAsset pTextAsset);
    }

    public interface ITarget
    {
        public ExistenceState ExistenceStateType { get; }
        public SurvivalState SurvivalStateType { get; }

        public Vector3 Position { get; }
        public Vector3 Rotation { get; }

        public void OnEnableTargetOutline();
        public void OnDisableTargetOutline();

        public void OnDamage(int pValue);
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

    //public interface ILoader<TKey, TValue>
    //{
    //    Dictionary<TKey, TValue> MakeDict();
    //}

    //public interface ISpawner
    //{
    //    public bool SwitchPooling { set; }
    //    //public void Play();
    //    //public void Stop();
    //    //public void SetWorldSpawner(int pSpawnerID, Action<GameObject, Define.Actor, int> pSpawnAction, Action<GameObject> pDespawnAction);
    //}
}
