using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
  public float spawnRadiusmin;
  public float spawnRadiusmax;
  public float spawnRate;

  public GameObject enemy;

  private int enemyCount1 = 0;
  private int enemyCount2=0;
  public bool stopSpawning = false;
  public GameObject strongEnemy;
  private int totalSpawned = 0;


  // Start is called before the first frame update
  void Start()
  {

    InvokeRepeating("spawnEnemy", 1f, spawnRate); 
      


  }

  // Update is called once per frame
  void Update()
  {

  }

  public void spawnEnemy()
  {
    if (stopSpawning || totalSpawned>=500) return;
    float spawnRadius = Random.Range(spawnRadiusmin, spawnRadiusmax);
    Vector2 spawnDir = Random.insideUnitCircle.normalized * spawnRadius;
    Vector3 spawnPos = transform.position + new Vector3(spawnDir.x, 0, spawnDir.y);


    GameObject enemyToSpawn;

if (totalSpawned < 500)
{
    enemyToSpawn = enemy;
    enemyCount1++;
}
else
{
    enemyToSpawn = strongEnemy;
    enemyCount2++;
}

    Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

    totalSpawned++;

  }
  
  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, spawnRadiusmin); // Match radius


    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, spawnRadiusmax); // Match radius
        

        
    }
}
