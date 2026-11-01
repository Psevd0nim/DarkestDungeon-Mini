using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class Character_UI : MonoBehaviour
    {
        public Character Character { get; private set; }

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _mainView;
        [SerializeField] private Image _cardOutline;

        private Rect _startRect;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _startRect = _rectTransform.rect;
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void Init(Character character)
        {
            Character = character;
            _mainView.sprite = character.Portrait;
            _cardOutline.color = character.Color;
        }

        public void SetTurnCompleted(bool completed)
        {
            _canvasGroup.alpha = completed ? 0.35f : 1f;
        }

        public void SetCardActive(bool active)
        {
            if (active)
            {
                _rectTransform.sizeDelta = _startRect.size * 1.1f;
            }
            else
            {
                _rectTransform.sizeDelta = _startRect.size;
            }

        }
    }
}
