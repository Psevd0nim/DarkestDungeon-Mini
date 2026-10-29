using UnityEngine;

namespace MyProject
{
    public class GameLevelManager : LevelManager
    {
        [SerializeField] private FightManager _fightManager;
        [SerializeField] private CharactersManager _charactersManager;
        [SerializeField] private Game_UI_Manager _gameUIManager;
        [SerializeField] private Factory _factory;
        [SerializeField] private SkillsManager _skillsManager;

        private AppServices _appServices;

        public override void Init(AppServices appServices)
        {
            _appServices = appServices;

            _fightManager.Init(_charactersManager, _gameUIManager, _skillsManager);

            _charactersManager.Init(_factory, _gameUIManager, _skillsManager);

            _gameUIManager.Init(_factory);

            _skillsManager.Init(_gameUIManager, _charactersManager);
        }

        public override void StartLevel()
        {
            _charactersManager.FillSquads();
            _fightManager.StartFight();
        }
    }
}
