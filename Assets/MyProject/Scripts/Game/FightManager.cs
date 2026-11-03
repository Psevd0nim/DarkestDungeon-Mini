using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyProject
{
    public class FightManager : MonoBehaviour
    {
        public enum BattlePhase
        {
            BeforeFight,
            PrepareRound,
            RoundLoop,
            EndFight,
            Canceled,
            EndRound
        }

        public BattlePhase CurrentPhase { get; private set; }
        public Character ActiveCharacter { get; private set; }

        private CharacterTurnManager _characterTurnManager;
        private CharactersManager _charactersManager;
        private SkillsManager _skillsManager;
        private Game_UI_Manager _gameUIManager;
        private CharacterSequence_UI _characterSequence_UI;

        private List<Character> _characterQueue = new List<Character>();
        private List<Character> _characterQueueHistory = new List<Character>();
        private List<Character> _chatactersDidTurn = new List<Character>();

        public void Init(CharacterTurnManager characterTurnManager, CharactersManager charactersManager, Game_UI_Manager gameUIManager, SkillsManager skillsManager)
        {
            _characterTurnManager = characterTurnManager;
            _charactersManager = charactersManager;
            _gameUIManager = gameUIManager;
            _skillsManager = skillsManager;
            _characterSequence_UI = gameUIManager.CharacterSequence_UI;
            _charactersManager.OnCharacterDied += AfterCharacterDied;

        }

        private void AfterCharacterDied(Character character)
        {
            _characterQueue.Remove(character);
            _characterSequence_UI.Remove(character);
            CheckForEndFight();
        }

        private void CheckForEndFight()
        {

        }

        public void StartFight()
        {
            ChangePhase(BattlePhase.BeforeFight);
        }

        public void RestartFight()
        {
            ChangePhase(BattlePhase.Canceled);
            _charactersManager.FillSquadByFormation(_charactersManager.PlayerSquadController, _charactersManager.PlayerSquadController.InitialFormationData);
            _charactersManager.FillSquadByFormation(_charactersManager.EnemySquadController, _charactersManager.EnemySquadController.InitialFormationData);
            ChangePhase(BattlePhase.PrepareRound);
        }

        public void LeaveBattle()
        {
            ChangePhase(BattlePhase.Canceled);
        }

        private void ChangePhase(BattlePhase battlePhase)
        {
            CurrentPhase = battlePhase;
            EnterCurrentPhase();
        }

        private void EnterCurrentPhase()
        {
            switch (CurrentPhase)
            {
                case BattlePhase.BeforeFight:
                    BeforeFight();
                    break;
                case BattlePhase.PrepareRound:
                    PrepareRound();
                    break;
                case BattlePhase.RoundLoop:
                    RoundLoop();
                    break;
                case BattlePhase.EndRound:
                    _characterSequence_UI.Clear();
                    ChangePhase(BattlePhase.PrepareRound);
                    break;
                case BattlePhase.EndFight:
                    break;
                case BattlePhase.Canceled:
                    Canceled();
                    break;
            }
        }

        private void BeforeFight()
        {
            _charactersManager.FillEnemySquad();
            ChangePhase(BattlePhase.PrepareRound);
        }

        private void PrepareRound()
        {
            List<Character> aliveCharacters = _charactersManager.GetAliveCharacters();
            ShuffleList(aliveCharacters);
            _characterQueue = aliveCharacters.OrderByDescending(character => character.Speed).ToList();
            _characterQueueHistory = _characterQueue.ToList();
            _characterSequence_UI.ShowCharacterSequence(_characterQueueHistory.Where(x => x.IsAlive).ToList());
            ChangePhase(BattlePhase.RoundLoop);
        }

        private void RoundLoop()
        {
            ActiveCharacter = _characterQueue.First();
            _characterQueue.Remove(ActiveCharacter);
            ActiveCharacter.SetCharacterActive(true);

            _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, true);
            _characterTurnManager.StartCharacterTurn(ActiveCharacter, AfterCharacterMadeTurn);
        }

        private void AfterCharacterMadeTurn()
        {
            ActiveCharacter.SetCharacterActive(false);
            _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, false);
            ActiveCharacter = null;

            if (_characterQueue.Count == 0)
            {
                ChangePhase(BattlePhase.EndRound);
            }
            else
            {
                ChangePhase(BattlePhase.RoundLoop);
            }
        }

        private void Canceled()
        {
            if (ActiveCharacter != null)
            {
                ActiveCharacter.SetCharacterActive(false);
                _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, false);
                ActiveCharacter = null;
            }

            _characterQueue.Clear();
            _characterQueueHistory.Clear();
            _characterSequence_UI.Clear();

            _charactersManager.ClearEnemySquad();

            _characterTurnManager.CancelCharacterTurn();

        }

        private void ShuffleList(List<Character> characters)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                int rndIndex = Random.Range(0, characters.Count);
                Character tempCharacter = characters[i];
                characters[i] = characters[rndIndex];
                characters[rndIndex] = tempCharacter;
            }
        }
    }
}
