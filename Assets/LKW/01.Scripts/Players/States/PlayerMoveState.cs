using Animation;
using Code.Players;
using Entities;
using UnityEngine;

namespace Players
{
    public class PlayerMoveState : EntityState
    {
        private Player _player;
        private EntityMover _mover;
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.Movement.OnValueChanged += HandleDirectionChanged;
        }
        

        public override void Update()
        {
            base.Update();
            _mover.SetMovement(_player.inputReader.MoveDirection);
            _mover.SetRotation(_player.inputReader.MoveDirection);
            if (_mover.Movement.Value == Vector2.zero)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.Movement.OnValueChanged -= HandleDirectionChanged;
            base.Exit();
        }
        
        private void HandleDirectionChanged(Vector2 prev, Vector2 next)
        {
            if (_mover.Movement.Value == Vector2.zero) return;
            _renderer.Animator.Play("Move",-1,0f);
        }
    }
}