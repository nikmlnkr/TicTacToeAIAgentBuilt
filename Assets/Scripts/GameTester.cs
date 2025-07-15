using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Game tester script to verify Tic Tac Toe game functionality
/// This script will automatically test the game and report any issues
/// </summary>
public class GameTester : MonoBehaviour
{
    [Header("Testing Configuration")]
    [Tooltip("Run automated tests when the game starts")]
    public bool runTestsOnStart = false;
    
    [Tooltip("Run tests every few seconds to monitor for issues")]
    public bool runContinuousTests = false;
    
    [Header("Test Results")]
    public bool allTestsPassed = false;
    public string lastTestResult = "";
    
    private TicTacToeGame gameManager;
    private UIManager uiManager;
    private SoundManager soundManager;
    
    void Start()
    {
        if (runTestsOnStart)
        {
            StartCoroutine(RunAllTests());
        }
        
        if (runContinuousTests)
        {
            InvokeRepeating(nameof(QuickHealthCheck), 5f, 10f);
        }
    }
    
    [ContextMenu("Run Complete Game Tests")]
    public void RunTestsManually()
    {
        StartCoroutine(RunAllTests());
    }
    
    IEnumerator RunAllTests()
    {
        Debug.Log("🧪 Starting comprehensive game tests...");
        
        yield return StartCoroutine(TestGameComponents());
        yield return StartCoroutine(TestGameplay());
        yield return StartCoroutine(TestUIFunctionality());
        yield return StartCoroutine(TestSoundSystem());
        
        if (allTestsPassed)
        {
            Debug.Log("✅ ALL TESTS PASSED! The game is fully functional!");
            lastTestResult = "All tests passed - Game is ready to play!";
        }
        else
        {
            Debug.LogError("❌ Some tests failed. Check the logs for details.");
            lastTestResult = "Some tests failed - Check console for details";
        }
    }
    
    IEnumerator TestGameComponents()
    {
        Debug.Log("🔍 Testing game components...");
        bool componentsOK = true;
        
        // Find game components
        gameManager = FindAnyObjectByType<TicTacToeGame>();
        uiManager = FindAnyObjectByType<UIManager>();
        soundManager = FindAnyObjectByType<SoundManager>();
        
        // Test Game Manager
        if (gameManager == null)
        {
            Debug.LogError("❌ TicTacToeGame not found!");
            componentsOK = false;
        }
        else
        {
            Debug.Log("✅ TicTacToeGame found");
            
            // Test buttons array
            if (gameManager.buttons == null || gameManager.buttons.Length != 9)
            {
                Debug.LogError($"❌ Game buttons not properly configured. Expected 9, found {(gameManager.buttons?.Length ?? 0)}");
                componentsOK = false;
            }
            else
            {
                Debug.Log("✅ Game buttons array configured");
                
                // Test each button
                for (int i = 0; i < gameManager.buttons.Length; i++)
                {
                    if (gameManager.buttons[i] == null)
                    {
                        Debug.LogError($"❌ Button {i} is null!");
                        componentsOK = false;
                    }
                    else if (gameManager.buttons[i].GetComponentInChildren<Text>() == null)
                    {
                        Debug.LogError($"❌ Button {i} missing Text component!");
                        componentsOK = false;
                    }
                }
                
                if (componentsOK)
                {
                    Debug.Log("✅ All 9 buttons properly configured");
                }
            }
            
            // Test status text
            if (gameManager.statusText == null)
            {
                Debug.LogError("❌ Status text not configured!");
                componentsOK = false;
            }
            else
            {
                Debug.Log("✅ Status text configured");
            }
            
            // Test restart button
            if (gameManager.restartButton == null)
            {
                Debug.LogError("❌ Restart button not configured!");
                componentsOK = false;
            }
            else
            {
                Debug.Log("✅ Restart button configured");
            }
        }
        
        // Test UI Manager
        if (uiManager == null)
        {
            Debug.LogWarning("⚠️ UIManager not found (optional)");
        }
        else
        {
            Debug.Log("✅ UIManager found");
        }
        
        // Test Sound Manager
        if (soundManager == null)
        {
            Debug.LogWarning("⚠️ SoundManager not found (optional)");
        }
        else
        {
            Debug.Log("✅ SoundManager found");
        }
        
        allTestsPassed = componentsOK;
        yield return null;
    }
    
