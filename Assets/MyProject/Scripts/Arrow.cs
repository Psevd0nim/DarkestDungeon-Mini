using DG.Tweening;
using UnityEngine;

namespace MyProject
{
    public class Arrow : MonoBehaviour
    {
        private Sequence _arrowSequence;

        private void Start()
        {
            _arrowSequence = DOTween.Sequence()
                .Append(transform.DOLocalMoveY(0.5f, 0.5f).SetRelative(true).SetEase(Ease.InOutSine))
                .Append(transform.DOLocalMoveY(-0.5f, 0.5f).SetRelative(true).SetEase(Ease.InOutSine))
                .SetLoops(-1);
        }

        private void OnEnable()
        {
            _arrowSequence.Play();
        }

        private void OnDisable()
        {
            _arrowSequence.Pause();
        }
    }
}
