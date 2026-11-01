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
            Stopped,
            PrepareRound,
            NextCharacter,
            WaitingForAction,
            FinishTurn
        }

        public BattlePhase CurrentPhase { get; private set; }
        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }
        public Character ActiveCharacter { get; private set; }
        public bool AutoPlay { get; set; }

        private CharactersManager _charactersManager;
        private SkillsManager _skillsManager;
        private Game_UI_Manager _gameUIManager;
        private CharacterSequence_UI _characterSequence_UI;
        private List<Character> _characterQueue = new List<Character>();
        private List<Character> _roundCharacters = new List<Character>();
        private HashSet<Character> _charactersWithCompletedTurns = new HashSet<Character>();
        private int _battleRunId;
        private bool _phaseEntryPending;

        public void Init(CharactersManager charactersManager, Game_UI_Manager gameUIManager, SkillsManager skillsManager)
        {
            _charactersManager = charactersManager;
            _gameUIManager = gameUIManager;
            _skillsManager = skillsManager;
            _characterSequence_UI = gameUIManager.CharacterSequence_UI;
            _charactersManager.OnCharacterDied += AfterCharacterDied;
        }

        public void StartFight()
        {
            IsRunning = true;
            ChangePhase(HasBattleEnded() ? BattlePhase.Stopped : BattlePhase.PrepareRound);
        }

        public void RestartFight()
        {
            ChangePhase(BattlePhase.Stopped);
            //_charactersManager.RestartSquads();
            StartFight();
        }

        private void ChangePhase(BattlePhase battlePhase)
        {
            CurrentPhase = battlePhase;
            _phaseEntryPending = true;
            EnterCurrentPhase();
        }

        private void EnterCurrentPhase()
        {
            if (CurrentPhase != BattlePhase.Stopped && (!IsRunning || IsPaused))
            {
                return;
            }

            _phaseEntryPending = false;
            switch (CurrentPhase)
            {
                case BattlePhase.PrepareRound:
                    PrepareRound(() => ChangePhase(BattlePhase.NextCharacter));
                    break;
                case BattlePhase.NextCharacter:
                    if (TryTakeNextCharacter(out Character character))
                    {
                        ActiveCharacter = character;
                        ChangePhase(BattlePhase.WaitingForAction);
                    }
                    else
                    {
                        ChangePhase(HasBattleEnded() ? BattlePhase.Stopped : BattlePhase.PrepareRound);
                    }

                    break;
                case BattlePhase.WaitingForAction:
                    BeginTurn(() => ChangePhase(BattlePhase.FinishTurn));
                    break;
                case BattlePhase.FinishTurn:
                    EndTurn();
                    ChangePhase(HasBattleEnded() ? BattlePhase.Stopped : BattlePhase.NextCharacter);
                    break;
                case BattlePhase.Stopped:
                    EndBattle();
                    break;
            }
        }

        private void PrepareRound(Action callback)
        {
            _roundCharacters = _charactersManager.GetAliveCharacters().OrderByDescending(character => character.Speed).ToList();
            _characterQueue = new List<Character>(_roundCharacters);
            _charactersWithCompletedTurns.Clear();
            RefreshCharacterSequence();
            callback?.Invoke();
        }

        private bool TryTakeNextCharacter(out Character character)
        {
            if (_characterQueue.Count == 0)
            {
                character = null;
                return false;
            }

            character = _characterQueue[0];
            _characterQueue.RemoveAt(0);
            return true;
        }

        private void BeginTurn(Action completed)
        {
            ActiveCharacter.SetCharacterActive(true);
            _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, true);
            _gameUIManager.ShowCharacter(ActiveCharacter);
            int battleRunId = _battleRunId;
            Action finish = () =>
            {
                if (_battleRunId == battleRunId && IsRunning)
                {
                    completed();
                }
            };

            if (_charactersManager.IsPlayer(ActiveCharacter) && !AutoPlay)
            {
                _skillsManager.BeginPlayerTurn(ActiveCharacter, finish);
            }
            else
            {
                _skillsManager.Begin_AI_Turn(ActiveCharacter, finish);
            }
        }

        private void EndTurn()
        {
            if (ActiveCharacter != null)
            {
                ActiveCharacter.SetCharacterActive(false);
                _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, false);
                if (ActiveCharacter.IsAlive)
                {
                    _charactersWithCompletedTurns.Add(ActiveCharacter);
                    _characterSequence_UI.SetCharacterTurnCompleted(ActiveCharacter, true);
                }
            }

            ActiveCharacter = null;
        }

        private void AfterCharacterDied(Character character)
        {
            _characterQueue.Remove(character);
            _roundCharacters.Remove(character);
            _charactersWithCompletedTurns.Remove(character);
            RefreshCharacterSequence();
        }

        private void RefreshCharacterSequence()
        {
            _characterSequence_UI.Clear();
            _characterSequence_UI.ShowCharacterSequence(_roundCharacters);
            foreach (var character in _charactersWithCompletedTurns)
            {
                _characterSequence_UI.SetCharacterTurnCompleted(character, true);
            }

            if (ActiveCharacter != null && ActiveCharacter.IsAlive)
            {
                _characterSequence_UI.SetCharacter_UI_Active(ActiveCharacter, true);
            }
        }

        private bool HasBattleEnded()
        {
            bool playerAlive = _charactersManager.PlayerSquadController.GetAliveCharacters().Count > 0;
            bool enemyAlive = _charactersManager.EnemySquadController.GetAliveCharacters().Count > 0;
            return !playerAlive || !enemyAlive;
        }

        private void EndBattle()
        {
            _battleRunId++;
            IsRunning = false;
            _skillsManager.CancelTurn();
            SetPaused(false);
            _charactersManager.ForEachCharacter(character =>
            {
                character.SetCharacterActive(false);
                character.SetTargetActive(false);
                character.Visual.ResetPose(character.IsAlive);
            });
            ActiveCharacter = null;
            _characterQueue.Clear();
            _roundCharacters.Clear();
            _charactersWithCompletedTurns.Clear();
            _characterSequence_UI.Clear();
        }

        public void TogglePause()
        {
            SetPaused(!IsPaused);
        }

        public void Resume()
        {
            SetPaused(false);
        }

        private void SetPaused(bool paused)
        {
            IsPaused = paused;
            _skillsManager.IsPaused = paused;
            _charactersManager.ForEachCharacter(character => character.Visual.SetPaused(paused));
            Time.timeScale = paused ? 0 : 1;
            _gameUIManager.SetPaused(paused);
            if (!paused && _phaseEntryPending)
            {
                EnterCurrentPhase();
            }
        }

        public void LeaveBattle()
        {
            ChangePhase(BattlePhase.Stopped);
            _charactersManager.LeaveBattle();
        }

        private void OnDestroy()
        {
            if (_charactersManager != null)
            {
                _charactersManager.OnCharacterDied -= AfterCharacterDied;
            }

            Time.timeScale = 1;
        }
    }




















    public class FightManagerNew : MonoBehaviour
    {
        public enum BattlePhase
        {
            PrepareRound,
            DuringRound,
            EndRound,
            Canceled
        }

        public BattlePhase CurrentPhase { get; private set; }
        public Character ActiveCharacter { get; private set; }

        private CharactersManager _charactersManager;
        private SkillsManager _skillsManager;
        private Game_UI_Manager _gameUIManager;
        private CharacterSequence_UI _characterSequence_UI;

        private List<Character> _characterQueue = new List<Character>();
        private List<Character> _characterQueueHistory = new List<Character>();
        private List<Character> _chatactersDidTurn = new List<Character>();

        public void Init(CharactersManager charactersManager, Game_UI_Manager gameUIManager, SkillsManager skillsManager)
        {
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
        }

        public void StartFight()
        {
            _charactersManager.FillEnemySquad();
            ChangePhase(BattlePhase.PrepareRound);
        }

        public void RestartFight()
        {
            ChangePhase(BattlePhase.Canceled);
            _charactersManager.FillSquadByFormation(_charactersManager.PlayerSquadController, _charactersManager.PlayerSquadController.InitialFormationData);
            _charactersManager.FillSquadByFormation(_charactersManager.EnemySquadController, _charactersManager.EnemySquadController.InitialFormationData);
            ChangePhase(BattlePhase.PrepareRound);
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
                case BattlePhase.PrepareRound:
                    PrepareRound();
                    break;
                case BattlePhase.DuringRound:

                    break;
            }
        }

        private void PrepareRound()
        {
            List<Character> aliveCharacters = _charactersManager.GetAliveCharacters();
            ShuffleList(aliveCharacters);
            _characterQueue = aliveCharacters.OrderByDescending(character => character.Speed).ToList();
            _characterQueueHistory = _characterQueue.ToList();
            ChangePhase(BattlePhase.DuringRound);
        }

        private void RefreshCharacterSequence()
        {
            _characterSequence_UI.ShowCharacterSequence(_characterQueueHistory.Where(x => x.IsAlive).ToList());
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
