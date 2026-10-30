using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyProject
{
    public class FightManager : MonoBehaviour
    {
        private CharactersManager _charactersManager;
        private Game_UI_Manager _game_UI_Manager;
        private SkillsManager _skillsManager;

        private bool _fightIsEnded;
        private List<Character> _queueList = new List<Character>();
        private int _currentCharacterIndex;

        public void Init(CharactersManager charactersManager, Game_UI_Manager game_UI_Manager, SkillsManager skillsManager)
        {
            _charactersManager = charactersManager;
            _game_UI_Manager = game_UI_Manager;
            _skillsManager = skillsManager;
        }

        public void StartFight()
        {
            StartCoroutine(FightCoroutine());
        }

        private IEnumerator FightCoroutine()
        {
            while (!_fightIsEnded)
            {
                PrepareRound();
                while(_currentCharacterIndex < _queueList.Count)
                {
                    Character currentCharacter = _queueList[_currentCharacterIndex];
                    yield return CharacterFight(currentCharacter);
                    _currentCharacterIndex++;
                }
                AfterRound();
            }        
        }

        private void PrepareRound()
        {
            List<Character> aliveCharacters = _charactersManager.GetAliveCharacters();
            _queueList = aliveCharacters.OrderByDescending(x => x.Speed).ToList();
            _game_UI_Manager.ShowCharacterSequence(_queueList);
        }

        private void AfterRound()
        {
            _currentCharacterIndex = 0;
            _game_UI_Manager.ClearCharacterSequence();
        }

        private IEnumerator CharacterFight(Character currentCharacter)
        {
            currentCharacter.SetCharacterActive(true);
            _game_UI_Manager.SetCharacter_UI_Active(currentCharacter, true);

            bool characterTurnCompleted = false;
            if (_charactersManager.PlayerSquadController.Characters.Contains(currentCharacter))
            {
                _skillsManager.BeginPlayerTurn(currentCharacter, () => characterTurnCompleted = true);
                yield return new WaitUntil(() => characterTurnCompleted);
            }
            else
            {
                _skillsManager.Begin_AI_Turn(currentCharacter, () => characterTurnCompleted = true);
                yield return new WaitUntil(() => characterTurnCompleted);
            }

            currentCharacter.SetCharacterActive(false);
            _game_UI_Manager.SetCharacter_UI_Active(currentCharacter, false);
        }
    }
}
