using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "StateList", menuName = "SO/FSM/StateList", order = 0)]
    public class StateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}