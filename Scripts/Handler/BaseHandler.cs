using UnityEngine;

public abstract class BaseHandler : MonoBehaviour
{
    public bool IsPlaying { get; private set; } = false;

    public void Play()
    {
        IsPlaying = true;
        gameObject.SetActive(true);
        OnPlay();
        Debug.Log($"Play : {this.name}을 시작합니다.");
    }

    public void Stop()
    {
        IsPlaying = false;
        gameObject.SetActive(false);
        OnStop();

        Debug.Log($"Stop : {this.name}을 종료합니다.");
    }

    protected virtual void OnPlay() { }
    protected virtual void OnStop() { }
}
