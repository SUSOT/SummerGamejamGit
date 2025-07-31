using System.Collections;
using UnityEngine;

namespace LKW._01.Scripts.TimeLine
{
    public class TestPattern : TimeLinePattern
    {
        [SerializeField] private GameObject[] objects;  // 움직일 오브젝트 10개
        [SerializeField] private float moveDistance = 0.5f; // 위아래 움직이는 거리
        [SerializeField] private float waveSpeed = 2f;  // 물결 속도
        [SerializeField] private int repeatCount = 3;   // 반복 횟수
        
        private Vector3[] originalPositions; // 원래 위치 저장

        public override void Execute()
        {
            originalPositions = new Vector3[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null)
                    originalPositions[i] = objects[i].transform.position;
            }

            StartCoroutine(Wave());
        }
        
        private IEnumerator Wave()
        {
            for (int repeat = 0; repeat < repeatCount; repeat++)
            {
                float elapsed = 0f;
                while (elapsed < Mathf.PI * 2f) // 사인 한 주기
                {
                    elapsed += Time.deltaTime * waveSpeed;

                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (objects[i] == null) continue;

                        // 각 오브젝트별 시간 오프셋 → 물결 효과
                        float offset = i * 0.3f;
                        float yOffset = Mathf.Sin(elapsed + offset) * moveDistance;

                        Vector3 pos = originalPositions[i];
                        pos.y += yOffset;
                        objects[i].transform.position = pos;
                    }

                    yield return null;
                }
            }

            // 움직임 끝난 후 원래 위치 복귀
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] != null)
                    objects[i].transform.position = originalPositions[i];
            }
        }
    }
}