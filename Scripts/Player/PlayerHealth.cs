using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    private int health;


    private void Start()
    {
        health = maxHealth;

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Player health = " + health);

        if(health <= 0)
        {
            Debug.LogError("Player has died");
        }
    }
}
