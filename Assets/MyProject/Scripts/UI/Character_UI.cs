using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyProject
{
    public class Character_UI : MonoBehaviour
    {
        public Character Character => _character;

        [SerializeField] private Image _mainView;
        [SerializeField] private TextMeshProUGUI _squadNumText;

        private Character _character;
        private Vector3 _startScale;

        private void Awake()
        {
            _startScale = transform.localScale;
        }

        public void Init(Character character)
        {
            _character = character;
            _mainView.color = character.ColorData.color;
            _squadNumText.text = character.SquadPosNum.ToString();
        }

        public void SetCardActvie(bool isActive)
        {
            transform.localScale = isActive ? _startScale * 1.2f : _startScale;
        }
    }
}
