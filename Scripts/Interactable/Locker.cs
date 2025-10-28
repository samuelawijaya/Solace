using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;

    [SerializeField] private int healthPacks;
    [SerializeField] private int ammo;

    [SerializeField] private AudioClip lockerOpenSound;

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
        if (!isLockerOpened && isPlayerInRange)
        {
            SoundManager.instance.PlaySoundClip(lockerOpenSound, transform, 1f);
            //Give player items
            playerInventoryManager.AddHealthPack(healthPacks);
            playerInventoryManager.AddAmmo(ammo);


            isLockerOpened = true;
            Opened.SetActive(true);
            Normal.SetActive(false);
            Selected.SetActive(false);
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
