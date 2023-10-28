using SupraRealities.SupraSpace.Utilities.ObjectPoolPattern;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    private List<PooledObject> enemies;

    public List<PooledObject> Enemies
    {
        private get
        {
            return enemies;
        }
        set
        {
            enemies = value;
            for (int i = 0; i < enemies.Count; i++)
            {
                int enemyId = enemies[i].GetInstanceID();
                if (!enemyIdToPool.ContainsKey(enemyId))
                {
                    enemyIdToPool.Add(enemies[i].GetInstanceID(), new ObjectPool(enemies[i]));
                }
            }
        }
    }
 
    [SerializeField] private float spawnRate = 2f;

    private Dictionary<int, ObjectPool> enemyIdToPool = new Dictionary<int, ObjectPool>();

    private void Start()
    {
        InvokeRepeating("Spawn", spawnRate, spawnRate);
    }

    private void Spawn()
    {
        if (Enemies.Count > 0)
        {
            int randomEnemyId = Enemies[Random.Range(0, Enemies.Count)].GetInstanceID();
            ObjectPool randomPool = enemyIdToPool[randomEnemyId];
            PooledObject enemyInstance = randomPool.GetObject();
            Vector3 randomPosition = new Vector3(transform.position.x + Random.Range(-40, 40), transform.position.y + Random.Range(-5, 45), transform.position.z + Random.Range(-30, 30));
            enemyInstance.transform.SetPositionAndRotation(randomPosition, transform.rotation);
        }
    }
}
