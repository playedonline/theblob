using UnityEngine;

public class Enemy : MonoBehaviour {

    public Transform followTarget;
    public EnemyController enemyController;

    public float acceleration;

    private float velocity;

    void Awake(){
        enemyController = GetComponent<EnemyController>();
    }

    void Update(){
        if(!followTarget){
            Wander();
        } else {
            Follow();
        }
    }

    /*

        Private methods

     */

    private void Wander(){
        // Try to move to a position
    }

    private void Follow(){
        // Try to attack the target
//        enemyController.MoveTo(followTarget.position);
    }

}
