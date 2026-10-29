using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharactersManager : MonoBehaviour
    {
        public event Action<Character> OnCharacterPressed;

        public CharacterHubConfig hubConfig;

        public SquadController PlayerSquadController => _playerSquadController;
        public SquadController EnemySquadController => _enemySquadController;

        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;
        [SerializeField] private SquadSettingsConfig _squadSettingsConfig;

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
            _playerSquadController.FillSquadByRandom();
            _enemySquadController.FillSquadByRandom();

            _allCharacters.AddRange(_playerSquadController.Characters);
            _allCharacters.AddRange(_enemySquadController.Characters);
        }

        public Character GetRandomCharacter()
        {
            CharacterConfig config = hubConfig.GetRandomCharacterConfig();
            Character character = _factory.CreateCharacterByType(config.characterType);
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
