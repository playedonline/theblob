using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class MaskUpdater : MonoBehaviour
{

    SpriteMask mask;
    Camera screenCamera;

    private const int referenceTextureWidth = 1080;
    private const int referenceTextureHeight = 1920;
    private const int textureSizeFactor = 2;

    RenderTexture maskRenderTexture;
    RenderTexture gameRenderTexture;

    private int textureWidth;
    private int textureHeight;

    int glowSortingLayerId;
    Texture2D maskTexture;

    float lastMaskUpdate;

    private Dictionary<Enemy, Vector3> enemyPositions = new Dictionary<Enemy, Vector3>();

    void Awake()
    {
        textureWidth = referenceTextureWidth / textureSizeFactor;
        textureHeight = referenceTextureHeight / textureSizeFactor; 

        mask = GetComponent<SpriteMask>();
        mask.transform.localScale = textureSizeFactor * Vector3.one;
        screenCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        maskRenderTexture = RenderTexture.GetTemporary(textureWidth, textureHeight);
        gameRenderTexture = screenCamera.targetTexture;

        glowSortingLayerId = LayerMask.NameToLayer("MaskRender");
        maskTexture = new Texture2D(textureWidth, textureHeight);
    }

    void Update()
    {
        if (Time.time - lastMaskUpdate > 0.1f)
        {
            lastMaskUpdate = Time.time;
            UpdateMaskSprite();
        }


    }


    void UpdateMaskSprite()
    {
        foreach (Enemy enemy in BoardController.Instance.enemies)
        {
            enemyPositions[enemy] = enemy.transform.position;
            enemy.transform.localScale = 0.5f * Vector3.one;
            enemy.transform.position = 0.5f * enemy.transform.position;
        }
        // Disable camera background, set it to render only our object
        Color oldBgColor = screenCamera.backgroundColor;
        int oldSortingLayerId = screenCamera.cullingMask;
        CameraClearFlags oldClearFlags = screenCamera.clearFlags;

        screenCamera.clearFlags = CameraClearFlags.Nothing;
        screenCamera.backgroundColor = new Color(0, 0, 0, 0);
        screenCamera.cullingMask = 1 << glowSortingLayerId;

        // attach a render texture to the camera
        screenCamera.targetTexture = maskRenderTexture;

        // Fetch the texture
        RenderTexture currentRt = RenderTexture.active;
        RenderTexture.active = screenCamera.targetTexture;

        screenCamera.Render();

        maskTexture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
        maskTexture.Apply();
        RenderTexture.active = currentRt;

        // Create the sprite renderer
        Rect textureFetchRect = new Rect(0, 0, textureWidth, textureHeight);
        Sprite maskSprite = Sprite.Create(maskTexture, textureFetchRect, Vector2.one / 2.0f, 1);
        mask.sprite = maskSprite;

        // Restore camera properties
        screenCamera.targetTexture = gameRenderTexture;
        screenCamera.backgroundColor = oldBgColor;
        screenCamera.cullingMask = oldSortingLayerId;
        screenCamera.clearFlags = oldClearFlags;

        foreach (Enemy enemy in BoardController.Instance.enemies)
        {
            enemy.transform.localScale = Vector3.one;
            enemy.transform.position = enemyPositions[enemy];
        }

    }
}
