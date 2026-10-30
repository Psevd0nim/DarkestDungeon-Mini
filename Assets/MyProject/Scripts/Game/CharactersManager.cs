using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharactersManager : MonoBehaviour
    {
        public event Action<Character> OnCharacterPressed;

        public SquadController PlayerSquadController => _playerSquadController;
        public SquadController EnemySquadController => _enemySquadController;

        [SerializeField] private CharacterHubConfig _characterHubConfig;
        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;
        [SerializeField] private SquadColorsConfig _squadSettingsConfig;
        [SerializeField] bool _fillPlayerSquadByRandom;
        [SerializeField] private SquadConfig _playerSquadConfig;
        [SerializeField] private bool _fillEnemySquadByRandom;
        [SerializeField] private SquadConfig _enemySquadConfig;

        private Factory _factory;
        private List<Character> _allCharacters = new List<Character>();
        private SkillsManager _skillsManager;

        public void Init(Factory factory, Game_UI_Manager game_UI_Manager, SkillsManager skillsManager)
        {
            _factory = factory;
            _skillsManager = skillsManager;
            _playerSquadController.Init(this, _squadSettingsConfig.GetRandomColor());
            _enemySquadController.Init(this, _squadSettingsConfig.GetRandomColorWithException(_playerSquadController.SquadColor.colorType), needReverse: true);
        }

        public void FillSquads()
        {
            if(_playerSquadConfig == null || _fillPlayerSquadByRandom)
                _playerSquadController.FillSquadByRandom();
            else
                _playerSquadController.FillSquadByFixedConfig(_playerSquadConfig);

            if (_enemySquadConfig == null || _fillEnemySquadByRandom)
                _enemySquadController.FillSquadByRandom();
            else
                _enemySquadController.FillSquadByFixedConfig(_enemySquadConfig);

            _allCharacters.AddRange(_playerSquadController.Characters);
            _allCharacters.AddRange(_enemySquadController.Characters);
        }

        public Character GetCharacterByConfig(CharacterConfig config)
        {
            return CreateCharacter(config);
        }

        public Character GetRandomCharacter()
        {
            CharacterConfig config = _characterHubConfig.GetRandomCharacterConfig();
            Character character = CreateCharacter(config);
            return character;
        }

        private Character CreateCharacter(CharacterConfig config)
        {
            // TODO: Remove the factory fallback once all configs have a prefab assigned.
            Character character = config.characterPrefab != null? Instantiate(config.characterPrefab) : _factory.CreateCharacterByType(config.characterType);
            character.OnPressed += AfterCharacterPressed;
            CharacterData characterData = new CharacterData(config.characterData);
            character.Init(characterData, _skillsManager.GetSkillListBySkillConfigs(config.skillConfigList, character));
            return character;
        }

        public List<Character> GetAliveCharacters()
        {
            return _allCharacters.FindAll(character => character.IsAlive);
        }

        private void AfterCharacterPressed(Character character)
        {
            OnCharacterPressed?.Invoke(character);
        }
    }
}
