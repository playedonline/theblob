using UnityEngine;

public class Character : MonoBehaviour {

    private CharacterController controller;

    void Start(){
        controller = GetComponent<CharacterController>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            controller.SetTarget(target);
        }
    }

}