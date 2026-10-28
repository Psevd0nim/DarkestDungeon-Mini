using TMPro;
using UnityEngine;

namespace MyProject
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _healthBarView;
        [SerializeField] private TextMeshPro _healthText;

        private Material _material;

        private void Awake()
        {
            _material = new Material(_healthBarView.material);
            _healthBarView.material = _material;
        }

        public void UpdateHealthBar(float currentValue, float maxValue)
        {
            _healthText.text = $"{currentValue} / {maxValue}";
            _material.SetFloat("_FillAmount", currentValue / maxValue);
        }

        public void ReverseHealthBar()
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}
