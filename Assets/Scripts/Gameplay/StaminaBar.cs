using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image _barFill;
    [SerializeField] private float _maxStamina = 100f;
    private float _currentStamina;

    private void Start()
    {
        _currentStamina = _maxStamina;
        UpdateBar();
    }

    public void SetStamina(float stamina)
    {
        _currentStamina = Mathf.Clamp(stamina, 0, _maxStamina);
        UpdateBar();
    }

    private void UpdateBar()
    {
        float fillAmount = _currentStamina / _maxStamina;
        _barFill.fillAmount = fillAmount;
    }
}
