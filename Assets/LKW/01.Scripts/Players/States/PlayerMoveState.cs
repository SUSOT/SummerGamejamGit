using Animation;
using Entities;
using UnityEngine;

namespace Players
{
    public class PlayerMoveState : EntityState
    {
        private Player _player;
        private EntityMover _mover;
        private EntityRenderer _renderer;
        
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _renderer = entity.GetCompo<EntityRenderer>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.Movement.OnValueChanged += HandleDirectionChanged;
            _player.trailParticle.Play();
        }
        

        public override void Update()
        {
            base.Update();
            Vector2 direction = _player.inputReader.MoveDirection;
            
            _mover.SetMovement(direction);
            _mover.SetRotation(direction);
            _renderer.SetParam(_player.MOVE_XParam, direction.x);
            _renderer.SetParam(_player.MOVE_YParam, direction.y);
            if (_mover.Movement.Value == Vector2.zero)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.Movement.OnValueChanged -= HandleDirectionChanged;
            _player.trailParticle.Stop();

            base.Exit();
        }
        
        private void HandleDirectionChanged(Vector2 prev, Vector2 next)
        {
            if (_mover.Movement.Value == Vector2.zero) return;
            _renderer.Animator.Play("Move",-1,0f);
        }
    }
}