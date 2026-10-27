using UnityEngine.SceneManagement;

namespace MyProject
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private GameStateMachine _stateMachine;

        public LoadLevelState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter(string sceneName)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                _stateMachine.Enter<StartGameState>();
                return;
            }

            SceneManager.sceneLoaded += AfterSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= AfterSceneLoaded;
        }

        private void AfterSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _stateMachine.Enter<StartGameState>();
        }
    }
}
