using UnityEngine;
using UnityEngine.UI;

public class BlowerHUD : MonoBehaviour
{
    [SerializeField] private Image _fillShootForce;
    [SerializeField] private GameObject _fillShootGo;

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
}
