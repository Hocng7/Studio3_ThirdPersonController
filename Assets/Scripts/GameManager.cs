using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    [SerializeField] private TextMeshProUGUI scoreText;
    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateScore()
    {
        score +=1;
        scoreText.text = "Score: " + score; // Update the UI text with the new score
    }
}
