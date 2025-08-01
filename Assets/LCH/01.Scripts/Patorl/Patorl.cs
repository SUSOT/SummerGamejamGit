using DG.Tweening;
using EasyTransition;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patorl : MonoBehaviour
{

    [SerializeField] private string nextScene;
    [SerializeField] private Vector2 overlapSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private void Start()
    {
        StartCoroutine(CheckPlayerOverlap());
    }

    private IEnumerator CheckPlayerOverlap()
    {
        bool wasOverlapping = false;

        while (true)
        {
            bool isOverlapping = Physics2D.OverlapBox(transform.position, overlapSize, 0, whatIsPlayer);

            if (isOverlapping && !wasOverlapping)
            {
                transform.DOScale(4f, 1.2f);
            }
            else if (!isOverlapping && wasOverlapping)
            {
                transform.DOScale(2f, 1.2f);
            }

            wasOverlapping = isOverlapping;
            yield return new WaitForSeconds(0.3f); 
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("¤·?");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¤·?");
            transform.DOScale(0, 0.3f).OnComplete(()=>SceneManager.LoadScene(nextScene));
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, overlapSize);
    }
#endif
}
