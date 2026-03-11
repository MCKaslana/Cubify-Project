using UnityEngine;
using System.Collections;

public class CubeControl : MonoBehaviour
{
    private Vector3 _originalPosition;
    [SerializeField] private float _delayBeforeReturn = 4f;

    private void Start()
    {
        _originalPosition = transform.position;
        CombatManager.Instance.OnCombatEnded += HandleCombatEnded;
    }

    public void BeginAction()
    {
        CombatManager.Instance.JoinCombat(gameObject);

        StartCoroutine(BeginReturnMovement());
    }

    private IEnumerator BeginReturnMovement()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _delayBeforeReturn)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        float returnSpeed = 10f;

        while (Vector3.Distance(transform.position, _originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _originalPosition,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = _originalPosition;
    }

    private void HandleCombatEnded()
    {
        Debug.Log("Combat ended, cube can return to original position.");
        CombatManager.Instance.LeaveCombat(gameObject);
    }
}