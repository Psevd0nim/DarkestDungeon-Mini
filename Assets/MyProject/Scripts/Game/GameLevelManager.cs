using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyProject
{
    public class GameLevelManager : LevelManager
    {
        [SerializeField] private FightManager _fightManager;
        [SerializeField] private CharacterTurnManager _characterTurnManager;
        [SerializeField] private CharactersManager _charactersManager;
        [SerializeField] private Game_UI_Manager _gameUIManager;
        [SerializeField] private Factory _factory;
        [SerializeField] private SkillsManager _skillsManager;

        public override void Init(AppServices services)
        {
            _fightManager.Init(_characterTurnManager, _charactersManager, _gameUIManager, _skillsManager);
            _characterTurnManager.Init(_skillsManager);
            _gameUIManager.Init(_factory, _fightManager);
            _skillsManager.Init(_gameUIManager, _charactersManager);
            _charactersManager.Init(_factory, _gameUIManager, _skillsManager);
        }

        public override void StartLevel()
        {
            _charactersManager.FillPlayerSquad();
            _fightManager.StartFight();
        }
    }
}
