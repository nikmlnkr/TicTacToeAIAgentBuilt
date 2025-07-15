using System;
using UnityEngine;

namespace TicTacToe
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public enum GameState
    {
        Playing,
        PlayerXWins,
        PlayerOWins,
        Draw
    }

    [System.Serializable]
    public class TicTacToeGame
    {
        [SerializeField] private CellState[,] board = new CellState[3, 3];
        [SerializeField] private CellState currentPlayer = CellState.X;
        [SerializeField] private GameState gameState = GameState.Playing;
        [SerializeField] private int[] winningLine = null;

        public event Action<int, int, CellState> OnCellChanged;
        public event Action<GameState, int[]> OnGameEnded;
        public event Action<CellState> OnPlayerChanged;

        public CellState CurrentPlayer => currentPlayer;
        public GameState State => gameState;
        public CellState[,] Board => board;

        public void StartNewGame()
        {
            // Reset board
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = CellState.Empty;
                }
            }

            currentPlayer = CellState.X;
            gameState = GameState.Playing;
            winningLine = null;

            OnPlayerChanged?.Invoke(currentPlayer);
        }

        public bool MakeMove(int row, int col)
        {
            // Check if game is still playing and cell is empty
            if (gameState != GameState.Playing || row < 0 || row >= 3 || col < 0 || col >= 3)
                return false;

            if (board[row, col] != CellState.Empty)
                return false;

            // Make the move
            board[row, col] = currentPlayer;
            OnCellChanged?.Invoke(row, col, currentPlayer);

            // Check for win or draw
            CheckGameEnd();

            // Switch player if game is still playing
            if (gameState == GameState.Playing)
            {
                currentPlayer = currentPlayer == CellState.X ? CellState.O : CellState.X;
                OnPlayerChanged?.Invoke(currentPlayer);
            }

            return true;
        }

        private void CheckGameEnd()
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] != CellState.Empty &&
                    board[row, 0] == board[row, 1] &&
                    board[row, 1] == board[row, 2])
                {
                    EndGame(board[row, 0], new int[] { row * 3, row * 3 + 1, row * 3 + 2 });
                    return;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] != CellState.Empty &&
                    board[0, col] == board[1, col] &&
                    board[1, col] == board[2, col])
                {
                    EndGame(board[0, col], new int[] { col, col + 3, col + 6 });
                    return;
                }
            }

            // Check diagonals
            if (board[0, 0] != CellState.Empty &&
                board[0, 0] == board[1, 1] &&
                board[1, 1] == board[2, 2])
            {
                EndGame(board[0, 0], new int[] { 0, 4, 8 });
                return;
            }

            if (board[0, 2] != CellState.Empty &&
                board[0, 2] == board[1, 1] &&
                board[1, 1] == board[2, 0])
            {
                EndGame(board[0, 2], new int[] { 2, 4, 6 });
                return;
            }

            // Check for draw
            bool isBoardFull = true;
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == CellState.Empty)
                    {
                        isBoardFull = false;
                        break;
                    }
                }
                if (!isBoardFull) break;
            }

            if (isBoardFull)
            {
                gameState = GameState.Draw;
                OnGameEnded?.Invoke(gameState, null);
            }
        }

        private void EndGame(CellState winner, int[] winLine)
        {
            gameState = winner == CellState.X ? GameState.PlayerXWins : GameState.PlayerOWins;
            winningLine = winLine;
            OnGameEnded?.Invoke(gameState, winningLine);
        }

        public CellState GetCell(int row, int col)
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
                return CellState.Empty;
            return board[row, col];
        }
    }
}