using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake() {
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}

	public static void RequestPath(GameObject requester, Vector3 pathStart, Vector3 pathEnd, Action<Node[], bool> callback) {
		PathRequest newRequest = new PathRequest(requester, pathStart,pathEnd,callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	void TryProcessNext() {
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.requester, currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	public void FinishedProcessingPath(Node[] path, bool success) {
		currentPathRequest.callback(path, success);
		isProcessingPath = false;
		TryProcessNext();
	}

	struct PathRequest {
        public GameObject requester;
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Node[], bool> callback;

		public PathRequest(GameObject _requester, Vector3 _start, Vector3 _end, Action<Node[], bool> _callback) {
            requester = _requester;
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}
