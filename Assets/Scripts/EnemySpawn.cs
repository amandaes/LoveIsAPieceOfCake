using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour {

    public GameObject enemy;
    float randX; //random x position
    Vector2 whereToSpawn;
    public float spawnRate = 2f;
    float nextSpawn = 0f;




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-8.67f, 3.9f); //random range of x position
            whereToSpawn = new Vector2(randX, transform.position.y);
            Instantiate(enemy, whereToSpawn, Quaternion.identity);

            
        }

        //Destroy(gameObject, 3f);
    }
}
