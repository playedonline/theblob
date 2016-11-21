using UnityEngine;

public class MaskUpdater : MonoBehaviour {

    void Update() {
        var mask = GetComponent<SpriteMask>();
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
            mask.updateSprites(renderer.transform);
        }
    }

}
