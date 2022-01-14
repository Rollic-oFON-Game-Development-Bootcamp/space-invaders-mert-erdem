using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasController : MonoBehaviour
{
    private static CanvasController _instance;
    public static CanvasController Instance => _instance ?? (_instance = FindObjectOfType<CanvasController>());

    [SerializeField] private TextMeshProUGUI textHealth, textScore;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealthText(int value)
    {
        textHealth.text = value.ToString();
    }

    public void UpdateScoreText(int value)
    {
        textScore.text = "score" + value.ToString();
    }
}
