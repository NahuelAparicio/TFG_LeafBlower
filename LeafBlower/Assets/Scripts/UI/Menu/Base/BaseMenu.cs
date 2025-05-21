using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseMenu<T> : Menu<T> where T: BaseMenu<T>
{
    protected static EventSystem _eventSystem;
    protected override void Awake()
    {
        base.Awake();
        _eventSystem = EventSystem.current;
        GameManager.Instance.UnlockCursor();
    }

    protected void OnEnable()
    {
    }



    public static void Show()
    {
        Open();
    }

    public static void Hide()
    {
        //FadeOut
        Close();
    }

}
