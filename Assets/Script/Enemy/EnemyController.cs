using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Action reachedWaypoint;

    public Vector2 target;
    public float currentSpeed;
    public float speed = 120;
    public float cleanSpead = 50;
    public bool isMove;
    public bool isMovingLeft;

    private Node[] path;
    private int targetIndex;
    private BoardController board;
    private bool isReached;

    void Start() {
        currentSpeed = speed;
        board = FindObjectOfType<BoardController>();
    }

    void Update(){
        if(isMove && path != null) {
            FollowTarget();
        }
    }

    public void MoveTo(Vector2 target){
        isMove = true;
        this.target = target;
        PathRequestManager.RequestPath(gameObject, transform.position, target, OnPathFound);
    }

    public void Stop(){
        isMove = false;
        path = null;
    }

    /*

        Private methods

     */

    private void FollowTarget(){
        Vector3 currentWaypoint = GetWaypoint();
        if(transform.position == currentWaypoint){
            if (targetIndex < path.Length) {
                targetIndex ++;
                currentWaypoint = GetWaypoint();
            }
        } else {
            isMovingLeft = (transform.position - currentWaypoint).x > 0;
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,currentSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetWaypoint(){
        if(targetIndex < path.Length){
            return path[targetIndex].worldPosition;
        } else {
            return board.grid.NodeFromWorldPoint(target).worldPosition;
        }
    }

    private void OnPathFound(Node[] newPath, bool pathSuccessful) {
        if (!pathSuccessful) {
            return;
        }
        path = newPath;
        targetIndex = 0;
    }

    public void OnDrawGizmos() {
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i ++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i].worldPosition, Vector3.one);

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i].worldPosition);
                }
                else {
                    Gizmos.DrawLine(path[i-1].worldPosition, path[i].worldPosition);
                }
            }
        }
    }

}
