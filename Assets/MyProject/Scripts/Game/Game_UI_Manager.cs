using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class Game_UI_Manager : MonoBehaviour
    {
        public CharactersBoard_UI CharactersBoard_UI => _charactersBoard_UI;
        public SkillsBoard_UI SkillsBoard_UI => _skillsBoard_UI;

        [SerializeField] private SkillsBoard_UI _skillsBoard_UI;
        [SerializeField] private CharactersBoard_UI _charactersBoard_UI;

        private Factory _factory;

        public void Init(Factory factory)
        {
            _factory = factory;
            _skillsBoard_UI.Init(factory);
        }

        public void ShowCharacterSequence(List<Character> characters)
        {
            _charactersBoard_UI.ShowCharacterSequence(characters, _factory);
        }

        public void ClearCharacterSequence()
        {
            _charactersBoard_UI.Clear();
        }

        public void SetCharacter_UI_Active(Character currentCharacter, bool isActive)
        {
            _charactersBoard_UI.SetCharacter_UI_Active(currentCharacter, isActive);
        }
    }
}
