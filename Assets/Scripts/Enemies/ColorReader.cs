using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorReader : MonoBehaviour
{
    private Camera renderTextureCamera;
    private RenderTexture renderTexture;

    public bool shouldBePhased { get; private set; } = false;

    private Color colorUnderEnemy;

    public IEnumerator GetColorUnderEnemy()
    {
        if (renderTextureCamera == null || renderTexture == null)
            yield break;

        renderTextureCamera.transform.position = transform.position + Vector3.up;
        renderTextureCamera.transform.rotation = Quaternion.LookRotation(Vector3.down);

        yield return new WaitForEndOfFrame();

        Color color = GetCenterPixelColor();
        colorUnderEnemy = color;
        CompareColor();
    }

    private Color GetCenterPixelColor()
    {
        Texture2D tempTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        tempTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tempTexture.Apply();

        Color color = tempTexture.GetPixel(tempTexture.width / 2, tempTexture.height / 2);

        Destroy(tempTexture);
        RenderTexture.active = null;

        return color;
    }

    public void SetCameraAndRenderTexture(Camera camera, RenderTexture rt)
    {
        renderTextureCamera = camera;
        renderTexture = rt;
    }

    private void CompareColor()
    {
        if (colorUnderEnemy.g > 0.5)
        {
            shouldBePhased = true;
        }
        else
        {
            shouldBePhased = false;
        }
    }
}
