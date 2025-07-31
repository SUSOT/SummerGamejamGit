using System;
using System.Collections.Generic;
using System.Linq;
using Entities;
using SkillSystem;
using UnityEngine;

namespace Code.SkillSystem
{
    public class SkillCompo : MonoBehaviour, IEntityComponent
    {
        public Skill activeSkill; 
        public ContactFilter2D whatIsEnemy;


        private Entity _entity;

        private Dictionary<Type, Skill> _skills;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            
            _skills = new Dictionary<Type, Skill>();
            GetComponentsInChildren<Skill>().ToList().ForEach(skill => _skills.Add(skill.GetType(), skill));
            _skills.Values.ToList().ForEach(skill => skill.InitializeSkill(_entity, this));
        }

        public T GetSkill<T>() where T : Skill
        {
            Type type = typeof(T);
            return _skills.GetValueOrDefault(type) as T;
        }
    }
}