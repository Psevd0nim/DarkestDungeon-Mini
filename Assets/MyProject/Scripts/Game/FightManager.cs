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
        public bool IsPaused { get; private set; }
        public bool IsGameOver { get; private set; }

        private CharacterTurnManager _characterTurnManager;
        private CharactersManager _charactersManager;
        private SkillsManager _skillsManager;
        private Game_UI_Manager _gameUIManager;
        private CharacterSequence_UI _characterSequence_UI;
        private List<Character> _characterQueue = new List<Character>();
        private List<Character> _characterQueueHistory = new List<Character>();
        private List<CharacterConfig> _initialTurnOrder = new List<CharacterConfig>();
        private bool _wasRestarted;

        public void Init(CharacterTurnManager characterTurnManager, CharactersManager charactersManager, Game_UI_Manager gameUIManager, SkillsManager skillsManager)
        {
            _characterTurnManager = characterTurnManager;
            _charactersManager = charactersManager;
            _gameUIManager = gameUIManager;
            _skillsManager = skillsManager;
            _characterSequence_UI = gameUIManager.CharacterSequence_UI;
            _charactersManager.OnCharacterDied += AfterCharacterDied;
        }

        public void StartFight()
        {
            ChangePhase(BattlePhase.BeforeFight);
        }

        public void RestartFight()
        {
            IsGameOver = false;
            ChangePhase(BattlePhase.Canceled);
            _charactersManager.FillSquadByFormation(_charactersManager.PlayerSquadController, _charactersManager.PlayerSquadController.InitialFormationData);
            _charactersManager.FillSquadByFormation(_charactersManager.EnemySquadController, _charactersManager.EnemySquadController.InitialFormationData);
            _wasRestarted = true;
            ChangePhase(BattlePhase.PrepareRound);
        }

        public void LeaveBattle()
        {
            ChangePhase(BattlePhase.Canceled);
        }

        public void TogglePause()
        {
            if (IsGameOver)
            {
                return;
            }

            SetPaused(!IsPaused);
        }

        public void Resume()
        {
            SetPaused(false);
        }

        private void SetPaused(bool paused)
        {
            IsPaused = paused;
            Time.timeScale = paused ? 0f : 1f;
            _gameUIManager.SetPaused(paused);
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
                    EndFight();
                    break;
                case BattlePhase.Canceled:
                    Canceled();
                    break;
            }
        }

        private void EndFight()
        {
            IsGameOver = true;
            Canceled();
            _gameUIManager.ShowGameOver(_charactersManager.PlayerSquadController.GetAliveCharacters().Count > 0);
        }

        private void BeforeFight()
        {
            _charactersManager.FillEnemySquad();
            ChangePhase(BattlePhase.PrepareRound);
        }

        private void PrepareRound()
        {
            List<Character> aliveCharacters = _charactersManager.GetAliveCharacters();
            if (_wasRestarted)
            {
                _characterQueue.Clear();
                foreach (CharacterConfig config in _initialTurnOrder)
                {
                    Character character = aliveCharacters.Find(candidate => candidate.Config == config);
                    _characterQueue.Add(character);
                    aliveCharacters.Remove(character);
                }

                _wasRestarted = false;
            }
            else
            {
                ShuffleList(aliveCharacters);
                _characterQueue = aliveCharacters.OrderByDescending(character => character.Speed).ToList();
                if (_initialTurnOrder.Count == 0)
                {
                    _initialTurnOrder = _characterQueue.Select(character => character.Config).ToList();
                }
            }
            _characterQueueHistory = _characterQueue.ToList();
            _characterSequence_UI.ShowCharacterSequence(_characterQueueHistory.Where(x => x.IsAlive).ToList());
            ChangePhase(BattlePhase.RoundLoop);
        }

        private void RoundLoop()
        {
            ActiveCharacter = _characterQueue.First();
            _characterQueue.Remove(ActiveCharacter);
            ActiveCharacter.SetCharacterActive(true);
            _gameUIManager.ShowCharacter(ActiveCharacter);

            _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, true);
            _characterTurnManager.StartCharacterTurn(ActiveCharacter, AfterCharacterMadeTurn);
        }

        private void AfterCharacterMadeTurn()
        {
            ActiveCharacter.SetCharacterActive(false);
            _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, false);
            _characterSequence_UI.SetCharacterTurnCompleted(ActiveCharacter, true);
            ActiveCharacter = null;

            if(CheckForEndFight())
            {
                ChangePhase(BattlePhase.EndFight);
                return;
            }

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
            _characterTurnManager.CancelCharacterTurn();
            _charactersManager.CancelAnimations();

            if (ActiveCharacter != null)
            {
                ActiveCharacter.SetCharacterActive(false);
                _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, false);
                ActiveCharacter = null;
            }

            _characterQueue.Clear();
            _characterQueueHistory.Clear();
            _characterSequence_UI.Clear();
            _gameUIManager.ClearCharacterInfo();

            _charactersManager.ClearEnemySquad();

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

        private void AfterCharacterDied(Character character)
        {
            _characterQueue.Remove(character);
            _characterSequence_UI.Remove(character);
        }

        private bool CheckForEndFight()
        {
            if(_charactersManager.PlayerSquadController.GetAliveCharacters().Count == 0 
                || _charactersManager.EnemySquadController.GetAliveCharacters().Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
