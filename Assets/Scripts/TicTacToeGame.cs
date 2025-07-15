using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGame : MonoBehaviour
{
    [Header("Game Settings")]
    public Button[] buttons = new Button[9];
    public Text statusText;
    public Button restartButton;
    
    [Header("UI Manager Reference")]
    public UIManager uiManager;
    
    [Header("Game State")]
    private string currentPlayer = "X";
    private string[] board = new string[9];
    private bool gameActive = true;
    
    // Winning combinations
    private int[,] winningCombinations = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
        {0, 4, 8}, {2, 4, 6}             // Diagonals
    };
    
    void Start()
    {
        InitializeGame();
        SetupButtons();
    }
    
    void InitializeGame()
    {
        currentPlayer = "X";
        gameActive = true;
        
        // Clear the board
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = "";
        }
        
        // Clear button texts and reset colors
        foreach (Button button in buttons)
        {
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = "";
            button.interactable = true;
            
            // Reset button color
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.color = Color.white;
        }
        
        UpdateStatusText("Player X's Turn");
        
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(RestartGame);
        }
    }
    
    void SetupButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the index for closure
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnCellClicked(index));
        }
    }
    
    public void OnCellClicked(int index)
    {
        if (!gameActive || !string.IsNullOrEmpty(board[index]))
            return;
        
        // Play button click sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        
        // Animate button press
        if (uiManager != null)
            uiManager.AnimateButtonPress(buttons[index]);
        
        // Make the move
        board[index] = currentPlayer;
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        if (buttonText != null)
            buttonText.text = currentPlayer;
        buttons[index].interactable = false;
        
        // Play move sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMove();
        
        // Check for win
        if (CheckWin())
        {
            UpdateStatusText("Player " + currentPlayer + " Wins!");
            gameActive = false;
            DisableAllButtons();
            
            // Play win sound
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayWin();
            return;
        }
        
        // Check for draw
        if (CheckDraw())
        {
            UpdateStatusText("It's a Draw!");
            gameActive = false;
            
            // Play draw sound
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayDraw();
            return;
        }
        
        // Switch player
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
        UpdateStatusText("Player " + currentPlayer + "'s Turn");
    }
    
    bool CheckWin()
    {
        for (int i = 0; i < winningCombinations.GetLength(0); i++)
        {
            int a = winningCombinations[i, 0];
            int b = winningCombinations[i, 1];
            int c = winningCombinations[i, 2];
            
            if (!string.IsNullOrEmpty(board[a]) && 
                board[a] == board[b] && 
                board[b] == board[c])
            {
                HighlightWinningCells(a, b, c);
                return true;
            }
        }
        return false;
    }
    
    bool CheckDraw()
    {
        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell))
                return false;
        }
        return true;
    }
    
    void HighlightWinningCells(int a, int b, int c)
    {
        if (a < buttons.Length && buttons[a] != null)
        {
            Image imageA = buttons[a].GetComponent<Image>();
            if (imageA != null) imageA.color = Color.green;
        }
        
        if (b < buttons.Length && buttons[b] != null)
        {
            Image imageB = buttons[b].GetComponent<Image>();
            if (imageB != null) imageB.color = Color.green;
        }
        
        if (c < buttons.Length && buttons[c] != null)
        {
            Image imageC = buttons[c].GetComponent<Image>();
            if (imageC != null) imageC.color = Color.green;
        }
    }
    
    void DisableAllButtons()
    {
        foreach (Button button in buttons)
        {
            if (button != null)
                button.interactable = false;
        }
    }
    
    void UpdateStatusText(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
    
    public void RestartGame()
    {
        // Play button click sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        
        // Animate restart button
        if (uiManager != null && restartButton != null)
            uiManager.AnimateButtonPress(restartButton);
        
        // Reset button colors
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                    buttonImage.color = Color.white;
            }
        }
        
        InitializeGame();
    }
}