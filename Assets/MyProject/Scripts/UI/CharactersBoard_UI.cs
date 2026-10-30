using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharactersBoard_UI : MonoBehaviour
    {
        private List<Character_UI> _targetCharacters_UI = new List<Character_UI>();

        public void ShowCharacterSequence(List<Character> characters, Factory factory)
        {
            foreach (var character in characters)
            {
                Character_UI character_UI = factory.CreateCharacter_UI();
                character_UI.transform.SetParent(transform);
                character_UI.Init(character);
                _targetCharacters_UI.Add(character_UI);
            }
        }

        public void SetCharacter_UI_Active(Character currentCharacter, bool isActive)
        {
            Character_UI targerCard_UI = _targetCharacters_UI.Find(x => x.Character == currentCharacter);
            if(targerCard_UI != null)
                targerCard_UI.SetCardActvie(isActive);
        }

        public void Clear()
        {
            foreach(var character_UI in _targetCharacters_UI)
            {
                Destroy(character_UI.gameObject);
            }
            _targetCharacters_UI.Clear();
        }
    }
}
