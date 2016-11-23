using UnityEngine;

public class Floating : MonoBehaviour {

    private float y;
    public float amplitude = 4f;
    public float speed = 1f;

    void Start(){
        y = transform.position.y;
    }

    void Update(){
        transform.position = new Vector3(transform.position.x, y+amplitude*Mathf.Sin(speed*Time.time), transform.position.z) ;
    }
}
