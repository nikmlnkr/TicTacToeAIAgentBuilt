using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGame : MonoBehaviour
{
    [Header("Game Settings")]
    public Button[] buttons = new Button[9];
    public Text statusText;
    public Button restartButton;
    
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
        
        // Clear button texts
        foreach (Button button in buttons)
        {
            button.GetComponentInChildren<Text>().text = "";
            button.interactable = true;
        }
        
        UpdateStatusText("Player X's Turn");
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }
    
    void SetupButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Capture the index for closure
            buttons[i].onClick.AddListener(() => OnCellClicked(index));
        }
    }
    
    public void OnCellClicked(int index)
    {
        if (!gameActive || !string.IsNullOrEmpty(board[index]))
            return;
        
        // Make the move
        board[index] = currentPlayer;
        buttons[index].GetComponentInChildren<Text>().text = currentPlayer;
        buttons[index].interactable = false;
        
        // Check for win
        if (CheckWin())
        {
            UpdateStatusText("Player " + currentPlayer + " Wins!");
            gameActive = false;
            DisableAllButtons();
            return;
        }
        
        // Check for draw
        if (CheckDraw())
        {
            UpdateStatusText("It's a Draw!");
            gameActive = false;
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
        buttons[a].GetComponent<Image>().color = Color.green;
        buttons[b].GetComponent<Image>().color = Color.green;
        buttons[c].GetComponent<Image>().color = Color.green;
    }
    
    void DisableAllButtons()
    {
        foreach (Button button in buttons)
        {
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
        // Reset button colors
        foreach (Button button in buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }
        
        InitializeGame();
    }
}