using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool _isPlaying;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isPlaying) return;

            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        _isPlaying = true;
        animator.SetTrigger("DoorOpen");
        yield return new WaitForSeconds(2);
        _isPlaying = false;
    }
}
