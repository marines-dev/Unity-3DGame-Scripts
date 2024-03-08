using System;
using UnityEngine;

public static class Extension
{
    #region GameObject

    public static T GetOrAddComponent<T>(this GameObject pGO) where T : Component
    {
        T baseComp = pGO.GetComponent<T>();
        if (baseComp == null)
        {
            baseComp = pGO.AddComponent<T>();
        }

        return baseComp;
    }

    public static Component GetOrAddComponent(this GameObject pGO, Type pType)
    {
        Component component = pGO.GetComponent(pType);
        if (component == null)
            component = pGO.AddComponent(pType);

        return component;
    }

    public static bool IsActiveSelf(this GameObject pGO)
    {
        return pGO.activeSelf;
    }

    //public static T GetComponent<T>(this GameObject pGO) where T : Component
    //{
    //    if (pGO == null)
    //    {
    //        Debug.LogWarning($"Failed : ");
    //        return null;
    //    }

    //    T baseComp = pGO.GetComponent<T>();
    //    if (baseComp == null)
    //    {
    //        Debug.LogError($"Failed : ");
    //        return null;
    //    }

    //    return baseComp;
    //}

    //public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    //{
    //	UI_Base.BindEvent(go, action, type);
    //}

    ////Spawning
    //public static void DespawnOrDestroy(this GameObject myobject)
    //{
    //    Managers.Spawn.Despawn(myobject);
    //}

    #endregion GameObject

    #region Transform

    public static Transform GetChild(this Transform pTrans, string pName)
    {
        if (pTrans == null || string.IsNullOrEmpty(pName))
        {
            Debug.LogWarning($"");
            return null;
        }

        Transform transform = GetChild<Transform>(pTrans, pName);
        if (transform == null)
            return null;

        return transform;
    }

    public static T GetChild<T>(this Transform pTrans, string pName) where T : UnityEngine.Object
    {
        if (pTrans == null || string.IsNullOrEmpty(pName))
        {
            Debug.LogWarning($"");
            return null;
        }

        for (int i = 0; i < pTrans.transform.childCount; i++)
        {
            Transform transform = pTrans.transform.GetChild(i);
            if (transform.name == pName)
            {
                T component = transform.GetComponent<T>();
                if (component != null)
                    return component;
            }
        }

        return null;
    }

    //public static Transform GetDescendant(this Transform pTrans, string pName)
    //{
    //    if (pTrans == null || string.IsNullOrEmpty(pName))
    //    {
    //        Debug.LogWarning($"");
    //        return null;
    //    }

    //    Transform transform = GetDescendant<Transform>(pTrans, pName);
    //    if (transform == null)
    //        return null;

    //    return transform;
    //}

    //public static T GetDescendant<T>(this Transform pTrans, string pName) where T : UnityEngine.Object
    //{
    //    if (pTrans == null || string.IsNullOrEmpty(pName))
    //    {
    //        Debug.LogWarning($"");
    //        return null;
    //    }

    //    foreach (T component in pTrans.GetComponentsInChildren<T>())
    //    {
    //        if (component.name == pName)
    //            return component;
    //    }

    //    return null;
    //}

    //public static T[] GetDescendants<T>(this Transform pTrans) where T : UnityEngine.Object
    //{
    //    if (pTrans == null)
    //    {
    //        Debug.LogWarning($"");
    //        return null;
    //    }

    //    return pTrans.GetComponentsInChildren<T>();
    //}

    //public static T FindChild<T>(this Transform pTrans, string pName, bool pRecursive) where T : UnityEngine.Object
    //{
    //    if (pTrans == null || string.IsNullOrEmpty(pName))
    //        return null;

    //    if (pRecursive == false)
    //    {
    //        for (int i = 0; i < pTrans.transform.childCount; i++)
    //        {
    //            Transform transform = pTrans.transform.GetChild(i);
    //            if (transform.name == pName)
    //            {
    //                T component = transform.GetComponent<T>();
    //                if (component != null)
    //                    return component;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (T component in pTrans.GetComponentsInChildren<T>())
    //        {
    //            if (component.name == pName)
    //                return component;
    //        }
    //    }

    //    return null;
    //}

    #endregion Transform
}
