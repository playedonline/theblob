using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Grid : MonoBehaviour {

	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				grid[x,y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();

		var directions = new Vector2[]{
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
        };

        foreach(var direction in directions){

            int x = (int)direction.x;
            int y = (int)direction.y;

            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                neighbours.Add(grid[checkX,checkY]);
            }
        }

		return neighbours;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = worldPosition.x / gridWorldSize.x;
		float percentY = worldPosition.y / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}
	
	void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Vector3 size = new Vector3(gridWorldSize.x, gridWorldSize.y, 1);
		Gizmos.DrawWireCube(transform.position + size * 0.5f, size);
		if (grid != null && displayGridGizmos) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?n.occupy != null ? Color.green : Color.white : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter*0.95f));
			}
		}
	}
}