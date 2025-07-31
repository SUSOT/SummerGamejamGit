using System;
using Code.SkillSystem;
using Code.SkillSystem.Dash;
using Entities;
using Settings.InputSetting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Players
{
    public class Player : Entity
    {
        [SerializeField] private LayerMask projectileLayer;
        [SerializeField] private ParticleSystem deadParticles;
        public TrailRenderer trailRenderer;
        
        public UnityEvent gameOverEvent;
        [field:SerializeField] public InputReaderSO inputReader{get; private set;}
        
        [SerializeField] private StateListSO stateList;
        
        private StateMachine _stateMachine;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new StateMachine(this, stateList);
            trailRenderer = GetComponentInChildren<TrailRenderer>();
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

        
    }
}