using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    public Grid grid;

    public List<Enemy> enemies = new List<Enemy>();
    public List<Target> targets = new List<Target>();

    private static BoardController instance;
    public static BoardController Instance { get { return instance; } }

    float nextTargetSpawnTime;
    float nextEnemySpawnTime;

    public Character character;

    void Awake()
    {
        instance = this;

        grid = GetComponent<Grid>();

        character = ((GameObject)Instantiate(Resources.Load("prefabs/core/Character"))).GetComponent<Character>();
    }

    public void Restart()
    {
        StartCoroutine(DelayedRestart());
    }

    private IEnumerator DelayedRestart()
    {
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (Time.time > nextTargetSpawnTime)
        {
            nextTargetSpawnTime = Time.time + Random.Range(1f, 3f);
            GameObject target = (GameObject)Instantiate(Resources.Load("prefabs/core/Target"));
            target.transform.position = grid.grid[Random.Range(0, 25), Random.Range(0, 25)].worldPosition;
            targets.Add(target.GetComponent<Target>());
        }

        if (Time.time > nextEnemySpawnTime)
        {
            nextEnemySpawnTime = Time.time + Random.Range(3f, 6f);
            GameObject enemy = (GameObject)Instantiate(Resources.Load("prefabs/core/Enemy"));
            enemy.transform.position = grid.grid[Random.Range(0, 25), Random.Range(0, 25)].worldPosition;
            enemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    void OnDrawGizmos(){
    }

}
