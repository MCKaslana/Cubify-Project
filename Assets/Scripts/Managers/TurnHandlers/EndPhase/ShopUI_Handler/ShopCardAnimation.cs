using UnityEngine;
using DG.Tweening;
public class ShopCardAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _delay;

    private Vector3 _startPos;

    private void OnEnable()
    {
        AnimateIn();
    }

    public void AnimateIn()
    {
        transform.DOKill();

        _startPos = transform.localPosition;

        transform.localPosition = _startPos + new Vector3(0, -200f, 0);

        transform.DOLocalMoveY(_startPos.y, _duration).SetEase(Ease.OutCubic).SetDelay(_delay);

    }

}
