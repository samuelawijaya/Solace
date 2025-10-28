using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private PlayerInventoryManager playerInventoryManager;


    [SerializeField] private AudioClip keyPickupSound;

    private bool isPlayerInRange = false;
    private GameObject Selected;
    private GameObject Normal;

    private void Awake()
    {
        Selected = transform.Find("Selected").gameObject;
        Normal = transform.Find("Key").gameObject;
        Selected.SetActive(false);
        Normal.SetActive(true);
    }

    private void Start()
    {
        gameInput.OnInteract += GameInput_OnPickupKey;
    }

    private void GameInput_OnPickupKey(object sender, System.EventArgs e)
    {
        Interact();
    }

    public void Interact()
    {
        if (isPlayerInRange)
        {
            SoundManager.instance.PlaySoundClip(keyPickupSound, transform, 1f);
            //Give player Key
            playerInventoryManager.PickupKey();



            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Selected.SetActive(true);
            Normal.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Selected.SetActive(false);
            Normal.SetActive(true);
        }
    }
}
