//using UnityEngine;

//public abstract class BaseHandler_Legacy : MonoBehaviour
//{
//    public bool IsPlaying { get; private set; } = false;

//    public void Play()
//    {
//        IsPlaying = true;
//        gameObject.SetActive(true);
//        OnPlay();

//        Util.LogSuccess($"{this.name}�� �����մϴ�.");
//    }

//    public void Stop()
//    {
//        IsPlaying = false;
//        gameObject.SetActive(false);
//        OnStop();

//        Util.LogSuccess($"{this.name}�� �����մϴ�.");
//    }

//    protected virtual void OnPlay() { }
//    protected virtual void OnStop() { }
//}
