using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ?? (_instance = FindObjectOfType<GameManager>());

    public UnityAction ActionGameStart, ActionGameOver;
    private int score, scoreDelta = 20;
    public int Score => score;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        score = 0;
        ActionGameOver += SaveScore;
    }

    public void AddScore()
    {
        score += scoreDelta;
        CanvasController.Instance.UpdateScoreText(score);
    }

    private void SaveScore()
    {
        int highScore = PlayerPrefs.GetInt("HIGH_SCORE", 0);

        if (score > highScore)
            PlayerPrefs.SetInt("HIGH_SCORE", score);
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        ActionGameOver -= SaveScore;
    }
}
