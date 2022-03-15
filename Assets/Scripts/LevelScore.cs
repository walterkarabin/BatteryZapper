using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelScore : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI kills;

    private int score;

    void Start()
    {
        score = Score.totalScore;
        level.text = score.ToString();
    }

    void Update()
    {
        kills.text = Score.totalKills.ToString();
    }
}
