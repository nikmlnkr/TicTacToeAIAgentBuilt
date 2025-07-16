using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Automatically sets up the complete tic-tac-toe game when the scene loads
/// This script creates all missing components and properly wires everything together
/// </summary>
public class AutoGameSetup : MonoBehaviour
{
    [Header("Game Components")]
    public TicTacToeGame gameManager;
    public Transform gameBoard;
    public Button restartButton;
    
    void Start()
    {
        SetupCompleteGame();
    }
    
    public void SetupCompleteGame()
    {
        Debug.Log("🎮 Setting up complete Tic Tac Toe game...");
        
        // Find components if not assigned
        FindGameComponents();
        
        // Create the 9 game buttons
        CreateGameButtons();
        
        // Wire up the restart button
        SetupRestartButton();
        
        Debug.Log("✅ Tic Tac Toe game setup complete!");
        Debug.Log("🎯 Game is ready to play!");
    }
    
    void FindGameComponents()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<TicTacToeGame>();
            
        if (gameBoard == null)
            gameBoard = GameObject.Find("GameBoard")?.transform;
            
        if (restartButton == null)
            restartButton = GameObject.Find("RestartButton")?.GetComponent<Button>();
    }
    
    void CreateGameButtons()
    {
        if (gameBoard == null)
        {
            Debug.LogError("GameBoard not found!");
            return;
        }
        
        // Clear existing buttons
        foreach (Transform child in gameBoard)
        {
            DestroyImmediate(child.gameObject);
        }
        
        // Create 9 buttons for the tic-tac-toe grid
        Button[] buttons = new Button[9];
        
        for (int i = 0; i < 9; i++)
        {
            // Create button GameObject
            GameObject buttonObj = new GameObject($"Button{i}");
            buttonObj.transform.SetParent(gameBoard);
            buttonObj.layer = 5; // UI layer
            
            // Add RectTransform
            RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
            
            // Add Image component
            Image buttonImage = buttonObj.AddComponent<Image>();
            buttonImage.color = Color.white;
            buttonImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
            
            // Add Button component
            Button button = buttonObj.AddComponent<Button>();
            button.targetGraphic = buttonImage;
            
            // Create TextMeshPro Text child for X/O display
            GameObject textObj = new GameObject("Text (TMP)");
            textObj.transform.SetParent(buttonObj.transform);
            textObj.layer = 5; // UI layer
            
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            textRect.localScale = Vector3.one;
            
            TMP_Text buttonText = textObj.AddComponent<TextMeshProUGUI>();
            buttonText.text = "";
            buttonText.fontSize = 48;
            buttonText.fontStyle = FontStyles.Bold;
            buttonText.alignment = TextAlignmentOptions.Center;
            buttonText.color = Color.black;
            
            // Store button reference
            buttons[i] = button;
            
            // Add click listener
            int buttonIndex = i; // Capture for closure
            button.onClick.AddListener(() => gameManager.OnCellClicked(buttonIndex));
        }
        
        // Assign buttons to game manager
        if (gameManager != null)
        {
            gameManager.buttons = buttons;
            Debug.Log($"✅ Created {buttons.Length} game buttons and assigned to TicTacToeGame");
        }
    }
    
    void SetupRestartButton()
    {
        if (restartButton != null && gameManager != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() => gameManager.RestartGame());
            Debug.Log("✅ Restart button configured");
        }
    }
}