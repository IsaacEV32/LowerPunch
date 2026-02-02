using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.XR;

public enum States
{
    Chase, Attack, Sleep, Dead
}

public class Enemy : MonoBehaviour
{
    internal float healthEnemy;
    internal float damageEnemy;
    internal float recoverEnemy;
    protected NavMeshAgent enemy;
    internal bool lookLeft = false;
    private void Awake()
    {
        enemy = this.GetComponent<NavMeshAgent>();
    }
    public virtual void InitStats(EnemyStats eS)
    {
        healthEnemy = eS.healthEnemy;
        damageEnemy = eS.damageEnemy;
        recoverEnemy = eS.recoverEnemy;
    }
    public virtual void ReceiveDamage(int damagePoints)
    {
        if (healthEnemy > 0)
        {
            healthEnemy -= damagePoints;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    protected virtual void Chase() { }
    protected virtual void Attack() { }
    protected virtual void Sleep() { }
    protected virtual void Dead() { }
    protected virtual void ChangeState() { }
}
