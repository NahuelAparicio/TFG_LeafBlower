using UnityEngine;
using UnityEngine.UI;

public enum Sens { SensX, SensY }

public class SensOnChange : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private Sens _sensType;

    private void Awake()
    {
        _slider = GetComponent<Slider>();   
        _slider.value = GameManager.Instance.sensX;
        _slider.value = GameManager.Instance.sensY;
    }
    public void OnSliderChange()
    {
        GameManager.Instance.sensX = _slider.value;
        GameManager.Instance.sensY = _slider.value;
    }
}
