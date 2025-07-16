using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public TMP_FontAsset gameFont;
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
        Canvas canvas = FindAnyObjectByType<Canvas>();
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
        if (FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
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
        
        TMP_Text text = statusTextGO.AddComponent<TextMeshProUGUI>();
        text.text = "Player X's Turn";
        text.font = gameFont;
        text.fontSize = 36;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
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
        
        // Create TextMeshPro Text child
        GameObject textGO = new GameObject("Text (TMP)");
        textGO.transform.SetParent(buttonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        TMP_Text text = textGO.AddComponent<TextMeshProUGUI>();
        text.text = "";
        text.font = gameFont;
        text.fontSize = 48;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.black;
        
        return buttonGO;
    }
    
    private GameObject CreateRestartButton(Transform parent)
    {
        GameObject restartButtonGO = new GameObject("RestartButton");
        restartButtonGO.transform.SetParent(parent);
        
        RectTransform rect = restartButtonGO.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.1f);
        rect.anchorMax = new Vector2(0.5f, 0.2f);
        rect.sizeDelta = new Vector2(200, 60);
        rect.anchoredPosition = Vector2.zero;
        
        Button button = restartButtonGO.AddComponent<Button>();
        Image buttonImage = restartButtonGO.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.6f, 1f, 1f);
        
        // Create TextMeshPro Text child
        GameObject textGO = new GameObject("Text (TMP)");
        textGO.transform.SetParent(restartButtonGO.transform);
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        TMP_Text text = textGO.AddComponent<TextMeshProUGUI>();
        text.text = "Restart";
        text.font = gameFont;
        text.fontSize = 24;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        
        return restartButtonGO;
    }
    
    private GameObject CreateMenuPanel(Transform canvasTransform)
    {
        GameObject menuPanel = new GameObject("MenuPanel");
        menuPanel.transform.SetParent(canvasTransform);
        
        RectTransform menuPanelRect = menuPanel.AddComponent<RectTransform>();
        menuPanelRect.anchorMin = Vector2.zero;
        menuPanelRect.anchorMax = Vector2.one;
        menuPanelRect.sizeDelta = Vector2.zero;
        menuPanelRect.anchoredPosition = Vector2.zero;
        
        Image menuPanelImage = menuPanel.AddComponent<Image>();
        menuPanelImage.color = backgroundColor;
        
        // Create title text
        GameObject titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(menuPanel.transform);
        
        RectTransform titleRect = titleGO.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.8f);
        titleRect.sizeDelta = new Vector2(600, 100);
        titleRect.anchoredPosition = Vector2.zero;
        
        TMP_Text titleText = titleGO.AddComponent<TextMeshProUGUI>();
        titleText.text = "Tic Tac Toe";
        titleText.font = gameFont;
        titleText.fontSize = 72;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.white;
        
        // Create play button
        GameObject playButtonGO = new GameObject("PlayButton");
        playButtonGO.transform.SetParent(menuPanel.transform);
        
        RectTransform playRect = playButtonGO.AddComponent<RectTransform>();
        playRect.anchorMin = new Vector2(0.5f, 0.4f);
        playRect.anchorMax = new Vector2(0.5f, 0.5f);
        playRect.sizeDelta = new Vector2(200, 60);
        playRect.anchoredPosition = Vector2.zero;
        
        Button playButton = playButtonGO.AddComponent<Button>();
        Image playButtonImage = playButtonGO.AddComponent<Image>();
        playButtonImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);
        
        // Create play button text
        GameObject playTextGO = new GameObject("Text (TMP)");
        playTextGO.transform.SetParent(playButtonGO.transform);
        
        RectTransform playTextRect = playTextGO.AddComponent<RectTransform>();
        playTextRect.anchorMin = Vector2.zero;
        playTextRect.anchorMax = Vector2.one;
        playTextRect.sizeDelta = Vector2.zero;
        playTextRect.anchoredPosition = Vector2.zero;
        
        TMP_Text playText = playTextGO.AddComponent<TextMeshProUGUI>();
        playText.text = "Play";
        playText.font = gameFont;
        playText.fontSize = 24;
        playText.fontStyle = FontStyles.Bold;
        playText.alignment = TextAlignmentOptions.Center;
        playText.color = Color.white;
        
        return menuPanel;
    }
    
    private void SetupGameManagerReferences(GameObject gameManager, GameObject gamePanel, GameObject uiManager)
    {
        TicTacToeGame gameScript = gameManager.GetComponent<TicTacToeGame>();
        if (gameScript != null)
        {
            // Find buttons in the game board
            Transform gameBoard = gamePanel.transform.Find("GameBoard");
            if (gameBoard != null)
            {
                Button[] buttons = new Button[9];
                for (int i = 0; i < 9; i++)
                {
                    Transform buttonTransform = gameBoard.Find($"Button{i}");
                    if (buttonTransform != null)
                    {
                        buttons[i] = buttonTransform.GetComponent<Button>();
                    }
                }
                gameScript.buttons = buttons;
            }
            
            // Find status text
            Transform statusTextTransform = gamePanel.transform.Find("StatusText");
            if (statusTextTransform != null)
            {
                gameScript.statusText = statusTextTransform.GetComponent<TMP_Text>();
            }
            
            // Find restart button
            Transform restartButtonTransform = gamePanel.transform.Find("RestartButton");
            if (restartButtonTransform != null)
            {
                gameScript.restartButton = restartButtonTransform.GetComponent<Button>();
            }
            
            // Assign UI manager
            if (uiManager != null)
            {
                gameScript.uiManager = uiManager.GetComponent<UIManager>();
            }
        }
    }
    
    private void SetupUIManagerReferences(GameObject uiManager, GameObject gamePanel, GameObject menuPanel)
    {
        UIManager uiScript = uiManager.GetComponent<UIManager>();
        if (uiScript != null)
        {
            uiScript.gamePanel = gamePanel;
            uiScript.menuPanel = menuPanel;
            
            // Find play button in menu
            if (menuPanel != null)
            {
                Transform playButtonTransform = menuPanel.transform.Find("PlayButton");
                if (playButtonTransform != null)
                {
                    uiScript.playButton = playButtonTransform.GetComponent<Button>();
                }
            }
            
            // Find title text in menu
            if (menuPanel != null)
            {
                Transform titleTextTransform = menuPanel.transform.Find("TitleText");
                if (titleTextTransform != null)
                {
                    uiScript.titleText = titleTextTransform.GetComponent<TMP_Text>();
                }
            }
        }
    }
}