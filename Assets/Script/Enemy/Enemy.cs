using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    public Transform followTarget;
    public EnemyController enemyController;

    public float acceleration;

    private int frameCount = 0;

    private float velocity;

    void Awake(){
        enemyController = GetComponent<EnemyController>();
        followTarget = FindObjectOfType<Character>().transform;
    }

    void Update(){

        frameCount++;
        if(frameCount % 4 != 0){
            return;
        }

        if(!followTarget){
            Wander();
        } else {
            Follow();
        }

        CleanTile();
    }

    /*

        Private methods

     */

    private void Wander(){
        // Try to move to a position
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
