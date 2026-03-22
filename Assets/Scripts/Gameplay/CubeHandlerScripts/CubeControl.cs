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
    [SerializeField] private CubeData _baseData;
    public CubeData GetCubeData() => _baseData;

    private CubeSize _currentSize;
    private float _currentMultiplier = 1f;

    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;

    [Header("Visuals")]
    private HealthBar _healthBar;
    [SerializeField] private Renderer _cubeRenderer;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _damagedColor = Color.red;

    public bool IsSelectable { get; set; } = true;

    [Header("Audio Feedback")]
    private SoundPlayer _soundPlayer;

    [Header("Movement")]
    public Vector3 OriginalPosition { get; private set; }
    [SerializeField] private float _moveSpeed = 6f;

    private bool _isBusy = false;

    private void Awake()
    {
        _currentHealth = _maxHealth;

        _soundPlayer = GetComponent<SoundPlayer>();
        _cubeRenderer = GetComponentInChildren<Renderer>();
        _healthBar = GetComponentInChildren<HealthBar>();
    }

    #region --- Core ---
    public void InitializeCube(CubeData data)
    {
        _baseData = data;

        _currentSize = data.cubeSize;
        _currentMultiplier = data.sizeMultiplier;
        _normalColor = data.color;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        transform.localScale = Vector3.one * _currentMultiplier;
        if (_cubeRenderer != null)
            _cubeRenderer.material.color = _normalColor;
    }

    public void SetOriginalPosition(Vector3 position)
        => OriginalPosition = position;

    public bool IsBusy() => _isBusy;
    public Lane GetLane() => _lane;
    public CubeSize GetCubeSize() => _currentSize;

    public void SetLane(Lane newLane)
    {
        _lane = newLane;
    }

    public void SetCubeSize(CubeSize size) => _currentSize = size;

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
        Debug.Log($"{name} moving FROM {transform.position} TO {targetPosition}");

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
    }

    public IEnumerator ReturnToOriginalPosition()
    {
        Debug.Log($"{name} returning to ORIGINAL {OriginalPosition}");
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

    public void IncreaseSize()
    {
        if (_currentSize == CubeSize.Large) return;

        _currentSize = (CubeSize)Mathf.Clamp((int)_currentSize + 1, 0, 2);
        _currentMultiplier += 0.5f;

        UpdateVisuals();
    }

    public void DecreaseSize()
    {
        if (_currentSize == CubeSize.Small) return;

        _currentSize = (CubeSize)Mathf.Clamp((int)_currentSize - 1, 0, 2);
        _currentMultiplier -= 0.5f;

        UpdateVisuals();
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