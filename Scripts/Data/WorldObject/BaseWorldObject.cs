using System;
using UnityEngine;


public abstract class BaseWorldObject : MonoBehaviour
{
    Define.WorldObject worldObjType = Define.WorldObject.None;
    int worldObjID = 0;

    //Define.SpawnState spawnState_ = Define.SpawnState.Despawn;
    //public Define.SpawnState SpawnState
    //{
    //    get
    //    {
    //        return spawnState_;
    //    }
    //    set
    //    {
    //        switch (spawnState_)
    //        {
    //            case Define.SpawnState.Despawn:
    //                {
    //                    gameObject.SetActive(false);
    //                }
    //                break;
    //            case Define.SpawnState.Spawn:
    //                {
    //                    gameObject.SetActive(true);
    //                }
    //                break;
    //        }
    //        spawnState_ = value;
    //    }
    //}
    //public bool IsActive { get { return gameObject.activeInHierarchy; } }

    public Vector3 LocalPosition 
    { 
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    public Vector3 LocalRotation 
    { 
        get { return transform.localEulerAngles; } 
        set { transform.localEulerAngles = value;}
    }
    public Vector3 Position 
    { 
        get { return transform.position; } 
        set { transform.position = value; }
    }
    public Vector3 Rotation 
    { 
        get { return transform.rotation.eulerAngles; } 
        set { transform.rotation = Quaternion.Euler(value); }
    }
    //public GameObject GameObject { get { return gameObject; } } // 임시

    Action<GameObject> despawnAction = null;


    /// <summary>
    /// 생성된 직후 초기화를 위해 한 번 호출 됩니다.
    /// </summary>
    public virtual void SetWorldObject(Define.WorldObject pWorldObjType, int pWorldObjID, Action<GameObject> pDespawnAction)
    {
        if (pWorldObjID <= 0)
        {
            Debug.LogWarning("Failed : ");
            Despawn();
        }

        worldObjType = pWorldObjType;
        worldObjID = pWorldObjID;
        despawnAction = pDespawnAction;

        //spawnState_ = Define.SpawnState.Despawn;
    }

    public virtual void Spawn(Vector3 pPos, Vector3 pRot)
    {
        // Pos
        Position = pPos;
        Rotation = pRot;

        //spawnState_ = Define.SpawnState.Despawn;
    }

    protected virtual void Despawn()
    {
        if (despawnAction != null)
            despawnAction.Invoke(gameObject);

        //spawnState_ = Define.SpawnState.Despawn;
    }
}
