using UnityEngine;

namespace Script {
    public class MeshTest : MonoBehaviour{

        public Mesh theMesh;

        void Awake(){

            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.active = false;
                i++;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.active = true;

            var meshes = GetComponentsInChildren<Mesh>();
            theMesh = meshes[0].CombineMeshes(new Mesh[]{meshes[1]});
        }
    }
}