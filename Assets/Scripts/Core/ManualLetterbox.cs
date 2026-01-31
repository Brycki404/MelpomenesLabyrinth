using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ManualLetterbox : MonoBehaviour
{
    public int refWidth = 640;
    public int refHeight = 480;
    public int CurrentScale { get; private set; } = 1;

    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        int screenW = Screen.width;
        int screenH = Screen.height;

        // Enforce 4:3 aspect ratio
        float targetAspect = (float)refWidth / refHeight;
        float windowAspect = (float)screenW / screenH;

        int viewportW, viewportH;

        if (windowAspect > targetAspect)
        {
            // Window is too wide → pillarbox
            viewportH = screenH;
            viewportW = Mathf.FloorToInt(screenH * targetAspect);
        }
        else
        {
            // Window is too tall → letterbox
            viewportW = screenW;
            viewportH = Mathf.FloorToInt(screenW / targetAspect);
        }

        // Integer scale enforcement
        int scale = Mathf.FloorToInt(Mathf.Min(
            viewportW / refWidth,
            viewportH / refHeight
        ));
        CurrentScale = Mathf.Max(scale, 1);

        viewportW = refWidth * scale;
        viewportH = refHeight * scale;

        // Center the viewport
        int offsetX = (screenW - viewportW) / 2;
        int offsetY = (screenH - viewportH) / 2;

        cam.pixelRect = new Rect(offsetX, offsetY, viewportW, viewportH);
    }
}