using UnityEngine;

public static class TitleEvents
{
    public static Setting Setting = new Setting();
}

public class Setting : GameEvent
{
    public bool Open = false;

    public Setting Init(bool isOpen)
    {
        Open = isOpen;
        return this;
    }
}
