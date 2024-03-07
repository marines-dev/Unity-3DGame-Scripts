﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
		return Util.GetOrAddComponent<T>(go);
	}

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    //public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    //{
    //	UI_Base.BindEvent(go, action, type);
    //}

    ////Spawning
    //public static void DespawnOrDestroy(this GameObject myobject)
    //{
    //    Managers.Spawn.Despawn(myobject);
    //}
}