using UnityEngine;
using UnityEngine.UI;

public class IconToShow : MonoBehaviour
{
    public Sprite interactXboxIcon, interactPlayIcon;

    private Sprite _icon;

    private void Awake()
    {
        _icon = GetComponent<Image>().sprite;
    }

    private void Start()
    {
        string[] joystickNames = Input.GetJoystickNames();

        foreach (string name in joystickNames)
        {
            if(name.ToLower().Contains("xbox"))
            {
                _icon = interactXboxIcon;
                break;
            }
            else if(name.ToLower().Contains("playstation"))
            {
                _icon = interactPlayIcon;
                break;
            }
            else
            {
                _icon = interactXboxIcon;
                break;
            }
        }
    }
}
