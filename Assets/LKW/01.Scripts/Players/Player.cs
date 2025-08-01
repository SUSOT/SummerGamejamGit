using System;
using Animation;
using Code.SkillSystem;
using Code.SkillSystem.Dash;
using Entities;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.Runtime;
using LCM._01.Scripts;
using LKW._01.Scripts.Core;
using Settings.InputSetting;
using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    public class Player : Entity, IDamageable
    {
        private int fillAmountHash = Shader.PropertyToID("_FillAmount");

        private int maxHealth = 3;
        
        [Inject] public PoolManagerMono poolManager;

        public AnimParamSO MOVE_XParam;
        public AnimParamSO MOVE_YParam;
        
        [SerializeField] private LayerMask projectileLayer;
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private ParticleSystem deadParticle;
        [SerializeField] public ParticleSystem trailParticle;
        [SerializeField] public ParticleSystem dashParticle;
        [SerializeField] public PoolingItemSO dashCirce;
        
        public UnityEvent gameOverEvent;
        [field:SerializeField] public InputReaderSO inputReader{get; private set;}
        
        [SerializeField] private StateListSO stateList;
        
        private StateMachine _stateMachine;

        [field: SerializeField] public int Health { get; private set; } = 3;

        private Material _material;
        
        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, stateList);
            _material = GetComponentInChildren<SpriteRenderer>().material;
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            inputReader.OnDashKeyPressed += HandleDashKeyPress;
        }

        private void OnDestroy()
        {
            inputReader.OnDashKeyPressed -= HandleDashKeyPress;
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        private void HandleDashKeyPress()
        {
            if(GetCompo<SkillCompo>().GetSkill<DashSkill>().AttemptUseSkill())
                ChangeState("DASH");
        }

        public void ChangeState(string stateName) => _stateMachine.ChangeState(stateName);


        public void TakeDamage()
        {
            Health--;
            Debug.Log("맞음");
            float fill = (float)Health / maxHealth;
            _material.SetFloat(fillAmountHash, fill);

            if (Health <= 0)
            {
                gameOverEvent?.Invoke();
                playerChannel.RaiseEvent(PlayerEvents.PlayerHitEvent);
                deadParticle.transform.position = transform.position;
                deadParticle.Play();
                gameObject.SetActive(false);
            }
        }
    }
}