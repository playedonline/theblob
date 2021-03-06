using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour {

    public Grid grid;

    private static BoardController instance;
    public static BoardController Instance { get { return instance; } }

    float nextTargetSpawnTime;
    float nextEnemySpawnTime;
    float totalEnemiesSpawned = 0;

    public float minSpawnTarget = 2f;
    public float maxSpawnTarget = 5f;
    public float minSpawnEnemy = 2f;
    public float maxSpawnEnemy = 5f;


    public Character character;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        grid = GetComponent<Grid>();
        character = Spawn("prefabs/core/Character").GetComponent<Character>();
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
            nextTargetSpawnTime = Time.time + Random.Range(minSpawnTarget, maxSpawnTarget);
            GameObject target = Spawn("prefabs/core/Target");
        }

        if (Time.time > nextEnemySpawnTime)
        {
            nextEnemySpawnTime = Time.time + Random.Range(minSpawnEnemy, maxSpawnEnemy);
            GameObject enemy = Spawn("prefabs/core/Enemy");
            totalEnemiesSpawned++;
        }
    }

    Node FindUnspatteredArea(){
        Node node = grid.nodes[0, 0];
        for(int i = 0; i < 10000; i++){
            var x = UnityEngine.Random.Range(2, grid.nodes.GetLength(0) - 2);
            var y = UnityEngine.Random.Range(2, grid.nodes.GetLength(1) - 2);
            node = grid.nodes[x, y];
            if(!node.isDirty){
                return node;
            }
        }
        return node;
    }

    void OnDrawGizmos(){
    }

    public GameObject Spawn(string type){
        var node = FindUnspatteredArea();
        var position = node.worldPosition;
        var spawn = (GameObject)Instantiate(Resources.Load(type));
        spawn.transform.position = position;
        var previousScale = spawn.transform.localScale;
        spawn.transform.localScale = 0f * Vector3.one;

        var hole = (GameObject)Instantiate(Resources.Load("prefabs/core/Hole"));
        hole.transform.position = position;
        hole.transform.localScale = new Vector3(1, 0, 1);

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1);
        sequence.Append(hole.transform.DOScaleY(1, 0.25f));
        var openDuration = sequence.Duration();
        sequence.AppendInterval(0.2f);
        sequence.Append(spawn.transform.DOScale(previousScale, 0.7f).SetEase(Ease.OutBack));
        sequence.Insert(openDuration + 0.2f, hole.transform.DOScaleY(0, 0.25f));
        sequence.AppendCallback(() => {
            try{
                spawn.BroadcastMessage("Init");
            } catch(System.Exception){}
            Destroy(hole.gameObject);
        });

        return spawn;
    }
}
