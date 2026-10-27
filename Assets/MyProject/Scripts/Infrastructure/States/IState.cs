namespace MyProject
{
    public interface IState
    {
        void Exit();
    }

    public interface IDefaultState : IState
    {
        void Enter();
    }

    public interface IPayloadedState<TPayload> : IState
    {
        void Enter(TPayload payload);
    }
}
