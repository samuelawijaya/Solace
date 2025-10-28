using UnityEngine;

public class FootstepSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] grassSteps;
    [SerializeField] private AudioClip[] concreteSteps;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundRadius;
    [SerializeField] private float stepDistance = 0.3f; // Distance traveled before next step sound

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);

        // Check if player moved enough to trigger a footstep
        if (distance > stepDistance)
        {
            PlayFootstep();
            lastPosition = transform.position;
        }
    }

    private void PlayFootstep()
    {
        // Detect ground type with a raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRadius, whatIsGround);

        if (hit.collider != null)
        {

            // Choose sound based on tag
            if (hit.collider.CompareTag("Grass"))
            {
                SoundManager.instance.PlaySoundClip(grassSteps[Random.Range(0, grassSteps.Length)], transform, 0.4f);
            }
            else if (hit.collider.CompareTag("Concrete"))
            {
                SoundManager.instance.PlaySoundClip(concreteSteps[Random.Range(0, concreteSteps.Length)], transform, 0.4f);
            }
        }
    }
}
