using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseMenu<T> : Menu<T> where T: BaseMenu<T>
{
    public GameObject firstSelectedInspector;
    protected static GameObject _firstSelected;
    protected static EventSystem _eventSystem;
    protected override void Awake()
    {
        base.Awake();
        _eventSystem = EventSystem.current;
        _firstSelected = firstSelectedInspector;
        GameManager.Instance.UnlockCursor();
    }

    protected void OnEnable()
    {
        StartCoroutine(highlightButton());
    }



    public static void Show()
    {
        Open();
        _eventSystem.firstSelectedGameObject = _firstSelected;
    }

    public static void Hide()
    {
        //FadeOut
        Close();
    }

    private IEnumerator highlightButton()
    {
        _eventSystem.SetSelectedGameObject(null);
        yield return null;
        _eventSystem.SetSelectedGameObject(_firstSelected);

    }
}
