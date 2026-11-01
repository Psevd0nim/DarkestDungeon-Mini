using System;
using UnityEngine;

namespace MyProject
{
    public class SquadSlot : MonoBehaviour
    {
        public Character CharacterInSlot { get; private set; }

        [SerializeField] private SpriteRenderer _view;
        [SerializeField] private Color _targetColor;

        public void SetCharacter(Character character)
        {
            CharacterInSlot = character;
        }

        public void SetTargetActive(bool active)
        {
            _view.color = active? _targetColor : Color.white;
        }
    }
}
