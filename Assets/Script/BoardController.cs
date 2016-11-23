using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour {

    public Grid grid;

    public List<Enemy> enemies = new List<Enemy>();

    private static BoardController instance;
    public static BoardController Instance { get { return instance; } }

    float nextTargetSpawnTime;

    void Awake()
    {
        instance = this;

        grid = GetComponent<Grid>();

        GameObject character = (GameObject)Instantiate(Resources.Load("prefabs/core/Character"));
        GameObject enemy = (GameObject)Instantiate(Resources.Load("prefabs/core/Enemy"));
        enemies.Add(enemy.GetComponent<Enemy>());

    }

    void Update()
    {
        if (Time.time > nextTargetSpawnTime)
        {
            nextTargetSpawnTime = Time.time + Random.Range(2f, 3f);
            GameObject target = (GameObject)Instantiate(Resources.Load("prefabs/core/Target"));
            target.transform.position = grid.grid[Random.Range(0, 10), Random.Range(0, 10)].worldPosition;
        }
    }

    void OnDrawGizmos(){
    }

}
