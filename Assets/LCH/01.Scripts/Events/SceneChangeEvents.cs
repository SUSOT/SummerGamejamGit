using UnityEngine;

public static class SceneChangeEvents
{
   public static SceneChangeCheck  SceneChangeCheck = new SceneChangeCheck();
}

public class SceneChangeCheck : GameEvent
{
    public string SceneName;
    public SceneChangeCheck Init(string sceneName)
    {
        SceneName = sceneName;
        return this;
    }
}
