using UnityEngine;

public abstract class TimeLinePattern : MonoBehaviour
{
    public float startTime;
    
    public abstract void Execute();

    public TimeLinePattern(float startTime)
    {
        this.startTime = startTime;
    }
}