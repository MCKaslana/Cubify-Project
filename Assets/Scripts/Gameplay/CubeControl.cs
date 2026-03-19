using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour
{
    public enum Lane { Left, Middle, Right }

    [Header("Team")]
    [SerializeField] private Team _team;
    public Team GetTeam() => _team;

    [Header("Lane")]
    [SerializeField] private Lane _lane;

    [Header("Stats")]
    [SerializeField] private CubeData _data;
    [SerializeField] private CubeSize _size;
    public CubeData GetCubeData() => _data;
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;

    [Header("Visuals")]
    private HealthBar _healthBar;
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;

    [Header("Audio Feedback")]
    private SoundPlayer _soundPlayer;

    [Header("Movement")]
    public Vector3 OriginalPosition { get; private set; }
    [SerializeField] private float _moveSpeed = 6f;

    private bool _isBusy = false;

    private void Awake()
    {
        OriginalPosition = transform.position;
        _currentHealth = _maxHealth;

        _soundPlayer = GetComponent<SoundPlayer>();
        _healthBar = GetComponentInChildren<HealthBar>();

        if (_cubeRenderer != null)
            _cubeRenderer.material.color = _normalColor;
    }

    #region --- Core State ---
    public bool IsBusy() => _isBusy;
    public Lane GetLane() => _lane;

    public void SetLane(Lane newLane)
    {
        _lane = newLane;
    }

    public void SetCubeSize(CubeSize size) => _size = size;

    #endregion

    #region --- Health ---

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        UpdateCubeHealth();

        Debug.Log($"{name} took {amount} damage. HP: {_currentHealth}");

        StartCoroutine(DamageFeedback());

        if (_currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_currentHealth, _maxHealth);

        UpdateCubeHealth();
    }

    private void UpdateCubeHealth() => _healthBar.UpdateHealthValue(_currentHealth);

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

    public IEnumerator SwapWith(CubeControl other)
    {
        if (_isBusy || other._isBusy || other == null || other == this)
            yield break;

        _isBusy = true;
        other._isBusy = true;

        Vector3 myStart = transform.position;
        Vector3 otherStart = other.transform.position;

        Coroutine moveA = StartCoroutine(MoveTo(otherStart));
        Coroutine moveB = other.StartCoroutine(other.MoveTo(myStart));

        yield return moveA;
        yield return moveB;

        Vector3 tempOriginal = other.OriginalPosition;
        other.OriginalPosition = OriginalPosition;
        OriginalPosition = tempOriginal;

        Lane tempLane = other._lane;
        other._lane = _lane;
        _lane = tempLane;

        Team myTeam = _team;
        Team otherTeam = other._team;

        if (myTeam != otherTeam)
        {
            _team = otherTeam;
            other._team = myTeam;

            Debug.Log($"{name} is now on {_team} team");
            Debug.Log($"{other.name} is now on {other._team} team");
        }

        _isBusy = false;
        other._isBusy = false;
    }

    public void SetTeam(Team newTeam)
    {
        _team = newTeam;
    }

    #endregion

    #region --- Modifiers ---

    public void Modify(CubeData newData)
    {
        _data = newData;
        transform.localScale = Vector3.one * _data.sizeMultiplier;

        if (_cubeRenderer != null)
            _cubeRenderer.material.color = _data.color;
    }

    public void Modify(float sizeMultiplier, Color color)
    {
        transform.localScale = Vector3.one * sizeMultiplier;
        if (_cubeRenderer != null)
            _cubeRenderer.material.color = color;
    }

    #endregion

    #region --- Feedback ---

    private IEnumerator DamageFeedback()
    {
        if (_cubeRenderer == null) yield break;

        _cubeRenderer.material.color = _damagedColor;

        yield return new WaitForSeconds(0.2f);

        _cubeRenderer.material.color = _normalColor;
    }

    public void PlaySound(int index)
    {
        _soundPlayer?.Play(index);
    }

    #endregion
}