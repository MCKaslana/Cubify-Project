using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    private void Awake()
    {
        _healthBar = GetComponent<Slider>();
    }

    public void UpdateHealthValue(int value)
    {
        if (_healthBar != null)
        {
            _healthBar.value = value;
        }
    }
}
