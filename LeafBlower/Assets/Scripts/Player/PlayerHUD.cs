using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Image _fillStamina;
    [SerializeField] private TextMeshProUGUI _text;

    public void UpdateCoinsText(int coins) => _text.text = "" + coins;       

    public void ResetStaminaBar() => _fillStamina.fillAmount = 1;

    public void UpdateStaminaBar(float currentStamina, float maxStamina)
    {
        _fillStamina.fillAmount = Mathf.Clamp01(currentStamina / maxStamina);
        UpdateStaminaColor(currentStamina, maxStamina);
    }

    private void UpdateStaminaColor(float currentStamina, float maxStamina)
    {
        float staminaClamped = Mathf.Clamp01(currentStamina / maxStamina);
        if (staminaClamped <= 0.15f)
        {
            _fillStamina.color = Color.red;
        }
        else if (staminaClamped > 0.15f && staminaClamped <= 0.60f)
        {
            _fillStamina.color = new Color(1.0f, 0.64f, 0.0f);
        }
        else
        {
            _fillStamina.color = Color.green;
        }
    }
}
