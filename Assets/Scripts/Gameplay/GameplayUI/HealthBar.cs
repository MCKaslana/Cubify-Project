using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    private LookAtConstraint _constraint;
    private Transform _targetTransform;

    private void Awake()
    {
        _healthBar = GetComponentInChildren<Slider>();
        _constraint = GetComponent<LookAtConstraint>();

        _healthBar.value = _healthBar.maxValue;
        _targetTransform = Camera.main.transform;
    }

    private void Start()
    {
        if (_constraint != null && _targetTransform != null)
        {
            ConstraintSource newSource = new ConstraintSource();
            newSource.sourceTransform = _targetTransform;
            newSource.weight = 1.0f;

            var sources = new List<ConstraintSource>();
            sources.Add(newSource);

            _constraint.SetSources(sources);

            _constraint.locked = true;
        }
    }

    public void UpdateHealthValue(int value)
    {
        if (value > _healthBar.maxValue) return;

        if (_healthBar != null)
        {
            _healthBar.value = value;
        }
    }
}
