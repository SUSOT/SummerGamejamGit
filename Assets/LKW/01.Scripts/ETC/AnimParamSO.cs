using System;
using UnityEngine;

namespace Animation
{
    [CreateAssetMenu(fileName = "AnimParam", menuName = "SO/FSM/AnimParamSO", order = 0)]
    public class AnimParamSO : ScriptableObject
    {
        public string animName;
        public int hashValue;
        public string description;

        private void OnValidate()
        {
            hashValue = Animator.StringToHash(animName);
        }
    }
}