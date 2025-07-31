using System.Collections;
using UnityEngine;

namespace LCM._01.Scripts.Timeline
{
    public class Pattern1 : TimeLinePattern
    {
        public Pattern1(float startTime) : base(startTime)
        {
            
        }

        public override void Execute()
        {
            //StartCoroutine(PatternCoroutine());
        }

        // private IEnumerator PatternCoroutine()
        // {
        //     
        // }
    }
}
