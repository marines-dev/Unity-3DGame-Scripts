using System;
using UnityEngine;

public class Util
{
    public static T GetStringToEnum<T>(string pStringName) where T : Enum
    {
        T enumType = (T)Enum.Parse(typeof(T), pStringName);

        return enumType;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    #region Temp

#if UNITY_EDITOR

    public static void LogWarning(string pMsg = "")
    {
        Debug.LogWarning($"Failed : {pMsg}");
    }

    public static void LogError(string pMsg = "")
    {
        Debug.LogError($"Failed : {pMsg}");
    }

    public static void LogSuccess(string pMsg = "")
    {
        Debug.Log($"Success : {pMsg}");
    }

    public static void LogMessage(string pMsg = "")
    {
        Debug.Log($"{pMsg}");
    }

#endif

    public static T CreateGameObject<T>(Transform pParent = null) where T : Component
    {
        string name = $"@{typeof(T).Name}";
        GameObject go = new GameObject(name);

        go.transform.SetParent(pParent);
        go.SetActive(true);

        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;

        return go.AddComponent<T>();
    }

    public static T CreateGlobalObject<T>(Transform pParent = null) where T : Component
    {
        T handler = GameObject.FindObjectOfType<T>();
        if (handler != null && handler.gameObject != null)
        {
            GameObject.Destroy(handler.gameObject);
        }

        handler = CreateGameObject<T>();
        GameObject.DontDestroyOnLoad(handler);

        handler.transform.SetParent(pParent);
        return handler;
    }

    #endregion Temp
}
