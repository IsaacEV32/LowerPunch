using System.Collections;
using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] internal GameObject objective;
    [SerializeField] internal EnemyStats stats;

    //Remove this when you have implemented the Change of States of the enemy
    private float chronometerPunch;
    private float delayPunch = 2;

    private float chronometerSleep;

    private void OnEnable()
    {
        InitStats(stats);
    }
    protected override void Chase()
    {
        enemy.SetDestination(objective.transform.position);
        if (enemy.remainingDistance <= 1)
        {
            actualState = States.Attack;
        }
    }
    protected override void Attack()
    {
        if (Physics.Raycast(transform.position, transform.forward))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<MainCharacter>(out MainCharacter Mc))
                {
                    Mc.ReceiveDamage(stats.damageEnemy);
                }
            }
            actualState = States.Sleep;
            Debug.Log("Golpe Enemigo izquierda");
        }
        else if (Physics.Raycast(transform.position, -transform.forward))
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<MainCharacter>(out MainCharacter Mc))
                {
                    Mc.ReceiveDamage(stats.damageEnemy);
                }
            }
            actualState = States.Sleep;
            Debug.Log("Golpe Enemigo derecha");
        }
        if (enemy.remainingDistance > 1)
        {
            actualState = States.Chase;
            chronometerPunch = 0;
        }
    }
    protected override void Sleep()
    {
        enemy.SetDestination(-objective.transform.position);
        if (recoverEnemy < chronometerSleep)
        {
            actualState = States.Chase;
            chronometerSleep = 0;
        }
    }
    protected override void Dead()
    {
        actualState = States.Dead;
        this.gameObject.SetActive(false);
        SpawnPointsEnemy.instance.DeactivateSpawnPoint();
    }
    private void Update()
    {
        switch (actualState)
        {
            case States.Chase:
                Chase();
                break;
            case States.Attack:
                chronometerPunch += Time.deltaTime;
                if (chronometerPunch > delayPunch)
                {
                    Attack();
                    chronometerPunch = 0;
                }
                break;
            case States.Sleep:
                chronometerSleep += Time.deltaTime;
                Sleep();
                break;
        }
    }
}