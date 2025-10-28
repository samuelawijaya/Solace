using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] private int loadedAmmo = 7;
    [SerializeField] private int spareAmmo = 8;
    [SerializeField] private int healthPack = 1;
    [SerializeField] private bool hasKey;
    [SerializeField] private Image KeyIcon;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthPacks;
    [SerializeField] private Text loadedAmmoUI;
    [SerializeField] private Text spareAmmoUI;

    [SerializeField] private float reloadTime = 1f;
    private bool isReloading = false;



    [SerializeField] AudioClip reloadSound;

    private void Start()
    {
        hasKey = false;
        SetHealthPackAmountUI();
        SetLoadedAmmoUI();
        SetSpareAmmoUI();
        SetKeyUI();
    }

    private void Update()
    {
        
    }

    public void SetHealthBar(int health)
    {
        healthBar.fillAmount = health / 10f;
    }

    public void SetHealthPackAmountUI()
    {
        healthPacks.text = healthPack.ToString();
    }

    public int GetHealthPackAmount()
    {
        return healthPack;
    }
    public void AddHealthPack(int healthPickup)
    {
        healthPack += healthPickup;
        SetHealthPackAmountUI();
    }

    public void AddAmmo(int ammoPickup)
    {
        spareAmmo += ammoPickup;
        SetSpareAmmoUI();
    }

    public void shootAmmo()
    {
        if(loadedAmmo > 0)
        {
            loadedAmmo -= 1;
            SetLoadedAmmoUI();
        }
        
    }

    public void PickupKey()
    {
        hasKey = true;
        SetKeyUI();
    }

    public void UseKey()
    {
        hasKey = false;
        SetKeyUI();
    }

    public bool GetKey()
    {
        return hasKey;
    }

    public void SetKeyUI()
    {
        if (hasKey)
        {
            KeyIcon.enabled = true;
        }
        else
        {
            KeyIcon.enabled = false;
        }
    }

    public void ReloadAmmo()
    {
        //if(loadedAmmo < 7 && spareAmmo > 0)
        //{
        //    int ammoLoaded = 7 - loadedAmmo;

        //    int ammoToLoad = Mathf.Min(spareAmmo, ammoLoaded);

        //    loadedAmmo += ammoToLoad;
        //    spareAmmo -= ammoToLoad;

        //    SetLoadedAmmoUI();
        //    SetSpareAmmoUI();
        //}

        if (loadedAmmo < 7 && spareAmmo > 0 && !isReloading)
        {
            StartCoroutine(ReloadCoroutine());
            SoundManager.instance.PlaySoundClip(reloadSound, transform, 1f);
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        // Wait for reload duration
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = 7 - loadedAmmo;
        int ammoToLoad = Mathf.Min(spareAmmo, ammoNeeded);

        loadedAmmo += ammoToLoad;
        spareAmmo -= ammoToLoad;

        SetLoadedAmmoUI();
        SetSpareAmmoUI();

        isReloading = false;
    }


    public int GetLoadedAmmo()
    {
        return loadedAmmo;
    }

    public void SetLoadedAmmoUI()
    {
        loadedAmmoUI.text = loadedAmmo.ToString();
    }

    public void SetSpareAmmoUI()
    {
        spareAmmoUI.text = spareAmmo.ToString();
    }


}
