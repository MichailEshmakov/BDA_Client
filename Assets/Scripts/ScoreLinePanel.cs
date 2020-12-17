using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLinePanel : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text scoreText;

    public ScoreLine ScoreLine 
    {
        set
        {
            nameText.text = value.playerName;
            scoreText.text = value.score.ToString();
        }
    } 
}
