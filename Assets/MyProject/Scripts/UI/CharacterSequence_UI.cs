using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharacterSequence_UI : MonoBehaviour
    {
        private List<Character_UI> _charactersInSequence = new List<Character_UI>();
        private Factory _factory;

        public void Init(Factory factory)
        {
            _factory = factory;
        }

        public void ShowCharacterSequence(List<Character> characters)
        {
            foreach (var character in characters)
            {
                Character_UI character_UI = _factory.CreateCharacter_UI();
                character_UI.transform.SetParent(transform, false);
                character_UI.Init(character);
                _charactersInSequence.Add(character_UI);
            }
        }

        public void SetCharacter_UI_Active(Character currentCharacter, bool isActive)
        {
            Character_UI targerCard_UI = _charactersInSequence.Find(x => x.Character == currentCharacter);
            targerCard_UI.SetCardActive(isActive);
        }

        public void SetCharacterTurnCompleted(Character character, bool completed)
        {
            Character_UI character_UI = _charactersInSequence.Find(card => card.Character == character);
            if (character_UI != null)
            {
                character_UI.SetTurnCompleted(completed);
            }
        }

        public void Remove(Character character)
        {
            Character_UI character_UI = _charactersInSequence.Find(card => card.Character == character);
            _charactersInSequence.Remove(character_UI);
            Destroy(character_UI.gameObject);
        }

        public void Clear()
        {
            foreach (var character_UI in _charactersInSequence)
            {
                character_UI.gameObject.SetActive(false);
                Destroy(character_UI.gameObject);
            }

            _charactersInSequence.Clear();
        }
    }
}
