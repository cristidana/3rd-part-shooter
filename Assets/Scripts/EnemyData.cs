using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data", order = 51)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth;
    public float detectionRange;
    public float attackRange;
    public float patrolSpeed;
    public float chaseSpeed;
    public float maxApproachDistance;
    public float yRotationOffset;  
}
