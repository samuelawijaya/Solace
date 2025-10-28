using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = true;
    [SerializeField] PlayerInventoryManager playerInventoryManager;
    [SerializeField] GameObject door;
    [SerializeField] AudioClip doorSound;
    [SerializeField] AudioClip doorLockedSound;
    [SerializeField] bool isLocked = false;

    public void ToggleDoor()
    {
        if (isLocked)
        {
            if (playerInventoryManager.GetKey())
            {
                SoundManager.instance.PlaySoundClip(doorSound, transform, 1f);
                isOpen = !isOpen;
                door.SetActive(isOpen);
                playerInventoryManager.UseKey();
            }
            else
            {
                SoundManager.instance.PlaySoundClip(doorLockedSound, transform, 1f);
            }
        }
        else
        {
            SoundManager.instance.PlaySoundClip(doorSound, transform, 1f);
            isOpen = !isOpen;
            door.SetActive(isOpen);
        }
        


    }

}
