using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TicTacToe
{
    public class TicTacToeCell : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button cellButton;
        [SerializeField] private TextMeshProUGUI cellText;
        [SerializeField] private Image cellBackground;

        [Header("Visual Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color winColor = Color.green;

        private int row, col;
        private CellState state = CellState.Empty;
        private GameManager gameManager;

        public int Row => row;
        public int Col => col;
        public CellState State => state;

        public void Initialize(int row, int col, GameManager gameManager)
        {
            this.row = row;
            this.col = col;
            this.gameManager = gameManager;

            if (cellButton == null)
                cellButton = GetComponent<Button>();
            if (cellText == null)
                cellText = GetComponentInChildren<TextMeshProUGUI>();
            if (cellBackground == null)
                cellBackground = GetComponent<Image>();

            cellButton.onClick.AddListener(OnCellClicked);
            ResetCell();
        }

        public void ResetCell()
        {
            state = CellState.Empty;
            cellText.text = "";
            cellBackground.color = normalColor;
            cellButton.interactable = true;
        }

        public void SetState(CellState newState)
        {
            state = newState;
            
            switch (state)
            {
                case CellState.Empty:
                    cellText.text = "";
                    cellButton.interactable = true;
                    break;
                case CellState.X:
                    cellText.text = "X";
                    cellButton.interactable = false;
                    break;
                case CellState.O:
                    cellText.text = "O";
                    cellButton.interactable = false;
                    break;
            }
        }

        public void HighlightAsWinning()
        {
            cellBackground.color = winColor;
        }

        public void SetHighlight(bool highlighted)
        {
            cellBackground.color = highlighted ? highlightColor : normalColor;
        }

        private void OnCellClicked()
        {
            if (gameManager != null && state == CellState.Empty)
            {
                gameManager.OnCellClicked(row, col);
            }
        }

        private void OnDestroy()
        {
            if (cellButton != null)
                cellButton.onClick.RemoveListener(OnCellClicked);
        }
    }
}