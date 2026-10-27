using UnityEngine;

namespace MyProject
{
    public class GameLevelManager : LevelManager
    {
        [SerializeField] private FightManager _fightManager;
        [SerializeField] private EntitiesManager _entitiesManager;
        [SerializeField] private Game_UI_Manager _gameUIManager;
        [SerializeField] private Factory _factory;

        private AppServices _appServices;

        public override void Init(AppServices appServices)
        {
            _appServices = appServices;

            _fightManager.Init(_entitiesManager);

            _entitiesManager.Init(_factory);
        }

        public override void StartLevel()
        {
            _entitiesManager.FillSquads();
            _fightManager.StartFight();
        }
    }
}
