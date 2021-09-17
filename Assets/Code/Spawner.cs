using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0)
        {
           // Instantiate(enemyPrefab, transform);
            Instantiate(enemyPrefab, new Vector3(Random.Range(-5,5),-8), transform.rotation, transform);

        }
    }
}
