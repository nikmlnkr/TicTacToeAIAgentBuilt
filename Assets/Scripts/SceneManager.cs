using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    
    public static SceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneManager>();
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadMainMenu()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
            
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
            
        UnityEngine.SceneManagement.SceneManager.LoadScene("TicTacToeGame");
    }
    
    public void QuitGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
            
        Debug.Log("Quitting Game...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}