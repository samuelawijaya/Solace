using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Cat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] Animator animator;

    [Header("Settings")]
    [SerializeField] float followSpeed = 3f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float maxFollowY = 2f;


    [SerializeField] AudioClip[] meowSounds;
    private bool isMoving = false;
    private Coroutine idleCoroutine;

    private void Start()
    {
        StartCoroutine(RandomMeowRoutine());
    }

    void Update()
    {
        if (!player) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (player.position.y - transform.position.y > maxFollowY)
        {
            Stop();
            return;
        }
        
        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Stop();
        }
    }

    void MoveTowardsPlayer()
    {
        if (!isMoving)
        {
            isMoving = true;
            animator.SetBool("isWalking", true);
            StopIdleCoroutine();
        }

        Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
    }

    void Stop()
    {
        if (isMoving)
        {
            isMoving = false;
            animator.SetBool("isWalking", false);
            StartIdleCoroutine();
        }
    }

    void StartIdleCoroutine()
    {
        StopIdleCoroutine();
        idleCoroutine = StartCoroutine(IdleRoutine());
    }

    void StopIdleCoroutine()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    IEnumerator IdleRoutine()
    {
        // 1. Wait a few seconds before doing any idle animation
        float waitBeforeIdle = Random.Range(3f, 4f);
        yield return new WaitForSeconds(waitBeforeIdle);

        // 2. Play a random idle animation
        int idleIndex = Random.Range(0, 3);
        animator.SetInteger("idleIndex", idleIndex);
        animator.SetTrigger("playIdle");

        // 3. Let animation play for random duration
        float idlePlayTime = Random.Range(4f, 5f);
        yield return new WaitForSeconds(idlePlayTime);

        // 4. Sit in a default idle pose
        animator.SetTrigger("sitIdle");
    }

    IEnumerator RandomMeowRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(5f, 15f); // Wait 5–15 seconds between meows
            yield return new WaitForSeconds(waitTime);

            if (meowSounds.Length > 0)
            {
                SoundManager.instance.PlayRandomSoundClip(meowSounds, transform, 0.4f);
            }
        }
    }
}
