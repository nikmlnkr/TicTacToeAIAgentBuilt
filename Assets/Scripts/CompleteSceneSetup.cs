using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Automatically sets up a complete TicTacToe game scene at runtime
/// This ensures the game is always playable regardless of scene state
/// </summary>
public class CompleteSceneSetup : MonoBehaviour
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
            SetupCompleteScene();
        }
    }
    
    [ContextMenu("Setup Complete TicTacToe Scene")]
    public void SetupCompleteScene()
    {
        Debug.Log("Setting up complete TicTacToe game scene...");
        
        // Clear existing UI if requested
        if (clearExistingUI)
        {
            CleanupExistingUI();
        }
        
        // Ensure we have managers
        SetupManagers();
        
        // Create or find Canvas
        Canvas canvas = SetupCanvas();
        
        // Create Game Panel with complete UI
        GameObject gamePanel = CreateGamePanel(canvas.transform);
        
        // Setup game components
        SetupGameReferences(gamePanel);
        
        Debug.Log("✅ Complete TicTacToe scene setup finished!");
    }
    
    void CleanupExistingUI()
    {
        // Find and destroy existing UI elements that might be incomplete
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
    
    GameObject CreateGamePanel(Transform canvasTransform)
    {
        // Create main game panel
        GameObject gamePanel = new GameObject("GamePanel");
        gamePanel.transform.SetParent(canvasTransform);
        
        RectTransform panelRect = gamePanel.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add background image
        Image panelImage = gamePanel.AddComponent<Image>();
        panelImage.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        // Create status text
        CreateStatusText(gamePanel.transform);
        
        // Create game board with buttons
        CreateGameBoard(gamePanel.transform);
        
        // Create control buttons
        CreateControlButtons(gamePanel.transform);
        
        return gamePanel;
    }
    
    void CreateStatusText(Transform parent)
    {
        GameObject statusTextGO = new GameObject("StatusText");
        statusTextGO.transform.SetParent(parent);
        
        RectTransform rect = statusTextGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(0, 200);
        rect.sizeDelta = new Vector2(400, 50);
        
        Text statusText = statusTextGO.AddComponent<Text>();
        statusText.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        statusText.fontSize = 24;
        statusText.fontStyle = FontStyle.Bold;
        statusText.alignment = TextAnchor.MiddleCenter;
        statusText.color = Color.white;
        statusText.text = "Player X's Turn";
    }
    
    void CreateGameBoard(Transform parent)
    {
        GameObject gameBoardGO = new GameObject("GameBoard");
        gameBoardGO.transform.SetParent(parent);
        
        RectTransform boardRect = gameBoardGO.AddComponent<RectTransform>();
        boardRect.anchorMin = new Vector2(0.5f, 0.5f);
        boardRect.anchorMax = new Vector2(0.5f, 0.5f);
        boardRect.anchoredPosition = Vector2.zero;
        boardRect.sizeDelta = new Vector2(450, 450);
        
        // Add Grid Layout Group
        GridLayoutGroup gridLayout = gameBoardGO.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(140, 140);
        gridLayout.spacing = new Vector2(5, 5);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = 3;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        
        // Create 9 game buttons
        for (int i = 0; i < 9; i++)
        {
            CreateGameButton(gameBoardGO.transform, i);
        }
    }
    
    void CreateGameButton(Transform parent, int index)
    {
        GameObject buttonGO = new GameObject($"Button{index}");
        buttonGO.transform.SetParent(parent);
        
        // Add Button component
        Button button = buttonGO.AddComponent<Button>();
        
        // Add Image component
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
        buttonText.fontSize = 48;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        buttonText.text = "";
    }
    
    void CreateControlButtons(Transform parent)
    {
        // Create restart button
        CreateControlButton(parent, "RestartButton", "RESTART", new Vector2(-100, -200));
        
        // Create back to menu button
        CreateControlButton(parent, "BackToMenuButton", "MENU", new Vector2(100, -200));
    }
    
    void CreateControlButton(Transform parent, string name, string text, Vector2 position)
    {
        GameObject buttonGO = new GameObject(name);
        buttonGO.transform.SetParent(parent);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(150, 40);
        
        // Add Button and Image
        Button button = buttonGO.AddComponent<Button>();
        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        buttonImage.type = Image.Type.Sliced;
        
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
        buttonText.fontSize = 16;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        buttonText.text = text;
    }
    
    void SetupGameReferences(GameObject gamePanel)
    {
        // Create or find Game Manager
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager == null)
        {
            gameManager = new GameObject("GameManager");
        }
        
        // Add TicTacToeGame script if not present
        TicTacToeGame gameScript = gameManager.GetComponent<TicTacToeGame>();
        if (gameScript == null)
        {
            gameScript = gameManager.AddComponent<TicTacToeGame>();
        }
        
        // Add UIManager script if not present
        UIManager uiManager = gameManager.GetComponent<UIManager>();
        if (uiManager == null)
        {
            uiManager = gameManager.AddComponent<UIManager>();
        }
        
        // Wire up all references
        WireGameReferences(gameScript, uiManager, gamePanel);
    }
    
    void WireGameReferences(TicTacToeGame gameScript, UIManager uiManager, GameObject gamePanel)
    {
        // Find and assign all buttons
        Button[] allButtons = gamePanel.GetComponentsInChildren<Button>();
        
        // Assign game buttons (first 9 buttons)
        gameScript.buttons = new Button[9];
        int buttonIndex = 0;
        
        foreach (Button button in allButtons)
        {
            if (button.name.StartsWith("Button") && buttonIndex < 9)
            {
                gameScript.buttons[buttonIndex] = button;
                buttonIndex++;
            }
            else if (button.name == "RestartButton")
            {
                gameScript.restartButton = button;
            }
            else if (button.name == "BackToMenuButton")
            {
                gameScript.backToMenuButton = button;
            }
        }
        
        // Assign status text
        Text statusText = gamePanel.GetComponentInChildren<Text>();
        if (statusText != null && statusText.name == "StatusText")
        {
            gameScript.statusText = statusText;
        }
        
        // Assign UI Manager reference
        gameScript.uiManager = uiManager;
        
        // Configure UI Manager
        uiManager.gamePanel = gamePanel;
        
        Debug.Log($"✅ Game references configured: {gameScript.buttons.Length} buttons, status text: {gameScript.statusText != null}, restart: {gameScript.restartButton != null}, back: {gameScript.backToMenuButton != null}");
    }
}