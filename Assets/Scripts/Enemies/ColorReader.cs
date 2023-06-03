using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorReader : MonoBehaviour
{
    [SerializeField] private Camera renderTextureCamera;
    [SerializeField] private RenderTexture renderTexture;

    public bool shouldBePhased { get; private set; } = false;

    private bool readColor = false;
    private Color colorUnderEnemy;

    private void Start()
    {
        renderTexture = new RenderTexture(1, 1, 24);
        renderTextureCamera.targetTexture = renderTexture;
    }

    private void Update()
    {
        if (readColor)
        {
            GetColorUnderEnemy();
        }
    }

    public void GetColorUnderEnemy()
    {
        renderTextureCamera.transform.position = transform.position + Vector3.up;
        renderTextureCamera.transform.rotation = Quaternion.LookRotation(Vector3.down);

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

    private void StartReadingColor()
    {
        readColor = true;
    }

    private void StopReadingColor()
    {
        readColor = false;
    }

    private void OnEnable()
    {
        GameController.OnAIPositiveTrigger += StartReadingColor;
        GameController.OnAINegativeTrigger += StopReadingColor;
    }

    private void OnDisable()
    {
        GameController.OnAIPositiveTrigger -= StartReadingColor;
        GameController.OnAINegativeTrigger -= StopReadingColor;
    }
}
