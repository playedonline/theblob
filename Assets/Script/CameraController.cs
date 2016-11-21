using UnityEngine;

public class CameraController : MonoBehaviour{

    public float dampTime = 0.15f;
    public BoardController board;
    public Transform target;
    public Camera camera;
    private Vector3 velocity = Vector3.zero;

    void Start(){
        camera = Camera.main;
        target = FindObjectOfType<Character>().transform;
        board = FindObjectOfType<BoardController>();
    }

    void LateUpdate(){
        if (target == null){
            return;
        }

        Vector3 point = camera.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

        var cameraHeight = camera.orthographicSize;
        var cameraWidth = camera.aspect * cameraHeight;

        targetPosition.x = Mathf.Clamp(targetPosition.x, cameraWidth, board.grid.gridWorldSize.x - cameraWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, cameraHeight, board.grid.gridWorldSize.y - cameraHeight);

        transform.position = targetPosition;
    }

}
