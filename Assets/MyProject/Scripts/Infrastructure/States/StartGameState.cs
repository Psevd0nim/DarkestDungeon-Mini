using System;
using UnityEngine;

namespace MyProject
{
    public class StartGameState : IDefaultState
    {
        private GameStateMachine _stateMachine;
        private LevelManager _levelManager;

        public StartGameState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _levelManager = UnityEngine.Object.FindAnyObjectByType<LevelManager>();
            if (_levelManager == null)
                throw new Exception("The current scene must contain a LevelManager.");

            _levelManager.Init(_stateMachine.AppServices);
            _levelManager.OnExitLevel += ChangeScene;
            _levelManager.StartLevel();
        }

        public void Exit()
        {
            _levelManager.OnExitLevel -= ChangeScene;
        }

        private void ChangeScene(string sceneName)
        {
            _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }

    }
}
