using System;
using Code.SkillSystem;
using Entities;
using UnityEngine;

namespace SkillSystem
{
    public delegate void CooldownInfo(float current, float totalTime);
    
    public abstract class Skill : MonoBehaviour
    {
        public bool skillEnabled = false;
        
        [SerializeField] protected float cooldown;
        protected float _cooldownTimer;
        protected Entity _entity;
        protected SkillCompo _skillCompo;

        public bool IsCooldown => _cooldownTimer > 0f;
        public event CooldownInfo OnCooldown;

        public virtual void InitializeSkill(Entity entity, SkillCompo skillCompo)
        {
            _entity = entity;
            _skillCompo = skillCompo;
        }

        protected virtual void Update()
        {
            if (_cooldownTimer > 0)
            {
                _cooldownTimer -= Time.deltaTime;

                if (_cooldownTimer <= 0)
                    _cooldownTimer = 0;
                
                OnCooldown?.Invoke(_cooldownTimer, cooldown);
            }
        }

        public virtual bool AttemptUseSkill()
        {
            if (_cooldownTimer <= 0 && skillEnabled)
            {
                _cooldownTimer = cooldown;
                UseSkill();
                return true;
            }
            Debug.Log("Skill cooldown or locked");
            return false;
        }

        public virtual void UseSkill()
        {
        }
    }
}