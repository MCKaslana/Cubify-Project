using UnityEngine;
using DG.Tweening;
public class ShopCardAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _delay;

    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = transform.localPosition;

        transform.localPosition += new Vector3(0, -200f, 0);
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        AnimateIn();
    }

    public void AnimateIn()
    {
        transform.DOScale(1f, _duration).SetEase(Ease.OutBack).SetDelay(_delay);
        
        transform.DOLocalMoveY(_startPos.y, _duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(_delay);
    }
}
