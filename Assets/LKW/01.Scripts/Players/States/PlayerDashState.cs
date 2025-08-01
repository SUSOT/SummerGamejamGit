using Animation;
using DG.Tweening;
using Entities;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using UnityEngine;

namespace Players
{
    public class PlayerDashState : EntityState
    {
        
        private Player _player;
        private EntityMover _mover;

        private readonly float _dashDistance = 10f, _dashTime = 0.25f;
        
        public PlayerDashState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            Vector2 playerInput = _player.inputReader.MoveDirection;
            Vector2 dashDirection = playerInput.normalized;

            if (playerInput == Vector2.zero)
            {
                dashDirection = _player.transform.up;
            }
            
            _player.transform.up = dashDirection;
            _mover.CanManualMove = true;
            _mover.StopImmediately();
            
            Vector3 destination = _player.transform.position + (Vector3)dashDirection * _dashDistance;
            float dashTime = _dashTime;

            DashCircle circle = _player.poolManager.Pop<DashCircle>(_player.dashCirce);
            circle.transform.position = _player.dashParticle.transform.position;
            
            _player.dashParticle.Play();

            RaycastHit2D hit =  Physics2D.Raycast(_player.transform.position, dashDirection, _dashDistance, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                float distance = hit.distance;
                destination = _player.transform.position + (Vector3)dashDirection  * distance;
                ;
                dashTime =  distance* _dashTime / _dashDistance;
            }
            else
            {
                destination = _player.transform.position + (Vector3)dashDirection  * _dashDistance;
                ;
                dashTime =  _dashDistance * _dashTime / _dashDistance;
            }
            _player.gameObject.layer = LayerMask.NameToLayer("IgnoreBody");
            _player.transform.DOMove(destination, dashTime).SetEase(Ease.OutQuad).OnComplete(EndDash).OnComplete(() =>
            {
                _player.gameObject.layer = 11;
                _player.ChangeState("IDLE");
            });

        }

        private void EndDash()
        {
            _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.StopImmediately();
            _mover.CanManualMove = false;
            base.Exit();
        }
    }
}
