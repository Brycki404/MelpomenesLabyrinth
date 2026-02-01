using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuBuilder : MonoBehaviour
{
    [Header("References")]
    public RectTransform uiRoot; // assign Canvas.UIRoot in Inspector
    public PauseMenuController controller; // your logic script

    void Start()
    {
        BuildPauseMenu();

        controller.enabled = true;
    }

    void BuildPauseMenu()
    {
        // -----------------------------
        // Create PauseMenu root object
        // -----------------------------
        GameObject pauseMenuGO = new GameObject("PauseMenu", typeof(RectTransform));
        RectTransform pauseMenu = pauseMenuGO.GetComponent<RectTransform>();
        pauseMenu.SetParent(uiRoot, false);

        pauseMenu.anchorMin = new Vector2(0, 1);
        pauseMenu.anchorMax = new Vector2(0, 1);
        pauseMenu.pivot = new Vector2(0, 1);
        pauseMenu.anchoredPosition = new Vector2(0, 0);
        pauseMenu.sizeDelta = uiRoot.sizeDelta;

        pauseMenuGO.SetActive(true); // hidden by default
        controller.panel = pauseMenu; // hook into controller


        // -----------------------------
        // Create Panel
        // -----------------------------
        GameObject panelGO = new GameObject("Panel", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
        controller.panelGO = panelGO;
        RectTransform panel = panelGO.GetComponent<RectTransform>();
        panel.SetParent(pauseMenu, false);

        panel.anchorMin = new Vector2(0, 1);
        panel.anchorMax = new Vector2(0, 1);
        panel.pivot = new Vector2(0, 1);
        panel.anchoredPosition = new Vector2(0, 0);
        panel.sizeDelta = new Vector2(640, 480);

        CanvasGroup group = panelGO.GetComponent<CanvasGroup>();
        controller.group = group;

        Image panelImg = panelGO.GetComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0.6f); // semi‑transparent black


        // -----------------------------
        // Helper: Create Text
        // -----------------------------
        TextMeshProUGUI MakeText(string name, Transform parent, Vector2 pos, int size)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);

            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = pos;

            TextMeshProUGUI txt = go.GetComponent<TextMeshProUGUI>();
            txt.fontSize = size;
            txt.color = Color.white;

            return txt;
        }

        // -----------------------------
        // Title
        // -----------------------------
        var title = MakeText("TitleText", panel, new Vector2(20, -20), 36);
        title.text = "Paused";


        // -----------------------------
        // Music Label
        // -----------------------------
        var musicLabel = MakeText("MusicLabel", panel, new Vector2(20, -90), 24);
        musicLabel.text = "Music Volume";


        // -----------------------------
        // Music Slider
        // -----------------------------
        Slider musicSlider = CreateSlider("MusicSlider", panel, new Vector2(20, -130));
        controller.musicSlider = musicSlider;


        // -----------------------------
        // SFX Label
        // -----------------------------
        var sfxLabel = MakeText("SFXLabel", panel, new Vector2(20, -190), 24);
        sfxLabel.text = "SFX Volume";


        // -----------------------------
        // SFX Slider
        // -----------------------------
        Slider sfxSlider = CreateSlider("SFXSlider", panel, new Vector2(20, -230));
        controller.sfxSlider = sfxSlider;


        // -----------------------------
        // Resume Button
        // -----------------------------
        Button resumeBtn = CreateButton("ResumeButton", panel, new Vector2(20, -310), "Resume");
        resumeBtn.onClick.AddListener(controller.OnResumePressed);


        // -----------------------------
        // Quit Button
        // -----------------------------
        Button quitBtn = CreateButton("QuitButton", panel, new Vector2(20, -390), "Quit to Title");
        quitBtn.onClick.AddListener(controller.OnQuitPressed);
    }


    // ============================================================
    // UI ELEMENT HELPERS
    // ============================================================

    Slider CreateSlider(string name, Transform parent, Vector2 pos)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Slider));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(380, 30);

        Slider slider = go.GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 1f;

        return slider;
    }

    Button CreateButton(string name, Transform parent, Vector2 pos, string label)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(parent, false);

        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(380, 60);

        Image img = go.GetComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);

        // Button label
        GameObject txtGO = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        RectTransform txtRT = txtGO.GetComponent<RectTransform>();
        txtRT.SetParent(go.transform, false);

        txtRT.anchorMin = new Vector2(0.5f, 0.5f);
        txtRT.anchorMax = new Vector2(0.5f, 0.5f);
        txtRT.pivot = new Vector2(0.5f, 0.5f);
        txtRT.anchoredPosition = Vector2.zero;

        TextMeshProUGUI txt = txtGO.GetComponent<TextMeshProUGUI>();
        txt.text = label;
        txt.fontSize = 28;
        txt.color = Color.white;
        txt.alignment = TextAlignmentOptions.Center;

        return go.GetComponent<Button>();
    }
}