using Animation;
using UnityEngine;

namespace Entities
{
    public class EntityRenderer : MonoBehaviour,IEntityComponent
    {
        private SpriteRenderer _spriteRenderer;
        public Animator Animator {get; set;}
        
        private Entity _entity;
       
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
        }

    
        public void SetParam(AnimParamSO animParam) => Animator.SetTrigger(animParam.hashValue);
        public void SetParam(AnimParamSO animParam,int value) => Animator.SetInteger(animParam.hashValue,value);
        public void SetParam(AnimParamSO animParam,float value) => Animator.SetFloat(animParam.hashValue,value);
        public void SetParam(AnimParamSO animParam,bool value) => Animator.SetBool(animParam.hashValue,value);
    }
}