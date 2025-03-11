using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMeshProToImage : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Image targetImage;
    public Camera renderCamera;
    public RenderTexture renderTexture;

    void Start()
    {
        ConvertTextToImage();
    }

    public void ConvertTextToImage()
    {
        // Ensure RenderTexture is assigned
        if (renderTexture == null || renderCamera == null || textMeshPro == null)
        {
            Debug.LogError("Missing required components.");
            return;
        }

        // Set Camera target texture
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();

        // Read the pixels into a new Texture2D
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Convert Texture2D to Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Assign to UI Image
        targetImage.sprite = sprite;
        targetImage.preserveAspect = true;
    }
}
