using UnityEngine;
using UnityEngine.UI;

public class CharacterProfileHUD : MonoBehaviour
{
    [Header("Profile Images")]
    public RawImage steveImage;
    public RawImage peteImage;
    public RawImage kateImage;

    [Header("Checkmark Images")]
    public Image steveCheck;
    public Image steveCheck1;
    public Image peteCheck;
    public Image peteCheck1;
    public Image kateCheck;
    public Image kateCheck1;

    [Header("References")]
    public CharacterSpawner characterSpawner;

    void Start()
    {
        // Hide checkmarks initially
        steveCheck.gameObject.SetActive(false);
        steveCheck1.gameObject.SetActive(false);
        peteCheck.gameObject.SetActive(false);
        peteCheck1.gameObject.SetActive(false);
        kateCheck.gameObject.SetActive(false);
        kateCheck1.gameObject.SetActive(false);

        // Link to characters
        LinkCharacters();
    }

    void Update()
    {
        // Update checkmarks based on saved status
        if (characterSpawner != null && characterSpawner.spawnedCharacters != null)
        {
            foreach (var character in characterSpawner.spawnedCharacters)
            {
                if (character != null)
                {
                    CharacterBehavior behavior = character.GetComponent<CharacterBehavior>();
                    if (behavior != null)
                    {
                        if (character.name.ToLower().Contains("steve")) steveCheck.gameObject.SetActive(behavior.IsSaved);
                        if (character.name.ToLower().Contains("steve")) steveCheck1.gameObject.SetActive(behavior.IsSaved);
                        if (character.name.ToLower().Contains("pete")) peteCheck.gameObject.SetActive(behavior.IsSaved);
                        if (character.name.ToLower().Contains("pete")) peteCheck1.gameObject.SetActive(behavior.IsSaved);
                        if (character.name.ToLower().Contains("kate")) kateCheck.gameObject.SetActive(behavior.IsSaved);
                        if (character.name.ToLower().Contains("kate")) kateCheck1.gameObject.SetActive(behavior.IsSaved);
                    }
                }
            }
        }
    }

    void LinkCharacters()
    {
        if (characterSpawner != null && characterSpawner.spawnedCharacters != null)
        {
            foreach (var character in characterSpawner.spawnedCharacters)
            {
                if (character != null && character.GetComponent<CharacterBehavior>() != null)
                {
                    // No direct linking needed; handled in Update
                }
            }
        }
    }
}