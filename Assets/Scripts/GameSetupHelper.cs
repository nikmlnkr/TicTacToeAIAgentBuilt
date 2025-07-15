using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helper script to automatically set up the Tic Tac Toe game scene
/// This can be run in the Unity Editor to create a complete game setup
/// </summary>
public class GameSetupHelper : MonoBehaviour
{
    [Header("Setup Configuration")]
    [Tooltip("Automatically set up the complete game scene")]
    public bool autoSetupScene = false;
    
    [Header("UI Configuration")]
    public Font gameFont;
    public Color buttonColor = Color.white;
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.2f, 1f);
    
    void Start()
    {
        if (autoSetupScene)
        {
            SetupCompleteGame();
        }
    }
    
    [ContextMenu("Setup Complete Game Scene")]
    public void SetupCompleteGame()
    {
        Debug.Log("Setting up Tic Tac Toe game scene...");
        
        // Create Canvas if it doesn't exist
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            canvas = CreateCanvas();
        }
        
        // Create Game Manager
        GameObject gameManager = CreateGameManager();
        
        // Create UI Manager
        GameObject uiManager = CreateUIManager();
        
        // Create Sound Manager
        GameObject soundManager = CreateSoundManager();
        
        // Create Game Panel
        GameObject gamePanel = CreateGamePanel(canvas.transform);
        
        // Create Menu Panel
        GameObject menuPanel = CreateMenuPanel(canvas.transform);
        
        // Setup Game Manager References
        SetupGameManagerReferences(gameManager, gamePanel, uiManager);
        
        // Setup UI Manager References
        SetupUIManagerReferences(uiManager, gamePanel, menuPanel);
        
        Debug.Log("✅ Game scene setup complete!");
        Debug.Log("🎮 The game is now ready to play!");
    }
    
    private Canvas CreateCanvas()
    {
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Create EventSystem if it doesn't exist
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
        
        Debug.Log("✅ Canvas created");
        return canvas;
    }
    
    private GameObject CreateGameManager()
    {
        GameObject gameManager = new GameObject("GameManager");
        gameManager.AddComponent<TicTacToeGame>();
        Debug.Log("✅ Game Manager created");
        return gameManager;
    }
    
    private GameObject CreateUIManager()
    {
        GameObject uiManager = new GameObject("UIManager");
        uiManager.AddComponent<UIManager>();
        Debug.Log("✅ UI Manager created");
        return uiManager;
    }
    
    private GameObject CreateSoundManager()
    {
        GameObject soundManager = new GameObject("SoundManager");
        soundManager.AddComponent<SoundManager>();
        Debug.Log("✅ Sound Manager created");
        return soundManager;
    }
    
    private GameObject CreateGamePanel(Transform canvasTransform)
    {
        // Create Game Panel
        GameObject gamePanel = new GameObject("GamePanel");
        gamePanel.transform.SetParent(canvasTransform);
        
        RectTransform gamePanelRect = gamePanel.AddComponent<RectTransform>();
        gamePanelRect.anchorMin = Vector2.zero;
        gamePanelRect.anchorMax = Vector2.one;
        gamePanelRect.sizeDelta = Vector2.zero;
        gamePanelRect.anchoredPosition = Vector2.zero;
        
        Image gamePanelImage = gamePanel.AddComponent<Image>();
        gamePanelImage.color = backgroundColor;
        
        // Create Status Text
        GameObject statusText = CreateStatusText(gamePanel.transform);
        
        // Create Game Board
        GameObject gameBoard = CreateGameBoard(gamePanel.transform);
        
        // Create Restart Button
        GameObject restartButton = CreateRestartButton(gamePanel.transform);
        
        Debug.Log("✅ Game Panel created with all UI elements");
        return gamePanel;
    }
    
    private GameObject CreateStatusText(Transform parent)
    {
        GameObject statusTextGO = new GameObject("StatusText");
        statusTextGO.transform.SetParent(parent);
        
        RectTransform rect = statusTextGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.8f);
        rect.anchorMax = new Vector2(0.5f, 0.9f);
        rect.sizeDelta = new Vector2(600, 80);
        rect.anchoredPosition = Vector2.zero;
        
        Text text = statusTextGO.AddComponent<Text>();
        text.text = "Player X's Turn";
        text.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 36;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        
        return statusTextGO;
    }
    
    private GameObject CreateGameBoard(Transform parent)
    {
        GameObject gameBoard = new GameObject("GameBoard");
        gameBoard.transform.SetParent(parent);
        
        RectTransform boardRect = gameBoard.AddComponent<RectTransform>();
        boardRect.anchorMin = new Vector2(0.5f, 0.5f);
        boardRect.anchorMax = new Vector2(0.5f, 0.5f);
        boardRect.sizeDelta = new Vector2(450, 450);
        boardRect.anchoredPosition = new Vector2(0, -50);
        
        GridLayoutGroup grid = gameBoard.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(140, 140);
        grid.spacing = new Vector2(5, 5);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
        grid.childAlignment = TextAnchor.MiddleCenter;
        
        // Create 9 buttons for the game board
        for (int i = 0; i < 9; i++)
        {
            CreateGameButton(gameBoard.transform, i);
        }
        
        return gameBoard;
    }
    
    private GameObject CreateGameButton(Transform parent, int index)
    {
        GameObject buttonGO = new GameObject($"Button{index}");
        buttonGO.transform.SetParent(parent);
        
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(140, 140);
        
        Button button = buttonGO.AddComponent<Button>();
        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = buttonColor;
        
        // Create button text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        Text buttonText = textGO.AddComponent<Text>();
        buttonText.text = "";
        buttonText.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 72;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        
        return buttonGO;
    }
    
    private GameObject CreateRestartButton(Transform parent)
    {
        GameObject restartButtonGO = new GameObject("RestartButton");
        restartButtonGO.transform.SetParent(parent);
        
        RectTransform rect = restartButtonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.1f);
        rect.anchorMax = new Vector2(0.5f, 0.1f);
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchoredPosition = Vector2.zero;
        
        Button button = restartButtonGO.AddComponent<Button>();
        Image buttonImage = restartButtonGO.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 0.2f, 1f); // Green color
        
        // Create button text
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(restartButtonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        Text buttonText = textGO.AddComponent<Text>();
        buttonText.text = "Restart";
        buttonText.font = gameFont ? gameFont : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 24;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        
        return restartButtonGO;
    }
    
    private GameObject CreateMenuPanel(Transform canvasTransform)
    {
        GameObject menuPanel = new GameObject("MenuPanel");
        menuPanel.transform.SetParent(canvasTransform);
        
        RectTransform rect = menuPanel.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        
        Image image = menuPanel.AddComponent<Image>();
        image.color = new Color(0.05f, 0.05f, 0.1f, 1f);
        
        // Initially disable menu panel (game panel will be active)
        menuPanel.SetActive(false);
        
        Debug.Log("✅ Menu Panel created");
        return menuPanel;
    }
    
    private void SetupGameManagerReferences(GameObject gameManager, GameObject gamePanel, GameObject uiManager)
    {
        TicTacToeGame gameScript = gameManager.GetComponent<TicTacToeGame>();
        
        // Find and assign buttons
        Button[] buttons = gamePanel.GetComponentsInChildren<Button>();
        System.Array.Resize(ref gameScript.buttons, 9);
        
        int buttonIndex = 0;
        foreach (Button button in buttons)
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
        }
        
        // Assign status text
        Text statusText = gamePanel.GetComponentInChildren<Text>();
        if (statusText != null && statusText.name == "StatusText")
        {
            gameScript.statusText = statusText;
        }
        
        // Assign UI Manager
        gameScript.uiManager = uiManager.GetComponent<UIManager>();
        
        Debug.Log("✅ Game Manager references configured");
    }
    
    private void SetupUIManagerReferences(GameObject uiManager, GameObject gamePanel, GameObject menuPanel)
    {
        UIManager uiScript = uiManager.GetComponent<UIManager>();
        
        uiScript.gamePanel = gamePanel;
        uiScript.menuPanel = menuPanel;
        
        // Find play and quit buttons in menu panel (if they exist)
        Button[] menuButtons = menuPanel.GetComponentsInChildren<Button>(true);
        foreach (Button button in menuButtons)
        {
            if (button.name.Contains("Play"))
                uiScript.playButton = button;
            else if (button.name.Contains("Quit"))
                uiScript.quitButton = button;
        }
        
        Debug.Log("✅ UI Manager references configured");
    }
}