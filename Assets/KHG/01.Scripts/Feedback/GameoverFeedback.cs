using UnityEngine;
using UnityEngine.Rendering;

public class GameoverFeedback : Feedback
{
    [SerializeField] private Volume GameoverVolume;
    public override void CreateFeedback()
    {
        GameoverVolume.enabled = true;
    }

    public override void StopFeedback()
    {
        GameoverVolume.enabled = false;
    }
}
