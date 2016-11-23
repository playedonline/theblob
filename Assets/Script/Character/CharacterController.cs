using UnityEngine;

public class CharacterController : MonoBehaviour{

    public float moveSpeed = 200;
    public float radiusStop = 3;
    public float radiusSlow = 20;

    public Vector2 target;
    public bool isOnPosition;
    public bool isMovingLeft;
    private Rigidbody2D rigidbody;

    void Awake(){
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start(){
        target = transform.position;
    }

    void Update(){
        if(!isOnPosition){
            MoveToTarget();
        }
    }

    /*

        Public methods

     */

    public void SetTarget(Vector2 target){
        isOnPosition = false;
        this.target = target;
    }

    /*

        Private methods

     */

    private void MoveToTarget(){
        Vector2 moveTo = target - (Vector2)transform.position;

        float distance = Vector2.Distance(transform.position, target);
        if(distance <= radiusStop){
            isOnPosition = true;
            return;
        }
        float slowFactor = Mathf.Min(distance / radiusSlow, 1);
        float speed = moveSpeed * slowFactor;
        transform.position += (Vector3)(moveTo.normalized * Time.deltaTime * speed);

        isMovingLeft = moveTo.x < 0;
    }

}
