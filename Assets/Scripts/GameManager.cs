using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform gridParent;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private TextMeshProUGUI currentPlayerText;
        [SerializeField] private Button newGameButton;
        [SerializeField] private TextMeshProUGUI scoreText;

        [Header("Game Settings")]
        [SerializeField] private float winAnimationDuration = 1f;

        private TicTacToeGame game = new TicTacToeGame();
        private TicTacToeCell[,] cells = new TicTacToeCell[3, 3];
        private int xWins = 0;
        private int oWins = 0;
        private int draws = 0;

        private void Start()
        {
            SetupUI();
            SetupGame();
        }

        private void SetupUI()
        {
            // Setup new game button
            if (newGameButton != null)
                newGameButton.onClick.AddListener(StartNewGame);

            // Create grid cells if they don't exist
            if (gridParent != null && cellPrefab != null)
            {
                // Clear existing cells
                for (int i = gridParent.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(gridParent.GetChild(i).gameObject);
                }

                // Create new cells
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        GameObject cellObj = Instantiate(cellPrefab, gridParent);
                        TicTacToeCell cell = cellObj.GetComponent<TicTacToeCell>();
                        if (cell == null)
                            cell = cellObj.AddComponent<TicTacToeCell>();
                        
                        cell.Initialize(row, col, this);
                        cells[row, col] = cell;
                    }
                }
            }
        }

        private void SetupGame()
        {
            // Subscribe to game events
            game.OnCellChanged += OnCellChanged;
            game.OnGameEnded += OnGameEnded;
            game.OnPlayerChanged += OnPlayerChanged;

            StartNewGame();
        }

        public void StartNewGame()
        {
            game.StartNewGame();
            
            // Reset all cell visuals
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (cells[row, col] != null)
                        cells[row, col].ResetCell();
                }
            }

            UpdateUI();
        }

        public void OnCellClicked(int row, int col)
        {
            game.MakeMove(row, col);
        }

        private void OnCellChanged(int row, int col, CellState state)
        {
            if (cells[row, col] != null)
            {
                cells[row, col].SetState(state);
            }
        }

        private void OnPlayerChanged(CellState currentPlayer)
        {
            UpdateUI();
        }

        private void OnGameEnded(GameState gameState, int[] winningLine)
        {
            // Update scores
            switch (gameState)
            {
                case GameState.PlayerXWins:
                    xWins++;
                    break;
                case GameState.PlayerOWins:
                    oWins++;
                    break;
                case GameState.Draw:
                    draws++;
                    break;
            }

            // Highlight winning line
            if (winningLine != null)
            {
                StartCoroutine(AnimateWinningLine(winningLine));
            }

            UpdateUI();
        }

        private IEnumerator AnimateWinningLine(int[] winningLine)
        {
            // Convert linear indices to row/col
            for (int i = 0; i < winningLine.Length; i++)
            {
                int index = winningLine[i];
                int row = index / 3;
                int col = index % 3;
                
                if (cells[row, col] != null)
                {
                    cells[row, col].HighlightAsWinning();
                }
            }

            yield return new WaitForSeconds(winAnimationDuration);
        }

        private void UpdateUI()
        {
            // Update current player text
            if (currentPlayerText != null)
            {
                if (game.State == GameState.Playing)
                {
                    currentPlayerText.text = $"Current Player: {game.CurrentPlayer}";
                }
                else
                {
                    currentPlayerText.text = "";
                }
            }

            // Update status text
            if (statusText != null)
            {
                switch (game.State)
                {
                    case GameState.Playing:
                        statusText.text = "Make your move!";
                        break;
                    case GameState.PlayerXWins:
                        statusText.text = "Player X Wins!";
                        break;
                    case GameState.PlayerOWins:
                        statusText.text = "Player O Wins!";
                        break;
                    case GameState.Draw:
                        statusText.text = "It's a Draw!";
                        break;
                }
            }

            // Update score text
            if (scoreText != null)
            {
                scoreText.text = $"X: {xWins} | O: {oWins} | Draws: {draws}";
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (game != null)
            {
                game.OnCellChanged -= OnCellChanged;
                game.OnGameEnded -= OnGameEnded;
                game.OnPlayerChanged -= OnPlayerChanged;
            }

            if (newGameButton != null)
                newGameButton.onClick.RemoveListener(StartNewGame);
        }
    }
}