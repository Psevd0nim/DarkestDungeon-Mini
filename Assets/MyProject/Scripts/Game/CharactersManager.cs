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

        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;
        [SerializeField] private CharacterHubConfig _playerCharacterHubConfig;
        [SerializeField] private CharacterHubConfig _enemyCharacterHubConfig;
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
            CharacterConfig[] _currentFormationData = ChooseFormation(_playerSquadConfig, _fillPlayerSquadByRandom, _playerCharacterHubConfig);
            _playerSquadController.FillFormation(_currentFormationData);
        }

        public void FillEnemySquad()
        {
            CharacterConfig[] _currentFormationData = ChooseFormation(_enemySquadConfig, _fillEnemySquadByRandom, _enemyCharacterHubConfig);
            _enemySquadController.FillFormation(_currentFormationData);
        }

        public void FillSquadByFormation(SquadController squadController, CharacterConfig[] formation)
        {
            squadController.FillFormation(formation);
        }

        private CharacterConfig[] ChooseFormation(SquadConfig config, bool random, CharacterHubConfig characterHubConfig)
        {
            if (!random && config != null)
            {
                return config.GetSquadFormation();
            }

            CharacterConfig[] result = new CharacterConfig[4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = characterHubConfig.GetRandomCharacterConfig();
            }

            return result;
        }

        public Character GetCharacterByConfig(CharacterConfig config)
        {
            Character character = Instantiate(config.characterPrefab);
            character.OnPressed += AfterCharacterPressed;
            character.OnDeath += AfterCharacterDied;
            character.Init(config, _skillsManager.GetSkillListBySkillConfigs(config.skillConfigList, character));
            return character;
        }

        public List<Character> GetAliveCharacters()
        {
            List<Character> result = _playerSquadController.GetAliveCharacters();
            result.AddRange(_enemySquadController.GetAliveCharacters());
            return result;
        }

        public bool IsPlayer(Character character)
        {
            return character.OwnSquad == _playerSquadController;
        }

        public SquadController GetOpponentSquad(Character character)
        {
            return IsPlayer(character) ? _enemySquadController : _playerSquadController;
        }

        public void CancelAnimations()
        {
            foreach (Character character in _playerSquadController.Characters)
            {
                character.CancelAnimation();
            }

            foreach (Character character in _enemySquadController.Characters)
            {
                character.CancelAnimation();
            }
        }

        public void ClearEnemySquad()
        {
            _enemySquadController.Clear();
        }

        private void AfterCharacterPressed(Character character)
        {
            OnCharacterPressed?.Invoke(character);
        }

        private void AfterCharacterDied(Character character)
        {
            OnCharacterDied?.Invoke(character);
        }
    }
}
