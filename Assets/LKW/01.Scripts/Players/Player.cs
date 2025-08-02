using System;
using System.Collections;
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

        private int maxHealth = 5;
        
        [Inject] public PoolManagerMono poolManager;

        public AnimParamSO MOVE_XParam;
        public AnimParamSO MOVE_YParam;
        
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] public ParticleSystem trailParticle;
        [SerializeField] public ParticleSystem dashParticle;
        [SerializeField] public PoolingItemSO dashCirce;
        
        public UnityEvent gameOverEvent;
        [field:SerializeField] public InputReaderSO inputReader{get; private set;}
        
        [SerializeField] private StateListSO stateList;
        
        private StateMachine _stateMachine;

        [field: SerializeField] public int Health { get; private set; } = 5;

        private Material _material;
        
        private bool _isDead;
        
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

        private void OnEnable()
        {
            Injector.Instance.InjectRuntime(this);
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            inputReader.OnDashKeyPressed += HandleDashKeyPress;
        }

        private void OnDestroy()
        {
            inputReader.OnDashKeyPressed -= HandleDashKeyPress;
            StopAllCoroutines();
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();

            if (Input.GetKeyDown(KeyCode.Home))
            {
                gameObject.layer = LayerMask.NameToLayer("IgnoreBody");
            }
            else if (Input.GetKey(KeyCode.PageUp))
            {
                Time.timeScale = 5;
            }
            else if (Input.GetKey(KeyCode.PageDown))
            {
                Time.timeScale = 1;
            }
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

            if (Health <= 0 && !_isDead)
            {
                _isDead = true;
                
                gameOverEvent?.Invoke();
                playerChannel.RaiseEvent(PlayerEvents.PlayerHitEvent);
            }

            StartCoroutine(IgnoreCoroutine());
        }

        private IEnumerator IgnoreCoroutine()
        {
            gameObject.layer = LayerMask.NameToLayer("IgnoreBody");
            yield return new WaitForSeconds(0.6f);
            gameObject.layer = 0;
        }
    }
}