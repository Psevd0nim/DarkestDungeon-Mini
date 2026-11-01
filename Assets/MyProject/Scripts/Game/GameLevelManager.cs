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
        [SerializeField] private SkillExecutionManager _skillExecutionManager;

        public override void Init(AppServices services)
        {
            _fightManager.Init(_charactersManager, _gameUIManager, _skillsManager);
            _gameUIManager.Init(_factory, _fightManager);
            _skillsManager.Init(_gameUIManager, _charactersManager, _skillExecutionManager);
            _charactersManager.Init(_factory, _gameUIManager, _skillsManager);
        }

        public override void StartLevel()
        {
            _charactersManager.FillPlayerSquad();
            _fightManager.StartFight();
        }
    }
}
