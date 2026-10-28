using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class EntitiesManager : MonoBehaviour
    {
        public event Action<Entity> OnCharacterPressed;

        public EntityHubConfig hubConfig;

        public SquadController PlayerSquadManager => _playerSquadController;
        public SquadController EnemySquadManager => _enemySquadController;

        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;
        [SerializeField] private SquadSettingsConfig _squadSettingsConfig;

        private Factory _factory;
        private List<Entity> _allCharacters = new List<Entity>();

        public void Init(Factory factory, Game_UI_Manager game_UI_Manager)
        {
            _factory = factory;
            _playerSquadController.Init(this, _squadSettingsConfig.GetRandomColor());
            _enemySquadController.Init(this, _squadSettingsConfig.GetRandomColorWithException(_playerSquadController.SquadColor.colorType), needReverse: true);
        }

        public void FillSquads()
        {
            _playerSquadController.FillSquadByRandom();
            _enemySquadController.FillSquadByRandom();

            _allCharacters.AddRange(_playerSquadController.Characters);
            _allCharacters.AddRange(_enemySquadController.Characters);

            foreach (Entity entity in _allCharacters)
            {
                entity.OnPressed += AfterCharacterPressed;
            }
        }

        public Entity GetRandomCharacter()
        {
            EntityConfig config = hubConfig.GetRandomCharacterConfig();
            Entity entity = _factory.CreateEntityByType(config.entityType);
            entity.Init(config.entityData);
            return entity;
        }

        public List<Entity> GetAliveCharacters()
        {
            return _allCharacters.FindAll(character => character.IsAlive);
        }

        private void AfterCharacterPressed(Entity entity)
        {

            //OnCharacterPressed?.Invoke(entity);
        }

        
    }
}
