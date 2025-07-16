using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Complete scene setup script for Tic Tac Toe game
/// This script will create all missing components and properly configure the game
/// </summary>
public class CompleteSceneSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    [Tooltip("Click this in the inspector to automatically fix the entire game")]
    public bool setupCompleteGame = false;
    
    [Header("Scene References")]
    public Canvas gameCanvas;
    public GameObject gamePanel;
    public GameObject gameBoard;
    public TMP_Text statusText;
    
    void Start()
    {
        if (setupCompleteGame)
        {
            FixCompleteGame();
        }
    }
    
    #if UNITY_EDITOR
    [ContextMenu("Fix Complete Game Setup")]
    #endif
    public void FixCompleteGame()
    {
        Debug.Log("🔧 Starting complete game setup...");
        
        // Step 1: Find or create Canvas
        gameCanvas = FindOrCreateCanvas();
        
        // Step 2: Find or create game components
        FindSceneComponents();
        
        // Step 3: Create missing managers
        CreateGameManagers();
        
        // Step 4: Fix the game board buttons
        FixGameBoardButtons();
        
        // Step 5: Create restart button
        CreateRestartButton();
        
        // Step 6: Wire up all references
        WireUpGameReferences();
        
        Debug.Log("✅ Complete game setup finished!");
        Debug.Log("🎮 The Tic Tac Toe game is now fully functional!");
        Debug.Log("🎯 Press Play to test the game!");
    }
    
    private Canvas FindOrCreateCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasGO.AddComponent<GraphicRaycaster>();
            
            Debug.Log("✅ Canvas created");
        }
        return canvas;
    }
    
    private void FindSceneComponents()
    {
        // Find GamePanel
        if (gamePanel == null)
        {
            gamePanel = GameObject.Find("GamePanel");
        }
        
        // Find GameBoard
        if (gameBoard == null)
        {
            gameBoard = GameObject.Find("GameBoard");
        }
        
        // Find StatusText
        if (statusText == null)
        {
            GameObject statusGO = GameObject.Find("StatusText");
            if (statusGO != null)
            {
                statusText = statusGO.GetComponent<TMP_Text>();
            }
        }
        
        Debug.Log("✅ Scene components located");
    }
    
    private void CreateGameManagers()
    {
        // Create GameManager if it doesn't exist
        TicTacToeGame gameManager = FindObjectOfType<TicTacToeGame>();
        if (gameManager == null)
        {
            GameObject gameManagerGO = new GameObject("GameManager");
            gameManager = gameManagerGO.AddComponent<TicTacToeGame>();
            Debug.Log("✅ Game Manager created");
        }
        
        // Create UIManager if it doesn't exist
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            GameObject uiManagerGO = new GameObject("UIManager");
            uiManager = uiManagerGO.AddComponent<UIManager>();
            Debug.Log("✅ UI Manager created");
        }
        
        // Create SoundManager if it doesn't exist
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            GameObject soundManagerGO = new GameObject("SoundManager");
            soundManager = soundManagerGO.AddComponent<SoundManager>();
            Debug.Log("✅ Sound Manager created");
        }
        
        // Create EventSystem if it doesn't exist
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            Debug.Log("✅ Event System created");
        }
    }
    
    private void FixGameBoardButtons()
    {
        if (gameBoard == null)
        {
            Debug.LogError("❌ GameBoard not found! Cannot create buttons.");
            return;
        }
        
        // Ensure GridLayoutGroup is properly configured
        GridLayoutGroup grid = gameBoard.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = gameBoard.AddComponent<GridLayoutGroup>();
        }
        
        grid.cellSize = new Vector2(140, 140);
        grid.spacing = new Vector2(5, 5);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
        grid.childAlignment = TextAnchor.MiddleCenter;
        
        // Remove any existing children first
        for (int i = gameBoard.transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(gameBoard.transform.GetChild(i).gameObject);
            }
            else
            {
                #if UNITY_EDITOR
                DestroyImmediate(gameBoard.transform.GetChild(i).gameObject);
                #endif
            }
        }
        
        // Create 9 properly configured buttons
        for (int i = 0; i < 9; i++)
        {
            CreateGameButton(i);
        }
        
        Debug.Log("✅ All 9 game buttons created and configured");
    }
    
    private void CreateGameButton(int index)
    {
        GameObject buttonGO = new GameObject($"Button{index}");
        buttonGO.transform.SetParent(gameBoard.transform);
        buttonGO.layer = 5; // UI layer
        
        // Add RectTransform
        RectTransform rect = buttonGO.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(140, 140);
        
        // Add Image component
        Image buttonImage = buttonGO.AddComponent<Image>();
        buttonImage.color = Color.white;
        buttonImage.sprite = CreateButtonSprite();
        
        // Add Button component
        Button button = buttonGO.AddComponent<Button>();
        button.targetGraphic = buttonImage;
        
        // Create TextMeshPro Text child
        GameObject textGO = new GameObject("Text (TMP)");
        textGO.transform.SetParent(buttonGO.transform);
        textGO.layer = 5; // UI layer
        
        RectTransform textRect = textGO.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        TMP_Text buttonText = textGO.AddComponent<TextMeshProUGUI>();
        buttonText.text = "";
        buttonText.fontSize = 48;
        buttonText.fontStyle = FontStyles.Bold;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.color = Color.black;
        
        Debug.Log($"✅ Button {index} created with TextMeshPro");
    }
    
    private Sprite CreateButtonSprite()
    {
        // Create a simple white square sprite for buttons
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    
    private void CreateRestartButton()
    {
        if (gamePanel == null)
        {
            Debug.LogError("❌ GamePanel not found! Cannot create restart button.");
            return;
        }
        
        // Check if restart button already exists
        Transform existingRestart = gamePanel.transform.Find("RestartButton");
        if (existingRestart != null)
        {
            Debug.Log("✅ Restart button already exists");
            return;
        }
        
        // Create restart button
        GameObject restartGO = new GameObject("RestartButton");
        restartGO.transform.SetParent(gamePanel.transform);
        restartGO.layer = 5; // UI layer
        
        RectTransform restartRect = restartGO.AddComponent<RectTransform>();
        restartRect.anchorMin = new Vector2(0.5f, 0.1f);
        restartRect.anchorMax = new Vector2(0.5f, 0.2f);
        restartRect.sizeDelta = new Vector2(200, 60);
        restartRect.anchoredPosition = Vector2.zero;
        
        Image restartImage = restartGO.AddComponent<Image>();
        restartImage.color = new Color(0.2f, 0.6f, 1f, 1f);
        
        Button restartButton = restartGO.AddComponent<Button>();
        restartButton.targetGraphic = restartImage;
        
        // Create TextMeshPro Text child
        GameObject restartTextGO = new GameObject("Text (TMP)");
        restartTextGO.transform.SetParent(restartGO.transform);
        restartTextGO.layer = 5; // UI layer
        
        RectTransform restartTextRect = restartTextGO.AddComponent<RectTransform>();
        restartTextRect.anchorMin = Vector2.zero;
        restartTextRect.anchorMax = Vector2.one;
        restartTextRect.sizeDelta = Vector2.zero;
        restartTextRect.anchoredPosition = Vector2.zero;
        
        TMP_Text restartText = restartTextGO.AddComponent<TextMeshProUGUI>();
        restartText.text = "Restart";
        restartText.fontSize = 24;
        restartText.fontStyle = FontStyles.Bold;
        restartText.alignment = TextAlignmentOptions.Center;
        restartText.color = Color.white;
        
        Debug.Log("✅ Restart button created with TextMeshPro");
    }
    
    private void WireUpGameReferences()
    {
        // Find the game manager
        TicTacToeGame gameManager = FindObjectOfType<TicTacToeGame>();
        if (gameManager == null)
        {
            Debug.LogError("❌ Game Manager not found!");
            return;
        }
        
        // Wire up buttons
        if (gameBoard != null)
        {
            Button[] buttons = new Button[9];
            for (int i = 0; i < 9; i++)
            {
                Transform buttonTransform = gameBoard.transform.Find($"Button{i}");
                if (buttonTransform != null)
                {
                    buttons[i] = buttonTransform.GetComponent<Button>();
                }
            }
            gameManager.buttons = buttons;
            Debug.Log("✅ Game buttons wired up");
        }
        
        // Wire up status text
        if (statusText != null)
        {
            gameManager.statusText = statusText;
            Debug.Log("✅ Status text wired up");
        }
        
        // Wire up restart button
        if (gamePanel != null)
        {
            Transform restartTransform = gamePanel.transform.Find("RestartButton");
            if (restartTransform != null)
            {
                gameManager.restartButton = restartTransform.GetComponent<Button>();
                Debug.Log("✅ Restart button wired up");
            }
        }
        
        // Wire up UI Manager
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            gameManager.uiManager = uiManager;
            Debug.Log("✅ UI Manager wired up");
        }
        
        Debug.Log("✅ All game references wired up successfully!");
    }
}