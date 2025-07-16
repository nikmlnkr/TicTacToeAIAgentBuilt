using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gamePanel;
    public GameObject menuPanel;
    public Button playButton;
    public Button quitButton;
    public TMP_Text titleText;
    
    [Header("Animation Settings")]
    public float fadeSpeed = 2f;
    public float scaleSpeed = 3f;
    
    void Start()
    {
        SetupUI();
    }
    
    void SetupUI()
    {
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
            
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
            
        // Show menu initially
        ShowMenu();
    }
    
    public void StartGame()
    {
        StartCoroutine(TransitionToGame());
    }
    
    public void ShowMenu()
    {
        if (menuPanel != null)
            menuPanel.SetActive(true);
            
        if (gamePanel != null)
            gamePanel.SetActive(false);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    IEnumerator TransitionToGame()
    {
        // Fade out menu
        if (menuPanel != null)
        {
            CanvasGroup menuCanvasGroup = menuPanel.GetComponent<CanvasGroup>();
            if (menuCanvasGroup == null)
                menuCanvasGroup = menuPanel.AddComponent<CanvasGroup>();
                
            while (menuCanvasGroup.alpha > 0)
            {
                menuCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
            
            menuPanel.SetActive(false);
        }
        
        // Show game panel
        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
            
            CanvasGroup gameCanvasGroup = gamePanel.GetComponent<CanvasGroup>();
            if (gameCanvasGroup == null)
                gameCanvasGroup = gamePanel.AddComponent<CanvasGroup>();
                
            gameCanvasGroup.alpha = 0;
            
            while (gameCanvasGroup.alpha < 1)
            {
                gameCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }
        }
    }
    
    public void AnimateButtonPress(Button button)
    {
        if (button != null)
            StartCoroutine(ButtonPressAnimation(button));
    }
    
    IEnumerator ButtonPressAnimation(Button button)
    {
        Vector3 originalScale = button.transform.localScale;
        Vector3 pressedScale = originalScale * 0.9f;
        
        // Scale down
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * scaleSpeed;
            button.transform.localScale = Vector3.Lerp(originalScale, pressedScale, t);
            yield return null;
        }
        
        // Scale back up
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * scaleSpeed;
            button.transform.localScale = Vector3.Lerp(pressedScale, originalScale, t);
            yield return null;
        }
        
        button.transform.localScale = originalScale;
    }
}