using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderValue : MonoBehaviour
{
    private Slider _slider;

    [SerializeField] private Enums.VolumeType _type;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (_type)
        {
            case Enums.VolumeType.Master:
                _slider.value = MusicManager.Instance.masterVolume;
                break;
            case Enums.VolumeType.Music:
                _slider.value = MusicManager.Instance.musicVolume;

                break;
            case Enums.VolumeType.Sfx:
                _slider.value = MusicManager.Instance.SFXVolume;

                break;
            case Enums.VolumeType.Ambience:
                _slider.value = MusicManager.Instance.ambienceVolume;

                break;
            default:
                break;
        }
    }

    public void OnSliderChange()
    {
        switch (_type)
        {
            case Enums.VolumeType.Master:
                MusicManager.Instance.masterVolume = _slider.value;
                break;
            case Enums.VolumeType.Music:
                MusicManager.Instance.musicVolume = _slider.value;

                break;
            case Enums.VolumeType.Sfx:
                MusicManager.Instance.SFXVolume = _slider.value;

                break;
            case Enums.VolumeType.Ambience:
                MusicManager.Instance.ambienceVolume = _slider.value;

                break;
            default:
                Debug.LogWarning("Volume Type not suported " + _type);
                break;
        }
    }

}
