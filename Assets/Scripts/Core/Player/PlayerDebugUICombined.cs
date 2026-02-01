using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerDebugUICombined : MonoBehaviour
{
    [Header("External")]
    public PlayerAnimationController anim;
    public Rigidbody2D rb;
    public ManualLetterbox letterbox;


    // Core
    Canvas canvas;
    EventSystem eventSystem;

    // Window + drag
    RectTransform windowRect;
    TextMeshProUGUI titleLabel;
    TextMeshProUGUI fpsLabel;

    TextMeshProUGUI scaleFactorLabel;

    // Tabs
    Button tabAnimation;
    Button tabMovement;
    Button tabCombat;
    Button tabPhysics;

    GameObject panelAnimation;
    GameObject panelMovement;
    GameObject panelCombat;
    GameObject panelPhysics;

    // Animation tab
    TextMeshProUGUI facingLabel;
    LineRenderer facingArrow;
    TextMeshProUGUI stateMachineLabel;
    Transform stateListParent;
    GameObject stateEntryPrefab;
    TextMeshProUGUI previewLabel;
    RawImage previewImage;
    TextMeshProUGUI flipXLabel;
    Image flipXIndicator;
    TextMeshProUGUI quadrantLabel;
    RectTransform quadrantVisualizer;
    RectTransform quadrantDot;

    // Movement tab
    TextMeshProUGUI velocityLabel;
    RawImage velocityGraph;
    Texture2D velTex;
    const int VelocitySamples = 60;
    float[] velHistory = new float[VelocitySamples];
    int velIndex;

    // Combat tab
    TextMeshProUGUI combatInfoLabel;

    // Physics tab
    TextMeshProUGUI physicsInfoLabel;
    RawImage hitboxView;

    // Collapsibles
    Button animSectionHeader;
    GameObject animSectionBody;
    Button moveSectionHeader;
    GameObject moveSectionBody;
    Button combatSectionHeader;
    GameObject combatSectionBody;
    Button physicsSectionHeader;
    GameObject physicsSectionBody;

    // State
    bool uiReady;
    float fpsSmoothed;

    void Start()
    {
        try
        {
            BuildUI();
            ValidateUI();
            uiReady = true;
            Debug.Log("[PlayerDebugUI] UI built and validated.");
        }
        catch (System.SystemException e)
        {
            Debug.LogError("[PlayerDebugUI] Failed to build UI: " + e);
            uiReady = false;
        }
    }

    void Update()
    {
        try
        {
            if (!uiReady) return;
            if (anim == null) return;

            UpdateFPS();
            UpdateScaleFactor();

            if (panelAnimation.activeSelf)
                UpdateAnimationTab();

            if (panelMovement.activeSelf)
                UpdateMovementTab();

            if (panelCombat.activeSelf)
                UpdateCombatTab();

            if (panelPhysics.activeSelf)
                UpdatePhysicsTab();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Debug UI Update failed: " + e);
        }
    }

    // ---------------- BUILD ----------------

    void BuildUI()
    {
        Debug.Log("BuildUI() START");
        
        // Canvas
        GameObject canvasGO = new GameObject("DebugCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Debug.Log("Canvas created");

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // EventSystem
        eventSystem = Object.FindFirstObjectByType<EventSystem>();
        if (eventSystem == null)
        {
            GameObject esGO = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem = esGO.GetComponent<EventSystem>();
        }
        Debug.Log("EventSystem created");

        // Window
        GameObject windowGO = new GameObject("DebugWindow");

        // Add RectTransform BEFORE parenting
        windowRect = windowGO.AddComponent<RectTransform>();

        // Now parent it
        windowGO.transform.SetParent(canvasGO.transform, false);
        Debug.Log("Window GO created");

        windowRect = windowGO.GetComponent<RectTransform>();
        windowRect.sizeDelta = new Vector2(420, 600);
        windowRect.anchorMin = new Vector2(0, 1);
        windowRect.anchorMax = new Vector2(0, 1);
        windowRect.pivot = new Vector2(0, 1);
        windowRect.anchoredPosition = new Vector2(20, -20);
        Debug.Log("Window RectTransform added");

        Image winBG = windowGO.AddComponent<Image>();
        winBG.color = new Color(0, 0, 0, 0.7f);
        winBG.raycastTarget = true;
        Debug.Log("Window Image added");

        Debug.Log("Adding draggable...");
        windowGO.AddComponent<DebugWindowDraggable>();
        Debug.Log("Added draggable...");

        // Title + FPS
        titleLabel = CreateLabel("Title", windowGO.transform, "Player Debug", 18, FontStyles.Bold, new Vector2(10, -20));
        fpsLabel = CreateLabel("FPSLabel", windowGO.transform, "FPS:", 14, FontStyles.Normal, new Vector2(10, -50));
        scaleFactorLabel = CreateLabel("ScaleFactorLabel", windowGO.transform, "ScaleFactor:", 14, FontStyles.Normal, new Vector2(200, -50));

        // Tabs row
        GameObject tabsRow = CreateUI("TabsRow", windowGO.transform);
        RectTransform tabsRect = tabsRow.AddComponent<RectTransform>();
        tabsRect.sizeDelta = new Vector2(400, 30);
        tabsRect.anchorMin = new Vector2(0, 1);
        tabsRect.anchorMax = new Vector2(0, 1);
        tabsRect.pivot = new Vector2(0, 1);
        tabsRect.anchoredPosition = new Vector2(10, -80);

        HorizontalLayoutGroup tabsLayout = tabsRow.AddComponent<HorizontalLayoutGroup>();
        tabsLayout.childControlWidth = true;
        tabsLayout.childForceExpandWidth = true;
        tabsLayout.spacing = 5;

        tabAnimation = CreateTabButton("Animation", tabsRow.transform);
        tabMovement  = CreateTabButton("Movement",  tabsRow.transform);
        tabCombat    = CreateTabButton("Combat",    tabsRow.transform);
        tabPhysics   = CreateTabButton("Physics",   tabsRow.transform);

        // Content root
        GameObject contentRoot = CreateUI("ContentRoot", windowGO.transform);
        RectTransform contentRect = contentRoot.AddComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(400, 470);
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(0, 1);
        contentRect.pivot = new Vector2(0, 1);
        contentRect.anchoredPosition = new Vector2(10, -120);

        // Panels
        panelAnimation = CreatePanel("AnimationPanel", contentRoot.transform);
        panelMovement  = CreatePanel("MovementPanel",  contentRoot.transform);
        panelCombat    = CreatePanel("CombatPanel",    contentRoot.transform);
        panelPhysics   = CreatePanel("PhysicsPanel",   contentRoot.transform);

        BuildAnimationPanel();
        BuildMovementPanel();
        BuildCombatPanel();
        BuildPhysicsPanel();

        // Tabs wiring
        tabAnimation.onClick.AddListener(() => ShowTab(panelAnimation));
        tabMovement.onClick.AddListener(() => ShowTab(panelMovement));
        tabCombat.onClick.AddListener(() => ShowTab(panelCombat));
        tabPhysics.onClick.AddListener(() => ShowTab(panelPhysics));

        ShowTab(panelAnimation);

        // Collapsibles wiring
        animSectionHeader.onClick.AddListener(() => ToggleSection(animSectionBody));
        moveSectionHeader.onClick.AddListener(() => ToggleSection(moveSectionBody));
        combatSectionHeader.onClick.AddListener(() => ToggleSection(combatSectionBody));
        physicsSectionHeader.onClick.AddListener(() => ToggleSection(physicsSectionBody));

        // Velocity graph texture
        velTex = new Texture2D(VelocitySamples, 40, TextureFormat.RGBA32, false);
        velTex.filterMode = FilterMode.Point;
        velocityGraph.texture = velTex;
    }

    void BuildAnimationPanel()
    {
        Transform panel = panelAnimation.transform;

        animSectionHeader = CreateSectionHeader("Animation", panel, out animSectionBody);

        facingLabel = CreateLabel("FacingLabel", animSectionBody.transform, "Facing:", 14, FontStyles.Normal, new Vector2(10, -30));

        GameObject arrowGO = CreateUI("FacingArrow", animSectionBody.transform);
        RectTransform arrowRect = arrowGO.AddComponent<RectTransform>();
        arrowRect.sizeDelta = new Vector2(100, 100);
        arrowRect.anchorMin = new Vector2(0, 1);
        arrowRect.anchorMax = new Vector2(0, 1);
        arrowRect.pivot = new Vector2(0, 1);
        arrowRect.anchoredPosition = new Vector2(200, -60);

        facingArrow = arrowGO.AddComponent<LineRenderer>();
        facingArrow.useWorldSpace = false;
        facingArrow.widthMultiplier = 3f;
        facingArrow.material = new Material(Shader.Find("Sprites/Default"));
        facingArrow.positionCount = 2;

        stateMachineLabel = CreateLabel("StateMachineLabel", animSectionBody.transform, "State Machine:", 14, FontStyles.Normal, new Vector2(10, -110));

        GameObject stateListGO = CreateUI("StateList", animSectionBody.transform);
        RectTransform stateRect = stateListGO.AddComponent<RectTransform>();
        stateRect.sizeDelta = new Vector2(360, 120);
        stateRect.anchorMin = new Vector2(0, 1);
        stateRect.anchorMax = new Vector2(0, 1);
        stateRect.pivot = new Vector2(0, 1);
        stateRect.anchoredPosition = new Vector2(10, -150);

        VerticalLayoutGroup vlg = stateListGO.AddComponent<VerticalLayoutGroup>();
        vlg.childControlHeight = true;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.spacing = 4;

        stateListParent = stateListGO.transform;
        stateEntryPrefab = BuildStateEntryPrefab();

        previewLabel = CreateLabel("PreviewLabel", animSectionBody.transform, "Live Preview:", 14, FontStyles.Normal, new Vector2(10, -290));

        GameObject previewGO = CreateUI("PreviewImage", animSectionBody.transform);
        previewImage = previewGO.AddComponent<RawImage>();
        RectTransform previewRect = previewGO.GetComponent<RectTransform>();
        previewRect.sizeDelta = new Vector2(80, 80);
        previewRect.anchorMin = new Vector2(0, 1);
        previewRect.anchorMax = new Vector2(0, 1);
        previewRect.pivot = new Vector2(0, 1);
        previewRect.anchoredPosition = new Vector2(10, -330);

        flipXLabel = CreateLabel("FlipXLabel", animSectionBody.transform, "flipX:", 14, FontStyles.Normal, new Vector2(110, -300));

        GameObject flipGO = CreateUI("FlipXIndicator", animSectionBody.transform);
        flipXIndicator = flipGO.AddComponent<Image>();
        RectTransform flipRect = flipGO.GetComponent<RectTransform>();
        flipRect.sizeDelta = new Vector2(20, 20);
        flipRect.anchorMin = new Vector2(0, 1);
        flipRect.anchorMax = new Vector2(0, 1);
        flipRect.pivot = new Vector2(0, 1);
        flipRect.anchoredPosition = new Vector2(170, -300);

        quadrantLabel = CreateLabel("QuadrantLabel", animSectionBody.transform, "Direction Quadrant:", 14, FontStyles.Normal, new Vector2(110, -330));

        GameObject quadGO = CreateUI("QuadrantVisualizer", animSectionBody.transform);
        Image quadImg = quadGO.AddComponent<Image>();
        quadImg.color = new Color(0, 0, 0, 0.3f);

        quadrantVisualizer = quadGO.GetComponent<RectTransform>();
        quadrantVisualizer.sizeDelta = new Vector2(120, 120);
        quadrantVisualizer.anchorMin = new Vector2(0, 1);
        quadrantVisualizer.anchorMax = new Vector2(0, 1);
        quadrantVisualizer.pivot = new Vector2(0, 1);
        quadrantVisualizer.anchoredPosition = new Vector2(200, -380);

        GameObject dotGO = CreateUI("Dot", quadGO.transform);
        Image dotImg = dotGO.AddComponent<Image>();
        dotImg.color = Color.yellow;
        quadrantDot = dotGO.GetComponent<RectTransform>();
        quadrantDot.sizeDelta = new Vector2(10, 10);
    }

    void BuildMovementPanel()
    {
        Transform panel = panelMovement.transform;

        moveSectionHeader = CreateSectionHeader("Movement", panel, out moveSectionBody);

        velocityLabel = CreateLabel("VelocityLabel", moveSectionBody.transform, "Velocity:", 14, FontStyles.Normal, new Vector2(10, -30));

        GameObject graphGO = CreateUI("VelocityGraph", moveSectionBody.transform);
        velocityGraph = graphGO.AddComponent<RawImage>();
        RectTransform graphRect = graphGO.GetComponent<RectTransform>();
        graphRect.sizeDelta = new Vector2(360, 80);
        graphRect.anchorMin = new Vector2(0, 1);
        graphRect.anchorMax = new Vector2(0, 1);
        graphRect.pivot = new Vector2(0, 1);
        graphRect.anchoredPosition = new Vector2(10, -80);
    }

    void BuildCombatPanel()
    {
        Transform panel = panelCombat.transform;

        combatSectionHeader = CreateSectionHeader("Combat", panel, out combatSectionBody);

        combatInfoLabel = CreateLabel("CombatInfo", combatSectionBody.transform, "Combat info here", 14, FontStyles.Normal, new Vector2(10, -30));
    }

    void BuildPhysicsPanel()
    {
        Transform panel = panelPhysics.transform;

        physicsSectionHeader = CreateSectionHeader("Physics", panel, out physicsSectionBody);

        physicsInfoLabel = CreateLabel("PhysicsInfo", physicsSectionBody.transform, "Physics info here", 14, FontStyles.Normal, new Vector2(10, -30));

        GameObject hitboxGO = CreateUI("HitboxView", physicsSectionBody.transform);
        hitboxView = hitboxGO.AddComponent<RawImage>();
        RectTransform hitRect = hitboxGO.GetComponent<RectTransform>();
        hitRect.sizeDelta = new Vector2(200, 200);
        hitRect.anchorMin = new Vector2(0, 1);
        hitRect.anchorMax = new Vector2(0, 1);
        hitRect.pivot = new Vector2(0, 1);
        hitRect.anchoredPosition = new Vector2(10, -80);
    }

    // ---------------- VALIDATION ----------------

    void ValidateUI()
    {
        if (canvas == null) throw new System.Exception("Canvas is null");
        if (eventSystem == null) throw new System.Exception("EventSystem is null");
        if (windowRect == null) throw new System.Exception("Window RectTransform is null");
        if (titleLabel == null) throw new System.Exception("titleLabel is null");
        if (fpsLabel == null) throw new System.Exception("fpsLabel is null");
        if (scaleFactorLabel == null) throw new System.Exception("scaleFactorLabel is null");

        if (tabAnimation == null || tabMovement == null || tabCombat == null || tabPhysics == null)
            throw new System.Exception("One or more tab buttons are null");

        if (panelAnimation == null || panelMovement == null || panelCombat == null || panelPhysics == null)
            throw new System.Exception("One or more panels are null");

        if (facingLabel == null) throw new System.Exception("facingLabel is null");
        if (facingArrow == null) throw new System.Exception("facingArrow is null");
        if (stateMachineLabel == null) throw new System.Exception("stateMachineLabel is null");
        if (stateListParent == null) throw new System.Exception("stateListParent is null");
        if (stateEntryPrefab == null) throw new System.Exception("stateEntryPrefab is null");
        if (previewLabel == null) throw new System.Exception("previewLabel is null");
        if (previewImage == null) throw new System.Exception("previewImage is null");
        if (flipXLabel == null) throw new System.Exception("flipXLabel is null");
        if (flipXIndicator == null) throw new System.Exception("flipXIndicator is null");
        if (quadrantLabel == null) throw new System.Exception("quadrantLabel is null");
        if (quadrantVisualizer == null) throw new System.Exception("quadrantVisualizer is null");
        if (quadrantDot == null) throw new System.Exception("quadrantDot is null");

        if (velocityLabel == null) throw new System.Exception("velocityLabel is null");
        if (velocityGraph == null) throw new System.Exception("velocityGraph is null");

        if (combatInfoLabel == null) throw new System.Exception("combatInfoLabel is null");
        if (physicsInfoLabel == null) throw new System.Exception("physicsInfoLabel is null");
        if (hitboxView == null) throw new System.Exception("hitboxView is null");
    }

    // ---------------- UPDATE HELPERS ----------------

    void ShowTab(GameObject target)
    {
        panelAnimation.SetActive(target == panelAnimation);
        panelMovement.SetActive(target == panelMovement);
        panelCombat.SetActive(target == panelCombat);
        panelPhysics.SetActive(target == panelPhysics);
    }

    void ToggleSection(GameObject body)
    {
        if (body != null)
            body.SetActive(!body.activeSelf);
    }

    void UpdateFPS()
    {
        float dt = Time.unscaledDeltaTime;
        float fps = 1f / Mathf.Max(dt, 0.0001f);
        fpsSmoothed = Mathf.Lerp(fpsSmoothed, fps, 0.1f);
        fpsLabel.text = $"FPS: {fpsSmoothed:0.}  Δt: {dt * 1000f:0.0} ms";
    }

    void UpdateScaleFactor()
    {
        scaleFactorLabel.text = $"ScaleFactor: {Mathf.Max(letterbox.CurrentScale, 1)}";   
    }

    void UpdateAnimationTab()
    {
        UpdateFacing();
        UpdateStates();
        UpdatePreview();
        UpdateFlipX();
        UpdateQuadrant();
    }

    void UpdateFacing()
    {
        if (anim == null || facingLabel == null || facingArrow == null) return;

        Vector2 dir = anim.GetFacingDir();
        facingLabel.text = $"Facing: {dir}";

        Vector3 start = Vector3.zero;
        Vector3 end = dir.sqrMagnitude < 0.001f ? Vector3.zero : (Vector3)(dir.normalized * 50f);

        facingArrow.positionCount = 2;
        facingArrow.SetPosition(0, start);
        facingArrow.SetPosition(1, end);
    }

    void UpdateStates()
    {
        if (anim == null || stateListParent == null || stateEntryPrefab == null) return;

        foreach (Transform child in stateListParent)
            Destroy(child.gameObject);

        AddState("Idle", anim.CurrentStateName.Contains("Idle"));
        AddState("Move", anim.CurrentStateName.Contains("Move"));
        AddState("Dash", anim.CurrentStateName.Contains("Dash"));
        AddState("Attack", anim.CurrentStateName.Contains("Attack"));
        AddState("Hurt", anim.CurrentStateName.Contains("Hurt"));
    }

    void AddState(string name, bool active)
    {
        // Instantiate without parent
        GameObject entry = Instantiate(stateEntryPrefab);

        // Ensure RectTransform exists BEFORE parenting
        RectTransform rt = entry.GetComponent<RectTransform>();
        if (rt == null)
            rt = entry.AddComponent<RectTransform>();

        // Now parent it safely
        entry.transform.SetParent(stateListParent, false);

        // Set visuals
        entry.GetComponentInChildren<TextMeshProUGUI>().text = name;
        entry.GetComponent<Image>().color = active ? Color.green : Color.gray;

        if (entry.transform.parent.name != "StateList")
            Debug.Log("Parent of state entry: " + entry.transform.parent);
    }

    void UpdatePreview()
    {
        if (previewImage == null || anim == null) return;

        SpriteRenderer sr = anim.GetSpriteRenderer();
        if (sr == null || sr.sprite == null) return;

        Sprite s = sr.sprite;
        Rect rect = s.textureRect;
        Texture2D tex = s.texture;

        previewImage.texture = tex;
        previewImage.uvRect = new Rect(
            rect.x / tex.width,
            rect.y / tex.height,
            rect.width / tex.width,
            rect.height / tex.height
        );
    }

    void UpdateFlipX()
    {
        if (flipXLabel == null || flipXIndicator == null || anim == null) return;

        SpriteRenderer sr = anim.GetSpriteRenderer();
        bool flip = sr != null && sr.flipX;
        flipXLabel.text = $"flipX: {flip}";
        flipXIndicator.color = flip ? Color.red : Color.green;
    }

    void UpdateQuadrant()
    {
        if (quadrantVisualizer == null || quadrantDot == null || anim == null) return;

        Vector2 dir = anim.GetFacingDir();
        if (dir.sqrMagnitude < 0.01f)
        {
            quadrantDot.anchoredPosition = Vector2.zero;
            return;
        }

        float radius = quadrantVisualizer.rect.width / 2f - 10f;
        quadrantDot.anchoredPosition = dir.normalized * radius;
    }

    void UpdateMovementTab()
    {
        if (velocityLabel == null || velocityGraph == null) return;

        Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;
        velocityLabel.text = $"Velocity: {vel}  |  Speed: {vel.magnitude:0.00}";

        velHistory[velIndex] = vel.magnitude;
        velIndex = (velIndex + 1) % VelocitySamples;

        RedrawVelocityGraph();
    }

    void RedrawVelocityGraph()
    {
        if (velTex == null) return;

        var clear = new Color32[VelocitySamples * velTex.height];
        velTex.SetPixels32(clear);

        float max = 0.1f;
        for (int i = 0; i < VelocitySamples; i++)
            max = Mathf.Max(max, velHistory[i]);

        for (int x = 0; x < VelocitySamples; x++)
        {
            int idx = (velIndex + x) % VelocitySamples;
            float v = velHistory[idx] / max;
            int h = Mathf.RoundToInt(v * (velTex.height - 1));

            for (int y = 0; y <= h; y++)
                velTex.SetPixel(x, y, Color.green);
        }

        velTex.Apply();
    }

    void UpdateCombatTab()
    {
        if (combatInfoLabel == null) return;
        combatInfoLabel.text = "Combat: hook into your combo / cooldown / last hit data here.";
    }

    void UpdatePhysicsTab()
    {
        if (physicsInfoLabel == null) return;
        physicsInfoLabel.text = "Physics: show grounded, raycasts, overlap checks, etc.";
        // hitboxView: you can draw into hitboxView.texture if desired
    }

    // ---------------- HELPERS ----------------

    GameObject CreateUI(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        return go;
    }

    TextMeshProUGUI CreateLabel(string name, Transform parent, string text, int size, FontStyles style, Vector2 anchoredPos)
    {
        GameObject go = CreateUI(name, parent);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.fontStyle = style;
        tmp.color = Color.white;

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(380, 24);
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = anchoredPos;

        return tmp;
    }

    Button CreateTabButton(string label, Transform parent)
    {
        GameObject go = CreateUI(label + "Tab", parent);
        Image img = go.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 1f);

        Button btn = go.AddComponent<Button>();

        GameObject textGO = CreateUI("Label", go.transform);
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 14;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform rt = textGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        ColorBlock cb = btn.colors;
        cb.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        cb.pressedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        btn.colors = cb;

        return btn;
    }

    GameObject CreatePanel(string name, Transform parent)
    {
        GameObject go = CreateUI(name, parent);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.sizeDelta = new Vector2(400, 470);
        rt.anchoredPosition = Vector2.zero;
        return go;
    }

Button CreateSectionHeader(string title, Transform parent, out GameObject body)
{
    // HEADER ROOT (background + button)
    GameObject headerGO = new GameObject(title + "Header");

    // Ensure RectTransform exists BEFORE parenting
    RectTransform hrt = headerGO.GetComponent<RectTransform>();
    if (hrt == null)
        hrt = headerGO.AddComponent<RectTransform>();

    headerGO.transform.SetParent(parent, false);

    hrt.sizeDelta = new Vector2(380, 24);
    hrt.anchorMin = new Vector2(0, 1);
    hrt.anchorMax = new Vector2(0, 1);
    hrt.pivot = new Vector2(0, 1);
    hrt.anchoredPosition = new Vector2(10, -10);

    // Background image (the Graphic)
    Image bg = headerGO.AddComponent<Image>();
    bg.color = new Color(0.15f, 0.15f, 0.15f, 1f);

    // Button
    Button btn = headerGO.AddComponent<Button>();

    // LABEL CHILD (this is where TMP goes)
    GameObject labelGO = new GameObject("Label");

    RectTransform lrt = labelGO.GetComponent<RectTransform>();
    if (lrt == null)
        lrt = labelGO.AddComponent<RectTransform>();

    labelGO.transform.SetParent(headerGO.transform, false);

    lrt.anchorMin = Vector2.zero;
    lrt.anchorMax = Vector2.one;
    lrt.offsetMin = Vector2.zero;
    lrt.offsetMax = Vector2.zero;

    TextMeshProUGUI tmp = labelGO.AddComponent<TextMeshProUGUI>();
    tmp.text = title;
    tmp.fontSize = 14;
    tmp.color = Color.white;
    tmp.alignment = TextAlignmentOptions.MidlineLeft;
    tmp.margin = new Vector4(6, 0, 0, 0);

    // BODY (collapsible content)
    body = new GameObject(title + "Body");

    RectTransform brt = body.GetComponent<RectTransform>();
    if (brt == null)
        brt = body.AddComponent<RectTransform>();

    body.transform.SetParent(parent, false);

    brt.anchorMin = new Vector2(0, 1);
    brt.anchorMax = new Vector2(0, 1);
    brt.pivot = new Vector2(0, 1);
    brt.sizeDelta = new Vector2(380, 430);
    brt.anchoredPosition = new Vector2(10, -40);

    return btn;
}

    GameObject BuildStateEntryPrefab()
    {
        GameObject entry = new GameObject("StateEntry");

        // Always ensure RectTransform exists BEFORE anything else
        RectTransform rt = entry.GetComponent<RectTransform>();
        if (rt == null)
            rt = entry.AddComponent<RectTransform>();

        rt.sizeDelta = new Vector2(360, 24);

        Image bg = entry.AddComponent<Image>();
        bg.color = Color.gray;

        GameObject labelGO = new GameObject("Label");

        RectTransform lrt = labelGO.GetComponent<RectTransform>();
        if (lrt == null)
            lrt = labelGO.AddComponent<RectTransform>();

        labelGO.transform.SetParent(entry.transform, false);

        TextMeshProUGUI tmp = labelGO.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = 14;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.margin = new Vector4(6, 0, 0, 0);

        return entry;
    }
}

public class DebugWindowDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    RectTransform rect;
    Canvas rootCanvas;
    Vector2 offset;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rootCanvas = GetComponentInParent<Canvas>().rootCanvas;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out offset
        );

        offset = rect.anchoredPosition - offset;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootCanvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        rect.anchoredPosition = pos + offset;
    }
}