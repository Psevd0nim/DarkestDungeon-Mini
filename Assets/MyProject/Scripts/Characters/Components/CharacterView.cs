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
        [SerializeField] private float _feedbackMoveHeight = 0.4f;
        [SerializeField] private float _feedbackMoveDuration = 0.85f;
        [SerializeField] private float _feedbackFadeDelay = 0.45f;
        [SerializeField] private float _feedbackFadeDuration = 0.4f;

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
            _onReactionCompleted = onCompleted;
            _animator.ResetTrigger("Reset");
            _animator.SetBool("IsDead", false);
            _animator.SetTrigger("Hurt");
        }

        public void PlayDeath(Action onCompleted = null)
        {
            _onReactionCompleted = onCompleted;
            _animator.ResetTrigger("Reset");
            _animator.SetBool("IsDead", true);
            _animator.SetTrigger("Hurt");
        }

        public void ShowFeedback(string text, Color color, Action onCompleted = null)
        {
            ClearFeedback();
            _feedback.text = text;
            _feedback.color = color;
            float targetY = _feedbackStart.y + _feedbackMoveHeight;
            Color transparentColor = color;
            transparentColor.a = 0f;
            Tweener moveTween = _feedback.transform.DOLocalMoveY(targetY, _feedbackMoveDuration)
                .SetEase(Ease.Linear);
            Tweener fadeTween = DOTween.To(
                () => _feedback.color,
                value => _feedback.color = value,
                transparentColor,
                _feedbackFadeDuration)
                .SetEase(Ease.Linear);

            _feedbackSequence = DOTween.Sequence()
                .Append(moveTween)
                .Insert(_feedbackFadeDelay, fadeTween)
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

        // Called by an Animation Event in an animation clip.
        public void AttackImpact()
        {
            Action callback = _onAttackImpact;
            _onAttackImpact = null;
            callback?.Invoke();
        }

        // Called by an Animation Event in an animation clip.
        public void AttackCompleted()
        {
            Action callback = _onAttackCompleted;
            _onAttackCompleted = null;
            callback?.Invoke();
        }

        // Called by an Animation Event in an animation clip.
        public void HurtCompleted()
        {
            if (_animator.GetBool("IsDead"))
            {
                return;
            }

            CompleteReaction();
        }

        // Called by an Animation Event in an animation clip.
        public void DeathCompleted()
        {
            CompleteReaction();
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
