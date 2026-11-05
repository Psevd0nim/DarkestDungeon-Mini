using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MyProject
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshPro _feedback;

        private Action _onAttackImpact;
        private Action _onAttackCompleted;
        private Action _onReactionCompleted;
        private Sequence _feedbackSequence;
        private Vector3 _feedbackStart;

        private void Awake()
        {
            _feedbackStart = _feedback.transform.localPosition;
            _feedback.text = "";
        }

        private void OnDisable()
        {
            CancelAnimation();
        }

        public void ReverseFeedback()
        {
            Vector3 scale = _feedback.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            _feedback.transform.localScale = scale;
        }

        public void PlayAttack(Action onImpact = null, Action onCompleted = null)
        {
            _onAttackImpact = onImpact;
            _onAttackCompleted = onCompleted;
            _animator.ResetTrigger("Reset");
            _animator.SetTrigger("Attack");
        }

        public void PlayHurt(Action onCompleted = null)
        {
            PlayReaction(false, onCompleted);
        }

        public void PlayDeath(Action onCompleted = null)
        {
            PlayReaction(true, onCompleted);
        }

        public void ShowFeedback(string text, Color color, Action onCompleted = null)
        {
            ClearFeedback();
            _feedback.text = text;
            _feedback.color = color;
            _feedbackSequence = DOTween.Sequence()
                .Append(_feedback.transform.DOLocalMoveY(_feedbackStart.y + 0.3825f, 0.85f).SetEase(Ease.Linear))
                .Insert(0.45f, DOTween.To(() => _feedback.color, value => _feedback.color = value,
                    new Color(color.r, color.g, color.b, 0), 0.4f).SetEase(Ease.Linear))
                .OnComplete(() => 
                {
                    onCompleted?.Invoke();
                    ClearFeedback();
                });
        }

        public void CancelAnimation()
        {
            _onAttackImpact = null;
            _onAttackCompleted = null;
            _onReactionCompleted = null;
            ClearFeedback();
            _animator.ResetTrigger("Attack");
            _animator.ResetTrigger("Hurt");
            _animator.SetTrigger("Reset");
        }

        public void AttackImpact()
        {
            Action callback = _onAttackImpact;
            _onAttackImpact = null;
            callback?.Invoke();
        }

        public void AttackCompleted()
        {
            Action callback = _onAttackCompleted;
            _onAttackCompleted = null;
            callback?.Invoke();
        }

        public void HurtCompleted()
        {
            if (_animator.GetBool("IsDead"))
            {
                return;
            }

            CompleteReaction();
        }

        public void DeathCompleted()
        {
            CompleteReaction();
        }

        private void PlayReaction(bool isDead, Action onCompleted)
        {
            _onReactionCompleted = onCompleted;
            _animator.ResetTrigger("Reset");
            _animator.SetBool("IsDead", isDead);
            _animator.SetTrigger("Hurt");
        }

        private void CompleteReaction()
        {
            Action callback = _onReactionCompleted;
            _onReactionCompleted = null;
            callback?.Invoke();
        }

        private void ClearFeedback()
        {
            _feedbackSequence?.Kill();
            _feedbackSequence = null;
            _feedback.text = "";
            _feedback.transform.localPosition = _feedbackStart;
        }
    }
}
