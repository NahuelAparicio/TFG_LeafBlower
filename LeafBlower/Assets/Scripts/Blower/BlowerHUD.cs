using UnityEngine;
using UnityEngine.UI;

public class BlowerHUD : MonoBehaviour
{
    [SerializeField] private Image _fillShootForce;
    [SerializeField] private GameObject _fillShootGo;

    [SerializeField] private Image _fillStamina;
    //[SerializeField] private GameObject _staminaParent;

    private void Awake()
    {
        _fillShootForce.fillAmount = 0;
        HideShootBar();
    }
    private void ShowShootBar() => _fillShootForce.gameObject.SetActive(true);
    private void HideShootBar() => _fillShootForce.gameObject.SetActive(false);
    public void UpdateShootBarForce(float timePressed, float maxTime)
    {
        if(!_fillShootForce.gameObject.activeSelf)
        {
            ShowShootBar();
        }
        _fillShootForce.fillAmount = Mathf.Clamp01(timePressed / maxTime);
    }
    public void ResetShootBarForce()
    {
        _fillShootForce.fillAmount = 0;
        Invoke(nameof(HideShootBar), 0.25f);
    }

    public void ResetStaminaBar()
    {
        _fillStamina.fillAmount = 1;
    }
    public void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        _fillStamina.fillAmount = Mathf.Clamp01(currentStamina / maxStamina);
        UpdateStaminaColor(currentStamina, maxStamina);
    }

    private void UpdateStaminaColor(float currentStamina, float maxStamina)
    {
        float staminaClamped = Mathf.Clamp01(currentStamina / maxStamina);
        if(staminaClamped <= 0.15f)
        {
            _fillStamina.color = Color.red;
        }
        else if(staminaClamped > 0.15f && staminaClamped <= 0.60f)
        {
            _fillStamina.color = new Color(1.0f, 0.64f, 0.0f);
        }
        else
        {
            _fillStamina.color = Color.green;
        }
    }
}
