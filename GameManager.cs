using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterSpawner characterSpawner; // Reference to CharacterSpawner
    [SerializeField] private TextMeshProUGUI missionAccomplishedText; // UI text for win message

    private bool hasWon = false; // Prevent multiple win triggers

    private void Awake()
    {
        // Validate references
        if (characterSpawner == null)
        {
            Debug.LogError("GameManager: CharacterSpawner not assigned. Disabling script.", this);
            enabled = false;
            return;
        }
        if (missionAccomplishedText == null)
        {
            Debug.LogError("GameManager: MissionAccomplishedText not assigned. Disabling script.", this);
            enabled = false;
            return;
        }

        // Ensure UI text is hidden initially
        missionAccomplishedText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Skip if already won or references are invalid
        if (hasWon || !enabled || characterSpawner == null)
            return;

        // Check if all characters are saved
        if (AreAllCharactersSaved())
        {
            StartCoroutine(WinRoutine());
        }
    }

    private bool AreAllCharactersSaved()
    {
        // Get spawned characters
        GameObject[] characters = characterSpawner.spawnedCharacters;
        if (characters == null || characters.Length == 0)
        {
            Debug.LogWarning("GameManager: No characters available from CharacterSpawner.", this);
            return false;
        }

        // Check if all characters are saved
        foreach (GameObject character in characters)
        {
            if (character != null)
            {
                CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
                if (behavior == null || !behavior.IsSaved)
                {
                    return false; // At least one character is not saved
                }
            }
        }

        return true; // All characters are saved
    }

    private IEnumerator WinRoutine()
    {
        hasWon = true;
        Debug.Log("GameManager: Mission Accomplished! All characters saved.");

        // Display win message
        if (missionAccomplishedText != null)
        {
            missionAccomplishedText.text = "Mission Accomplished !";
            missionAccomplishedText.gameObject.SetActive(true);
        }

        // Wait for 10 seconds
        yield return new WaitForSeconds(10f);

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Alternative: Load main menu scene (uncomment if main menu exists)
        // SceneManager.LoadScene("MainMenu");
    }
}