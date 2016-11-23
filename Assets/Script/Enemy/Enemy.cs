using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    public Transform followTarget;
    public EnemyController enemyController;
    public float wanderRadius = 100f;
    public float waitTime = 1f;
    public float alertProximity = 200;

    public float acceleration;

    private Node wanderNode;
    private BoardController board;
    private int frameCount = 0;

    private float velocity;
    private bool isThink;

    void Start(){
        enemyController = GetComponent<EnemyController>();

        board = FindObjectOfType<BoardController>();
        wanderNode = board.grid.NodeFromWorldPoint(transform.position);
        transform.position = wanderNode.worldPosition;

        Think();
    }

    void Update(){
        if (BoardController.Instance.character.alive)
        {
            var currentNode = board.grid.NodeFromWorldPoint(transform.position);
            var difference = currentNode.worldPosition - transform.position;
            if ((frameCount % 100 == 0 || !isThink) && difference.magnitude < 2)
            {
                Think();
            }
            isThink = difference.magnitude < 2;
            frameCount++;

            CleanTile();
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(transform.position, alertProximity);
    }

    /*

        Private methods

     */

    private bool SearchEnemy(){
        var character = FindObjectOfType<Character>();
        var difference = character.transform.position - transform.position;
        if(difference.magnitude < alertProximity){
            followTarget = character.transform;
            return true;
        }
        return false;
    }

    private void Think(){
        // Find enemy
        if(followTarget == null){
            if(!SearchEnemy()){
                Wander();
            } else {
                Follow();
            }
        } else {
            Follow();
        }
    }

    private void Wander(){
        // Try to move to a position
        var currentNode = board.grid.NodeFromWorldPoint(transform.position);
        if(wanderNode != currentNode) {
            return;
        }
        // Wandering
        while(true){
            try{
                Vector3 position = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(board.grid.nodeRadius, wanderRadius);
                var wanderingNode = board.grid.NodeFromWorldPoint(new Vector3(position.x, position.y, 0));
                if(wanderingNode != currentNode){
                    wanderNode = board.grid.NodeFromWorldPoint(new Vector3(position.x, position.y, 0));
                    break;
                }
            } catch(Exception){
            }
        }
        enemyController.MoveTo(wanderNode.worldPosition);
    }

    private void Follow(){
        // Try to attack the target
        enemyController.MoveTo(followTarget.position);
    }

    private void CleanTile()
    {
        Node node = BoardController.Instance.grid.NodeFromWorldPoint(transform.position);
        foreach (GameObject splat in node.splats)
        {
            splat.GetComponent<SpriteRenderer>().DOColor(new Color(1,1,1,0), 0.7f).OnComplete(() => { Destroy(splat); });
        }
        node.splats.Clear();
    }

}
