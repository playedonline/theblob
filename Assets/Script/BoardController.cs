using UnityEngine;

public class BoardController : MonoBehaviour {

    public Grid grid;

    void Awake(){
        grid = GetComponent<Grid>();
    }

    void OnDrawGizmos(){
    }

}
