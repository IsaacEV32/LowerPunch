using UnityEngine;

public class Enemy : MonoBehaviour
{
    internal float healthEnemy = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(healthEnemy);
    }
}
