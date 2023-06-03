using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorReaderManager : MonoBehaviour
{
    [SerializeField] private Camera renderTextureCamera;
    [SerializeField] private RenderTexture renderTexture;

    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(1, 1, 24);
        renderTextureCamera.targetTexture = renderTexture;

        ColorReader[] colorReaders = FindObjectsOfType<ColorReader>();

        foreach (ColorReader cr in colorReaders)
        {
            cr.SetCameraAndRenderTexture(renderTextureCamera, renderTexture);
        }
    }
}
