using System;
using Table;
using UnityEngine;

public interface IWeapon
{
    public int AttackValue { get; }

    public void SetReadyPos();
    public void SetAttackPos();
    public void SetEquiped(bool pEquiped);
    public void PlayHit();
}

public class Weapon : MonoBehaviour, IWeapon
{
    public int AttackValue
    {
        get
        {
            WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(weaponID);
            if (weaponData == null)
            {
                Util.LogWarning();
                return 0;
            }

            return weaponData.AttackValue;
        }
    }

    private int weaponID = 0;

    private ParticleSystem shotSFX = null;


    public static IWeapon CreateWeapon(Transform pOwner, int pWeaponID)
    {
        NullObject       nullWeapon = new NullObject();
        WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(pWeaponID);
        if (pOwner == null || weaponData == null)
        {
            Util.LogWarning();
            return nullWeapon;
        }

        if (weaponData.WeaponType == Define.WeaponType.Hand)
        {
            Util.LogSuccess();
            return nullWeapon;
        }

        string     path = $"Prefabs/Weapon/{weaponData.PrefabName}";
        GameObject go   = GlobalScene.Instance.InstantiateResource(path);
        if (go == null)
        {
            Util.LogWarning();
            return nullWeapon;
        }

        Weapon weapon = go.GetOrAddComponent<Weapon>();
        weapon.SetWeapon(pOwner, pWeaponID);

        return weapon;
    }

    //void Start()
    //{
    //    BoxCollider collider = gameObject.GetOrAddComponent<BoxCollider>();
    //    collider.isTrigger = true;
    //}

    private void SetWeapon(Transform pOwner, int pWeaponID)
    {
        /// SFX
        {
            WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(pWeaponID);
            if (pOwner == null || weaponData == null)
            {
                Util.LogWarning();
                SetEquiped(false);
                return;
            }

            weaponID = pWeaponID;

            ///
            Transform[] children = pOwner.GetComponentsInChildren<Transform>();
            Transform weponTrans = Array.Find(children, x => x.name == weaponData.WeaponParentName);
            if (weponTrans == null)
            {
                Util.LogWarning();
                SetEquiped(false);
                return;
            }

            transform.parent = weponTrans;
            SetEquiped(true);

            /// CreateShotSFX
            {
                if (shotSFX == null)
                {
                    string path = $"Prefabs/SFX/{weaponData.SFXPrefabName}";
                    GameObject go = GlobalScene.Instance.InstantiateResource(path, transform);
                    if (go == null)
                    {
                        Util.LogWarning();
                        return;
                    }

                    ///
                    shotSFX = go.GetOrAddComponent<ParticleSystem>();
                    shotSFX.Stop();
                    //shotSFX.gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetReadyPos()
    {
        WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(weaponID);
        if (weaponData == null)
        {
            Util.LogWarning();
            SetEquiped(false);
            return;
        }

        transform.localPosition = weaponData.ReadyPosition;
        transform.localRotation = Quaternion.Euler(weaponData.ReadyRotation);
        SetEquiped(true);
    }

    public void SetAttackPos()
    {
        WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(weaponID);
        if (weaponData == null)
        {
            Util.LogWarning();
            SetEquiped(false);
            return;
        }

        transform.localPosition = weaponData.AttackPosition;
        transform.localRotation = Quaternion.Euler(weaponData.AttackRotation);
        SetEquiped(true);
    }

    public void SetEquiped(bool pEquiped)
    {
        gameObject.SetActive(pEquiped);
    }

    public void PlayHit()
    {
        WeaponTable.Data weaponData = GlobalScene.Instance.WeaponTable.GetTableData(weaponID);
        if (weaponData == null)
        {
            Util.LogWarning();
            SetEquiped(false);
            return;
        }

        SetEquiped(true);

        /// SFX
        {
            if (shotSFX == null)
                return;

            shotSFX.transform.parent = transform;
            shotSFX.transform.localPosition = weaponData.SFXPosition;
            shotSFX.transform.localRotation = Quaternion.Euler(weaponData.SFXRotation);
            //shotSFX.gameObject.SetActive(false);
            //shotSFX.gameObject.SetActive(true);
            shotSFX.Play();
        }
    }
}