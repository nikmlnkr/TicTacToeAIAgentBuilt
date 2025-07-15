using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically sets up a complete Main Menu scene at runtime
/// </summary>
public class MainMenuSetup : MonoBehaviour
{
    [Header("Setup Configuration")]
    [Tooltip("Run the setup automatically on Start")]
    public bool autoSetup = true;
    
    [Header("Setup Options")]
    public bool clearExistingUI = true;
    public Font gameFont;
    
    void Start()
    {
        if (autoSetup)
        {
            SetupMainMenuScene();
        }
    }
    
    [ContextMenu("Setup Main Menu Scene")]
    public void SetupMainMenuScene()
    {
        Debug.Log("Setting up Main Menu scene...");
        
        // Clear existing UI if requested
        if (clearExistingUI)
        {
            CleanupExistingUI();
        }
        
        // Ensure we have managers
        SetupManagers();
        
        // Create or find Canvas
        Canvas canvas = SetupCanvas();
        
        // Create Menu Panel with UI
        GameObject menuPanel = CreateMenuPanel(canvas.transform);
        
        // Setup UI Manager references
        SetupUIReferences(menuPanel);
        
        Debug.Log("✅ Main Menu scene setup finished!");
    }
    
    void CleanupExistingUI()
    {
        // Find and destroy existing UI elements
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.name == "Canvas")
            {
                DestroyImmediate(canvas.gameObject);
            }
        }
    }
    
    void SetupManagers()
    {
        // Ensure SoundManager exists
        if (SoundManager.Instance == null)
        {
            GameObject soundManager = new GameObject("SoundManager");
            soundManager.AddComponent<SoundManager>();
            DontDestroyOnLoad(soundManager);
        }
        
        // Ensure SceneManager exists
        if (SceneManager.Instance == null)
        {
            GameObject sceneManager = new GameObject("SceneManager");
            sceneManager.AddComponent<SceneManager>();
            DontDestroyOnLoad(sceneManager);
        }
    }
    
    Canvas SetupCanvas()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        // Add CanvasScaler
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(800, 600);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        
        // Add GraphicRaycaster
        canvasGO.AddComponent<GraphicRaycaster>();
        
        return canvas;
    }
    
    GameObject CreateMenuPanel(Transform canvasTransform)
    {
        // Create main menu panel
        GameObject menuPanel = new GameObject("MenuPanel");
        menuPanel.transform.SetParent(canvasTransform);
        
        RectTransform panelRect = menuPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add background image
        Image panelImage = menuPanel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.2f, 1f);
        
        // Create title text
        CreateTitleText(menuPanel.transform);
        
        // Create menu buttons
        CreateMenuButtons(menuPanel.transform);
        
        return menuPanel;
    }
    
    void CreateTitleText(Transform parent)
    {
        GameObject titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(parent);
        
        RectTransform rect = titleGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 150);
        rect.sizeDelta = new Vector2(400, 80);
        
        Text titleText = titleGO.AddComponent<Text>();
        titleText.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 48;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        titleText.text = "TIC TAC TOE";
    }
    
    void CreateMenuButtons(Transform parent)
    {
        // Create Play Button
        CreateMenuButton(parent, "PlayButton", "PLAY", new Vector2(0, 20));
        
        // Create Quit Button
        CreateMenuButton(parent, "QuitButton", "QUIT", new Vector2(0, -60));
    }
    
    void CreateMenuButton(Transform parent, string name, string text, Vector2 position)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(parent);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(200, 60);
        
        // Add Button and Image
        Button button = buttonGO.AddComponent<Button>();
        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        buttonImage.type = Image.Type.Sliced;
        
        // Configure button colors
        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.96f, 0.96f, 0.96f);
        colors.pressedColor = new Color(0.78f, 0.78f, 0.78f);
        button.colors = colors;
        
        // Create button text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        Text buttonText = textGO.AddComponent<Text>();
        buttonText.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 24;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        buttonText.text = text;
    }
    
    void SetupUIReferences(GameObject menuPanel)
    {
        // Create or find UI Manager
        GameObject uiManager = GameObject.Find("UIManager");
        if (uiManager == null)
        {
            uiManager = new GameObject("UIManager");
        }
        
        // Add UIManager script if not present
        UIManager uiScript = uiManager.GetComponent<UIManager>();
        if (uiScript == null)
        {
            uiScript = uiManager.AddComponent<UIManager>();
        }
        
        // Wire up references
        WireUIReferences(uiScript, menuPanel);
    }
    
    void WireUIReferences(UIManager uiScript, GameObject menuPanel)
    {
        // Set menu panel reference
        uiScript.menuPanel = menuPanel;
        
        // Find and assign buttons
        Button[] buttons = menuPanel.GetComponentsInChildren<Button>();
        
        foreach (Button button in buttons)
        {
            if (button.name == "PlayButton")
            {
                uiScript.playButton = button;
                // Wire up the button event
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(uiScript.StartGame);
            }
            else if (button.name == "QuitButton")
            {
                uiScript.quitButton = button;
                // Wire up the button event
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(uiScript.QuitGame);
            }
        }
        
        // Find title text
        Text titleText = menuPanel.GetComponentInChildren<Text>();
        if (titleText != null && titleText.name == "TitleText")
        {
            uiScript.titleText = titleText;
        }
        
        Debug.Log($"✅ UI Manager references configured: Play button: {uiScript.playButton != null}, Quit button: {uiScript.quitButton != null}");
    }
}