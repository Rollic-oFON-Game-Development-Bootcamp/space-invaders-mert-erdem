using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    private static CanvasController _instance;
    public static CanvasController Instance => _instance ?? (_instance = FindObjectOfType<CanvasController>());

    [SerializeField] private GameObject panelMenu, panelInGame, panelEndGame;
    [SerializeField] private TextMeshProUGUI textHealth, textScore, textEndGameScore, textHighScore;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GameManager.Instance.ActionGameStart += SetInGameUI;
        GameManager.Instance.ActionGameOver += SetEndGameUI;
    }

    private void SetInGameUI()
    {
        panelMenu.SetActive(false);
        panelInGame.SetActive(true);
    }

    private void SetEndGameUI()
    {
        textEndGameScore.text += GameManager.Instance.Score.ToString();
        textHighScore.text += PlayerPrefs.GetInt("HIGH_SCORE", 0).ToString();

        panelInGame.SetActive(false);
        panelEndGame.SetActive(true);
    }

    public void UpdateHealthText(int value)
    {
        textHealth.text = value.ToString();
    }

    public void UpdateScoreText(int value)
    {
        textScore.text = "score: " + value.ToString();
    }

    public void StartButtonPressed()
    {
        GameManager.Instance.ActionGameStart?.Invoke();
    }

    public void RestartButtonPressed()
    {
        GameManager.Instance.RestartTheGame();
    }

    private void OnDestroy()
    {
        GameManager.Instance.ActionGameStart -= SetInGameUI;
        GameManager.Instance.ActionGameOver -= SetEndGameUI;
    }
}
