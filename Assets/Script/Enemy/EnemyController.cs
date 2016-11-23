using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector2 target;
    public float speed = 100;

    private Node[] path;
    private int targetIndex;
    private BoardController board;

    void Start() {
        board = FindObjectOfType<BoardController>();
    }

    void Update(){
        if(target != null && path != null) {
            FollowTarget();
        }
    }

    public void MoveTo(Vector2 target){
        this.target = target;
        PathRequestManager.RequestPath(gameObject, transform.position, target, OnPathFound);
    }

    /*

        Private methods

     */

    private void FollowTarget(){
        Vector3 currentWaypoint = GetWaypoint();
        if (targetIndex < path.Length && transform.position == currentWaypoint) {
            targetIndex ++;
            currentWaypoint = GetWaypoint();
        }
        transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
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
