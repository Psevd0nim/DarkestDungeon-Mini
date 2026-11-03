using System;
using UnityEngine;

namespace MyProject
{
    public class CharacterTurnManager : MonoBehaviour
    {
        private SkillsManager _skillsManager;
        private Character _activeCharacter;
        private Action _onTurnEnded;

        public void Init(SkillsManager skillsManager)
        {
            _skillsManager = skillsManager;
        }

        public void StartCharacterTurn(Character character, Action onTurnEnded)
        {
            _activeCharacter = character;
            _onTurnEnded = onTurnEnded;
            _skillsManager.ShowSkills(character.SkillList, character);
            _skillsManager.OnSkillUsed += EndCharacterTurn;
        }

        public void EndCharacterTurn()
        {
            _skillsManager.OnSkillUsed -= EndCharacterTurn;
            _activeCharacter = null;
            _onTurnEnded?.Invoke();
            //_onTurnEnded = null;
        }

        public void CancelCharacterTurn()
        {
            _activeCharacter = null;
            _onTurnEnded = null;
        }
    }
}
