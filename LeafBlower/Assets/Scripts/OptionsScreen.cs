using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class OptionsScreen : MonoBehaviour
{
    public Toggle fullScreenTog, vsyncTog;
    public List<ResolutionItems> resolutions = new List<ResolutionItems>();
    private int _selectedResolution;

    public TextMeshProUGUI textResolution;
    void Start()
    {
        _selectedResolution = 0;
        UpdateResText();

        fullScreenTog.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundResoulution = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].resX && Screen.height == resolutions[i].resY)
            {
                foundResoulution = true;
                _selectedResolution = i;
                UpdateResText();
            }
        }

        if(!foundResoulution)
        {
            ResolutionItems newResoulution = new ResolutionItems();
            newResoulution.resX = Screen.width;
            newResoulution.resY = Screen.height;
            resolutions.Add(newResoulution);
            _selectedResolution = resolutions.Count - 1;
            UpdateResText();
        }

    }

    public void ResLeft()
    {
        _selectedResolution--;
        if (_selectedResolution < 0)
            _selectedResolution = 0;

        UpdateResText();
    }
    public void ResRight()
    {
        _selectedResolution++;
        if(_selectedResolution > resolutions.Count - 1)
        {
            _selectedResolution = resolutions.Count - 1;
        }
        UpdateResText();
    }

    public void UpdateResText()
    {
        textResolution.text = "" + resolutions[_selectedResolution].resX + " x " + resolutions[_selectedResolution].resY;
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullScreenTog.isOn;
        if(vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resolutions[_selectedResolution].resX, resolutions[_selectedResolution].resY, fullScreenTog.isOn);
    }
}

[System.Serializable]
public class ResolutionItems
{
    public int resX, resY;
}
