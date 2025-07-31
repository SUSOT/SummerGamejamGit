using Animation;
using Entities;
using Vector2 = UnityEngine.Vector2;

namespace Players
{
    public class PlayerIdleState : EntityState
    {
        EntityMover _mover;
        Player _player;
        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.transform.up = Vector2.up;
            _mover.StopImmediately();
            
        }

        public override void Update()
        {
            base.Update();

            Vector2 inputDirection = _player.inputReader.MoveDirection;
            if (inputDirection.magnitude > 0)
                _player.ChangeState("MOVE");
        }
    }
}