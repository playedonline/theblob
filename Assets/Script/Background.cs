using UnityEngine;

    public class Background : MonoBehaviour {
        public float scale = 1;
        void Update(){
            transform.localScale = Vector3.one * scale;
        }
    }
