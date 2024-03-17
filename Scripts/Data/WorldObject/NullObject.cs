using UnityEngine;

public class NullObject : IPlayerCtrl, IWeapon
{
    #region IPlayerCtrl

    public Vector3 Position { get { return Vector3.zero; } }
    public Vector3 Rotation { get { return Vector3.zero; } }

    public void OnMove(Vector3 pEulerAngles) { }
    public void OnStop() { }
    public void OnAttack() { }
    public void OnReady() { }

    #endregion IPlayerCtrl

    #region IWeapon

    public int AttackValue { get { return 0; } }

    public void SetReadyPos() { }
    public void SetAttackPos() { }
    public void SetEquiped(bool pEquiped) { }
    public void PlayHit() { }

    #endregion IWeapon
}