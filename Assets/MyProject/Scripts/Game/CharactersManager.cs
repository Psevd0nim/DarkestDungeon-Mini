using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharactersManager : MonoBehaviour
    {
        public event Action<Character> OnCharacterPressed;
        public event Action<Character> OnCharacterDied;

        public SquadController PlayerSquadController => _playerSquadController;
        public SquadController EnemySquadController => _enemySquadController;

        [SerializeField] private CharacterHubConfig _characterHubConfig;
        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;
        [SerializeField] private bool _fillPlayerSquadByRandom;
        [SerializeField] private SquadConfig _playerSquadConfig;
        [SerializeField] private bool _fillEnemySquadByRandom;
        [SerializeField] private SquadConfig _enemySquadConfig;

        private SkillsManager _skillsManager;

        public void Init(Factory factory, Game_UI_Manager gameUIManager, SkillsManager skillsManager)
        {
            _skillsManager = skillsManager;
            _playerSquadController.Init(this, _playerSquadConfig.squadColor);
            _enemySquadController.Init(this, _enemySquadConfig.squadColor, reversed: true);
        }

        public void FillPlayerSquad()
        {
            CharacterConfig[] _currentFormationData = ChooseFormation(_playerSquadConfig, _fillPlayerSquadByRandom);
            _playerSquadController.FillFormation(_currentFormationData);
        }

        public void FillEnemySquad()
        {
            CharacterConfig[] _currentFormationData = ChooseFormation(_enemySquadConfig, _fillEnemySquadByRandom);
            _enemySquadController.FillFormation(_currentFormationData);
        }

        public void FillSquadByFormation(SquadController squadController, CharacterConfig[] formation)
        {
            squadController.FillFormation(formation);
        }

        public CharacterConfig GetRandomConfig()
        {
            return _characterHubConfig.GetRandomCharacterConfig();
        }

        private CharacterConfig[] ChooseFormation(SquadConfig config, bool random)
        {
            if (!random && config != null)
            {
                return config.GetSquadFormation();
            }

            var result = new CharacterConfig[4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetRandomConfig();
            }

            return result;
        }

        

        

        

        public Character GetCharacterByConfig(CharacterConfig config)
        {
            var character = Instantiate(config.characterPrefab);
            character.OnPressed += AfterCharacterPressed;
            character.OnDeath += AfterCharacterDied;
            character.Init(new CharacterData(config.characterData), _skillsManager.GetSkillListBySkillConfigs(config.skillConfigList, character));
            character.SetIdentity(config);
            return character;
        }

        public List<Character> GetAliveCharacters()
        {
            var result = _playerSquadController.GetAliveCharacters();
            result.AddRange(_enemySquadController.GetAliveCharacters());
            return result;
        }

        public bool IsPlayer(Character c)
        {
            return c.OwnSquad == _playerSquadController;
        }

        public SquadController GetOpponent(Character c)
        {
            return IsPlayer(c) ? _enemySquadController : _playerSquadController;
        }

        public void ForEachCharacter(Action<Character> action)
        {
            foreach (var c in _playerSquadController.Characters)
            {
                action(c);
            }

            foreach (var c in _enemySquadController.Characters)
            {
                action(c);
            }
        }

        public void LeaveBattle()
        {
            _enemySquadController.Clear();
        }

        private void AfterCharacterPressed(Character c)
        {
            OnCharacterPressed?.Invoke(c);
        }

        private void AfterCharacterDied(Character character)
        {
            OnCharacterDied?.Invoke(character);
        }
    }
}
