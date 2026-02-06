using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] internal float healthEnemy;
    [SerializeField] internal int damageEnemy;
    [SerializeField] internal float recoverEnemy;
}
