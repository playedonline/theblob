using UnityEngine;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

    public Grid grid;

    public List<Enemy> enemies = new List<Enemy>();

    private static BoardController instance;
    public static BoardController Instance { get { return instance; } }

    void Awake()
    {
        instance = this;

        grid = GetComponent<Grid>();

        GameObject mask = (GameObject)Instantiate(Resources.Load("Mask"));
        GameObject enemy = (GameObject)Instantiate(Resources.Load("Enemy"));
        enemies.Add(enemy.GetComponent<Enemy>());
    }

    void OnDrawGizmos(){
    }

}
