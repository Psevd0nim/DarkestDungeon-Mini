using System.Collections;
using TMPro;
using UnityEngine;

namespace MyProject
{
    public class CharacterView : MonoBehaviour
    {
        public float ActionDuration(SkillConfig config)
        {
            return config is SkipTurnSkillConfig ? 0.45f : _attack.length;
        }

        public float ReactionDuration => Mathf.Max(_hurt.length, _death.length);

        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationClip _attack;
        [SerializeField] private AnimationClip _hurt;
        [SerializeField] private AnimationClip _death;
        [SerializeField] private TextMeshPro _feedback;
        private Coroutine _feedbackRoutine;
        private Vector3 _feedbackStart;
        private Vector3 _artworkStart;

        private void Awake()
        {
            _feedbackStart = _feedback.transform.localPosition;
            _artworkStart = _animator.transform.localPosition;
            _feedback.text = "";
        }

        public void ReverseFeedback()
        {
            var scale = _feedback.transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            _feedback.transform.localScale = scale;
        }

        public void PlayAttack()
        {
            _animator.Play("Attack", 0, 0);
        }

        public void PlayAction(SkillConfig config)
        {
            if (config is SkipTurnSkillConfig)
            {
                StartCoroutine(WaitGesture());
            }
            else
            {
                PlayAttack();
            }
        }

        private IEnumerator WaitGesture()
        {
            float elapsed = 0;
            while (elapsed < 0.45f)
            {
                elapsed += Time.deltaTime;
                _animator.transform.localPosition = _artworkStart + Vector3.up * Mathf.Sin(Mathf.Clamp01(elapsed / 0.45f) * Mathf.PI) * 0.12f;
                yield return null;
            }

            _animator.transform.localPosition = _artworkStart;
        }

        public void PlayHurt()
        {
            _animator.Play("Hurt", 0, 0);
        }

        public void PlayDeath()
        {
            _animator.Play("Death", 0, 0);
        }

        public void SetPaused(bool paused)
        {
            _animator.speed = paused ? 0 : 1;
        }

        public void ResetPose(bool alive)
        {
            StopAllCoroutines();
            _feedbackRoutine = null;
            _feedback.text = "";
            _feedback.transform.localPosition = _feedbackStart;
            _animator.transform.localPosition = _artworkStart;
            _animator.speed = 1;
            _animator.Play(alive ? "Idle" : "Death", 0, alive ? 0 : 1);
        }

        public void ShowFeedback(string text, Color color)
        {
            if (_feedbackRoutine != null)
            {
                StopCoroutine(_feedbackRoutine);
            }

            _feedbackRoutine = StartCoroutine(Feedback(text, color));
        }

        private IEnumerator Feedback(string text, Color color)
        {
            _feedback.text = text;
            float elapsed = 0;
            while (elapsed < 0.85f)
            {
                elapsed += Time.deltaTime;
                _feedback.transform.localPosition = _feedbackStart + Vector3.up * (elapsed * 0.45f);
                color.a = 1 - Mathf.Clamp01((elapsed - 0.45f) / 0.4f);
                _feedback.color = color;
                yield return null;
            }

            _feedback.text = "";
            _feedbackRoutine = null;
        }
    }
}
