using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T:class, new()
{
    private static T instance_ = null;
    public static T instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = new T();

                Debug.Log($"SingletonManager<{typeof(T).ToString()}>가 생성되었습니다.");
            }
            return instance_;
        }
    }

    public Singleton()
    { }

    public void Release()
    {
        if(instance_ != null)
        {
            instance_ = null;

            Debug.Log($"SingletonManager<{typeof(T).ToString()}>가 삭제되었습니다.");
        }
    }
}
