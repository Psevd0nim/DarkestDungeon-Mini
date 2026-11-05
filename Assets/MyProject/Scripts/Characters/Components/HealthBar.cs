using TMPro;
using UnityEngine;

namespace MyProject
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _healthBarView;
        [SerializeField] private TextMeshPro _healthText;

        private Vector3 _fullScale;
        private Vector3 _fullPosition;
        private float _width;

        private void Awake()
        {
            _fullScale = _healthBarView.transform.localScale;
            _fullPosition = _healthBarView.transform.localPosition;
            _width = _healthBarView.sprite.bounds.size.x * _fullScale.x;
        }

        public void UpdateHealthBar(float currentValue, float maxValue)
        {
            _healthText.text = currentValue + " / " + maxValue;
            float fraction = Mathf.Clamp01(currentValue / maxValue);
            Vector3 scale = _fullScale;
            scale.x *= fraction;
            _healthBarView.transform.localScale = scale;
            _healthBarView.transform.localPosition = _fullPosition - Vector3.right * (_width * (1 - fraction) * 0.5f);
        }

        public void ReverseHealthBar()
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}