    IEnumerator TestGameplay()
    {
        Debug.Log("🎮 Testing gameplay functionality...");
        
        if (gameManager == null)
        {
            Debug.LogError("❌ Cannot test gameplay - Game Manager not found");
            allTestsPassed = false;
            yield break;
        }
        
        // Test initial state
        if (gameManager.statusText.text != "Player X's Turn")
        {
            Debug.LogError($"❌ Initial status incorrect. Expected 'Player X's Turn', got '{gameManager.statusText.text}'");
            allTestsPassed = false;
        }
        else
        {
            Debug.Log("✅ Initial game state correct");
        }
        
        // Test button click simulation
        bool moveTest = TestButtonClick(0); // Click first button
        if (!moveTest)
        {
            Debug.LogError("❌ Button click test failed");
            allTestsPassed = false;
        }
        else
        {
            Debug.Log("✅ Button click functionality working");
        }
        
        // Test restart functionality
        if (gameManager.restartButton != null)
        {
            gameManager.RestartGame();
            yield return new WaitForSeconds(0.1f);
            
            if (gameManager.statusText.text == "Player X's Turn")
            {
                Debug.Log("✅ Restart functionality working");
            }
            else
            {
                Debug.LogError("❌ Restart functionality not working properly");
                allTestsPassed = false;
            }
        }
        
        yield return null;
    }
    
    bool TestButtonClick(int buttonIndex)
    {
        if (buttonIndex >= 0 && buttonIndex < gameManager.buttons.Length && gameManager.buttons[buttonIndex] != null)
        {
            // Simulate button click
            gameManager.OnCellClicked(buttonIndex);
            
            // Check if the button now shows X
            Text buttonText = gameManager.buttons[buttonIndex].GetComponentInChildren<Text>();
            if (buttonText != null && buttonText.text == "X")
            {
                return true;
            }
        }
        return false;
    }
    
    IEnumerator TestUIFunctionality()
    {
        Debug.Log("🖼️ Testing UI functionality...");
        
        if (uiManager != null)
        {
            // Test button animation
            if (gameManager.buttons[0] != null)
            {
                uiManager.AnimateButtonPress(gameManager.buttons[0]);
                Debug.Log("✅ Button animation test completed");
            }
        }
        
        yield return null;
    }
    
    IEnumerator TestSoundSystem()
    {
        Debug.Log("🔊 Testing sound system...");
        
        if (soundManager != null)
        {
            // Test sound methods (they won't play without audio clips, but should not error)
            soundManager.PlayButtonClick();
            soundManager.PlayMove();
            Debug.Log("✅ Sound system methods working");
        }
        
        yield return null;
    }
    
    void QuickHealthCheck()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<TicTacToeGame>();
        }
        
        if (gameManager != null)
        {
            bool healthy = true;
            
            // Check if buttons are still configured
            if (gameManager.buttons == null || gameManager.buttons.Length != 9)
            {
                healthy = false;
            }
            
            // Check if essential components exist
            if (gameManager.statusText == null)
            {
                healthy = false;
            }
            
            if (!healthy)
            {
                Debug.LogWarning("⚠️ Health check failed - Game components may have been modified");
            }
        }
    }
    
    [ContextMenu("Print Game Status")]
    public void PrintGameStatus()
    {
        Debug.Log("📊 Current Game Status:");
        Debug.Log($"Game Manager: {(gameManager != null ? "✅" : "❌")}");
        Debug.Log($"UI Manager: {(uiManager != null ? "✅" : "❌")}");
        Debug.Log($"Sound Manager: {(soundManager != null ? "✅" : "❌")}");
        
        if (gameManager != null)
        {
            Debug.Log($"Buttons configured: {gameManager.buttons?.Length ?? 0}/9");
            Debug.Log($"Status text: {(gameManager.statusText != null ? "✅" : "❌")}");
            Debug.Log($"Restart button: {(gameManager.restartButton != null ? "✅" : "❌")}");
            Debug.Log($"Current status: {gameManager.statusText?.text ?? "N/A"}");
        }
        
        Debug.Log($"Last test result: {lastTestResult}");
    }
}