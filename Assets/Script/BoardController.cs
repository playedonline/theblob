using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        GameObject character = Spawn("prefabs/core/Character", 10, 10);

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
            nextTargetSpawnTime = Time.time + Random.Range(3f, 10f);
            GameObject target = Spawn("prefabs/core/Target", Random.Range(0, grid.nodes.GetLength(0)), Random.Range(0, grid.nodes.GetLength(1)));
            targets.Add(target.GetComponent<Target>());
         }

        if (Time.time > nextEnemySpawnTime)
        {
            nextEnemySpawnTime = Time.time + Random.Range(3f, 10f);
            GameObject enemy = Spawn("prefabs/core/Enemy", Random.Range(0, grid.nodes.GetLength(0)), Random.Range(0, grid.nodes.GetLength(1)));
            enemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    void OnDrawGizmos(){
    }

    public GameObject Spawn(string type, int x, int y){

        var position = grid.nodes[x, y].worldPosition;
        var spawn = (GameObject)Instantiate(Resources.Load(type));
        spawn.transform.position = position;
        spawn.transform.localScale = Vector3.zero;

        var hole = (GameObject)Instantiate(Resources.Load("prefabs/core/Hole"));
        hole.transform.position = position;
        hole.transform.localScale = new Vector3(1, 0, 1);

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.Append(hole.transform.DOScaleY(1, 0.25f));
        var openDuration = sequence.Duration();
        sequence.AppendInterval(0.2f);
        sequence.Append(spawn.transform.DOScale(1, 0.7f).SetEase(Ease.OutBack));
        sequence.Insert(openDuration + 0.2f, hole.transform.DOScaleY(0, 0.25f));
        sequence.AppendCallback(() => {
            try{
                spawn.BroadcastMessage("Init");
            } catch(System.Exception){}
        });

        return spawn;
    }

}
