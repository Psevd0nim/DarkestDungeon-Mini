namespace MyProject
{
    public class BootstrapState : IDefaultState
    {
        private GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _stateMachine.SetAppServices(new AppServices());
        }

        public void Exit() { }
    }
}
