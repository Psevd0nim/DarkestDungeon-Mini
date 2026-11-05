using System;
using UnityEngine;

namespace MyProject
{
    public class CharacterTurnManager : MonoBehaviour
    {
        private Action _onTurnEnded;
        private SkillsManager _skillsManager;
        private CharactersManager _charactersManager;
        private Character _activeCharacter;

        public void Init(SkillsManager skillsManager, CharactersManager charactersManager)
        {
            _skillsManager = skillsManager;
            _charactersManager = charactersManager;
        }

        public void StartCharacterTurn(Character character, Action onTurnEnded)
        {
            _activeCharacter = character;
            _onTurnEnded = onTurnEnded;
            _skillsManager.OnSkillUsed += EndCharacterTurn;
            bool isPlayerControlled = _charactersManager.IsPlayer(character);
            _skillsManager.ShowSkills(character.SkillList, character, isPlayerControlled);
        }

        public void EndCharacterTurn()
        {
            _skillsManager.OnSkillUsed -= EndCharacterTurn;
            _activeCharacter = null;
            Action onTurnEnded = _onTurnEnded;
            _onTurnEnded = null;
            onTurnEnded?.Invoke();
        }

        public void CancelCharacterTurn()
        {
            _skillsManager.OnSkillUsed -= EndCharacterTurn;
            _activeCharacter = null;
            _onTurnEnded = null;
            _skillsManager.CancelProcess();
        }
    }
}
