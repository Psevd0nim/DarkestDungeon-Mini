using UnityEngine;

namespace MyProject
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private bool _startWithCurrentScene = true;

        private static GameBootstrapper _instance;
        private GameStateMachine _stateMachine;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            _stateMachine = new GameStateMachine();
            _stateMachine.Enter<BootstrapState>();
        }

        private void Start()
        {
            string sceneName = Constants.StartSceneName;
#if UNITY_EDITOR
            if (_startWithCurrentScene)
                sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
#endif
            _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }
    }
}
