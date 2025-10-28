using UnityEngine;

public class LockedLockerCat : MonoBehaviour
{
    


    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;
    [SerializeField] private GameObject Cat;


    [SerializeField] private AudioClip lockerOpenSound;
    [SerializeField] private AudioClip lockedSound;
    [SerializeField] private AudioClip meowSound;
    //private bool isLocked = true;

    private bool isPlayerInRange = false;
    private bool isLockerOpened = false;
    private GameObject Selected;
    private GameObject Normal;
    private GameObject Opened;

    private void Awake()
    {
        Selected = transform.Find("Selected").gameObject;
        Normal = transform.Find("Locker").gameObject;
        Opened = transform.Find("Opened").gameObject;
        Selected.SetActive(false);
        Normal.SetActive(true);
    }

    private void Start()
    {
        gameInput.OnInteract += GameInput_OnInteractLocker;
    }

    private void GameInput_OnInteractLocker(object sender, System.EventArgs e)
    {
        Interact();
    }

    public void Interact()
    {
        //LOCKER IS LOCKED

        if (!isLockerOpened && isPlayerInRange)
        {
            if (playerInventoryManager.GetKey())
            {
                SoundManager.instance.PlaySoundClip(lockerOpenSound, transform, 1f);
                //Make Cat Appear
                Cat.SetActive(true);


                isLockerOpened = true;
                Opened.SetActive(true);
                Normal.SetActive(false);
                Selected.SetActive(false);
                playerInventoryManager.UseKey();
                SoundManager.instance.PlaySoundClip(meowSound, transform, 0.5f);
            }
            else
            {
                SoundManager.instance.PlaySoundClip(lockedSound, transform, 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLockerOpened)
        {
            isPlayerInRange = true;
            Selected.SetActive(true);
            Normal.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isLockerOpened)
        {
            isPlayerInRange = false;
            Selected.SetActive(false);
            Normal.SetActive(true);
        }
    }
}
