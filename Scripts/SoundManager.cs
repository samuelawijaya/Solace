using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //Spawn game object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        //Assign audio clip
        audioSource.clip = audioClip;

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get length of sound clip
        float clipLength = audioSource.clip.length;

        //Destroy clip after done
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        //Assign a random index
        int rand = Random.Range(0, audioClip.Length);
        //Spawn game object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        //Assign audio clip
        audioSource.clip = audioClip[rand];

        //Assign volume
        audioSource.volume = volume;

        //Play sound
        audioSource.Play();

        //Get length of sound clip
        float clipLength = audioSource.clip.length;

        //Destroy clip after done
        Destroy(audioSource.gameObject, 1f);
    }
}
