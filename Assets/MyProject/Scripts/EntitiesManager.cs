using System;
using UnityEngine;

namespace MyProject
{
    public class EntitiesManager : MonoBehaviour
    {
        public SquadController PlayerSquadManager => _playerSquadController;
        public SquadController EnemySquadManager => _enemySquadController;

        [SerializeField] private SquadController _playerSquadController;
        [SerializeField] private SquadController _enemySquadController;

        private Factory _factory;

        public void Init(Factory factory)
        {
            _factory = factory;
            _playerSquadController.Init(_factory);
            _enemySquadController.Init(_factory);
        }

        public void FillSquads()
        {
            _playerSquadController.FillSquadByRandom();
            _enemySquadController.FillSquadByRandom();
        }
    }
}
