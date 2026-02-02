using UnityEngine;

public class NormalEnemy : Enemy
{
    [SerializeField] internal GameObject objective;
    [SerializeField] internal EnemyStats stats;

    //Remove this when you have implemented the Change of States of the enemy
    private bool chronometer;
    private float chronometerPunch;
    private float delayPunch = 2;


    bool change = false; //Remove this when you have implemented the Change of States of the enemy
    private void OnEnable()
    {
        InitStats(stats);
    }
    protected override void Chase()
    {
        enemy.SetDestination(objective.transform.position);
        if (enemy.remainingDistance <= 1)
        {
            change = true;
        }
    }
    protected override void Attack()
    {
        if (Physics.Raycast(transform.position, transform.forward) && !chronometer)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(-1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<MainCharacter>(out MainCharacter Mc))
                {
                    Mc.ReceiveDamage(5);
                    chronometer = true;
                }
            }
            Debug.Log("Special izquierda");
        }
        else if (Physics.Raycast(transform.position, -transform.forward) && !chronometer)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(1, 0, 0), new Vector3(1f, 1.25f, 1f), Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<MainCharacter>(out MainCharacter Mc))
                {
                    Mc.ReceiveDamage(5);
                    chronometer = true;
                }
            }
            Debug.Log("Special derecha");
        }
        if (enemy.remainingDistance > 1)
        {
            change = false;
        }
    }
    private void Update()
    {
        Chase();
        if (change)
        {
            Attack();
            if (chronometer)
            {
                chronometerPunch += Time.deltaTime;
                if (chronometerPunch > delayPunch)
                {
                    chronometer = false;
                    chronometerPunch = 0;
                }
            }
        }
    }
}