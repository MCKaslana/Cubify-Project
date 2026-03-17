using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour
{
    public enum Lane { Left, Middle, Right }

    [Header("Team")]
    [SerializeField] private bool _isPlayerUnit;

    [Header("Lane")]
    [SerializeField] private Lane _lane;

    [Header("Stats")]
    [SerializeField] private int _maxHealth = 10;
    private int _currentHealth;

    [Header("Visuals")]
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;

    [Header("Movement")]
    public Vector3 OriginalPosition { get; private set; }
    [SerializeField] private float _moveSpeed = 6f;

    private bool _isBusy = false;

    private void Awake()
    {
        OriginalPosition = transform.position;
        _currentHealth = _maxHealth;

        if (_cubeRenderer != null)
            _cubeRenderer.material.color = _normalColor;
    }

    #region --- Core State ---

    public bool IsPlayerUnit() => _isPlayerUnit;
    public bool IsBusy() => _isBusy;
    public Lane GetLane() => _lane;

    public void SetLane(Lane newLane)
    {
        _lane = newLane;
    }

    #endregion

    #region --- Health ---

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        Debug.Log($"{name} took {amount} damage. HP: {_currentHealth}");

        StartCoroutine(DamageFeedback());

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);
    }

    private void Die()
    {
        Debug.Log(name + " died.");
        gameObject.SetActive(false);
    }

    #endregion

    #region --- Movement ---

    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        _isBusy = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;

        _isBusy = false;
    }

    public IEnumerator ReturnToOriginalPosition()
    {
        yield return MoveTo(OriginalPosition);
    }

    #endregion

    #region --- Swapping ---

    public void SwapWith(CubeControl other)
    {
        if (_isBusy || other._isBusy) return;

        Vector3 tempPos = other.transform.position;
        other.transform.position = transform.position;
        transform.position = tempPos;

        Vector3 tempOriginal = other.OriginalPosition;
        other.OriginalPosition = OriginalPosition;
        OriginalPosition = tempOriginal;

        Lane tempLane = other._lane;
        other._lane = _lane;
        _lane = tempLane;
    }

    #endregion

    #region --- Modifiers ---

    public void Modify(float scaleMultiplier, Color newColor)
    {
        transform.localScale *= scaleMultiplier;

        if (_cubeRenderer != null)
            _cubeRenderer.material.color = newColor;
    }

    #endregion

    #region --- Visual Feedback ---

    private IEnumerator DamageFeedback()
    {
        if (_cubeRenderer == null) yield break;

        _cubeRenderer.material.color = _damagedColor;

        yield return new WaitForSeconds(0.2f);

        _cubeRenderer.material.color = _normalColor;
    }

    #endregion
}