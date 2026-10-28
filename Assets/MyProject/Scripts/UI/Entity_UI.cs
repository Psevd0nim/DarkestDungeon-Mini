using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class Entity_UI : MonoBehaviour
    {
        public Entity Entity => _entity;

        [SerializeField] private Image _mainView;
        [SerializeField] private TextMeshProUGUI _squadNumText;

        private Entity _entity;
        private Vector3 _startScale;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        public void Init(Entity character)
        {
            _entity = character;
            _mainView.color = character.ColorData.color;
            _squadNumText.text = character.SquadPosNum.ToString();
        }

        public void SetCardActvie(bool isActive)
        {
            transform.localScale = isActive ? _startScale * 1.2f : _startScale;
        }
    }
}
