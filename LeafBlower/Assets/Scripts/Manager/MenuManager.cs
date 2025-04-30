using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    [SerializeField] private Stack<Menu> menuStack = new Stack<Menu>();

    public MainMenu menu;
    public PauseMenu pause;
    public SettingsMenu settings;

    public Enums.MenuState currentState;
    public Enums.MenuState lastState;

    public static MenuManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("Menu Manager");
                go.AddComponent<MenuManager>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadResources();
    }

    private void Update()
    {
        //switch (currentState)
        //{
        //    case Enums.MenuState.MainMenu:
        //        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
        //        {
        //            EventSystem.current.SetSelectedGameObject(null);
        //            menu.OnRetarget();
        //        }
        //        break;
        //    case Enums.MenuState.SettingsMenu:
        //        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
        //        {
        //            EventSystem.current.SetSelectedGameObject(null);
        //            settings.OnRetarget();
        //        }
        //        break;
        //    case Enums.MenuState.PauseMenu:
        //        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
        //        {
        //            EventSystem.current.SetSelectedGameObject(null);
        //            pause.OnRetarget();
        //        }
        //        break;
        //    default:
        //        break;
        //}
    }

    public void ChangeMenuState(Enums.MenuState newState)
    {
        if (currentState == newState) return;
        lastState = currentState;
        currentState = newState;
    }
    private void LoadResources()
    {
        menu = Resources.Load<MainMenu>("Menus/MainMenu");
        pause = Resources.Load<PauseMenu>("Menus/PauseMenu");
        settings = Resources.Load<SettingsMenu>("Menus/SettingsMenu");
    }

    public void CreateInstance<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();
        Instantiate(prefab, transform);
    }

    public void OpenMenu(Menu instance)
    {
        menuStack = new Stack<Menu>(menuStack.Where(menu => menu != null));
        if (menuStack.Count > 0)
        {
            if (instance.disableMenuUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                    if (menu.disableMenuUnderneath)
                    {
                        break;
                    }
                }
            }
            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = menuStack.Peek().GetComponent<Canvas>();
            topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
        }
        menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : Menu
    {
        // Get prefab dynamically, based on public fields set from Unity
        // You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }
        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void CloseMenu(Menu menu)
    {
        if (menuStack.Count == 0)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed bc menu stack is empty", menu.GetType());
        }
        if (menuStack.Peek() != menu)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }
        CloseTopMenu();
    }

    public void ClearStack()
    {
        menuStack.Clear();
    }

    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.destroyWhenClosed)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance.gameObject.SetActive(false);
        }

        // Re active top menu
        foreach (var menu in menuStack)
        {
            menu.gameObject.SetActive(true);
            if (menu.disableMenuUnderneath)
            {
                break;
            }
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
