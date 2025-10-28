using UnityEngine;


[CreateAssetMenu(fileName = "EnemySO")]
public class EnemySO : ScriptableObject
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;

    public int damage;
    public float attackSpeed;
    public float attackWindup;
    public float attackCooldown;
    public float chaseDistance = 7f;
}
