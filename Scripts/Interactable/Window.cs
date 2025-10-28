using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour
{
    [SerializeField] AudioClip windowBreakSound;
    [SerializeField] Animator animator;

    [SerializeField] private float disableDelay = 0.35f;

    private void Start()
    {
        
    }

    public void BreakWindow()
    {
        animator.Play("WindowBreak");
        SoundManager.instance.PlaySoundClip(windowBreakSound, transform, 0.5f);
        StartCoroutine(DisableObjectAfterTime());
    }




    private IEnumerator DisableObjectAfterTime()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }
}
