using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderbord : MonoBehaviour
{
    [SerializeField] GameObject ratingContainer;
    [SerializeField] ScoreLinePanel scoreLinePanelPrefab;
    [SerializeField] float offsetBetweenLines;

    List<ScoreLinePanel> ScoreLinePanels;

    private RectTransform ratingContainerTransform;
    private RectTransform scoreLinePanelPrefabTransform;

    private void Awake()
    {
        ratingContainerTransform = ratingContainer.GetComponent<RectTransform>();
        scoreLinePanelPrefabTransform = scoreLinePanelPrefab.GetComponent<RectTransform>();
    }

    public void Refresh(List<ScoreLine> scoreLines)
    {
        if (ScoreLinePanels != null)
        {
            ScoreLinePanels.Clear();
        }

        ratingContainerTransform.sizeDelta = new Vector2(ratingContainerTransform.sizeDelta.x, (scoreLines.Count + 1) * (scoreLinePanelPrefabTransform.sizeDelta.y + offsetBetweenLines));
        ratingContainerTransform.anchoredPosition = new Vector2(ratingContainerTransform.sizeDelta.x / 2, -ratingContainerTransform.sizeDelta.y / 2);

        foreach (ScoreLine scoreLine in scoreLines)
        {
            ScoreLinePanel scoreLinePanel = Instantiate(scoreLinePanelPrefab, ratingContainer.transform);
            scoreLinePanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,
                - (0.5f + (float)scoreLines.IndexOf(scoreLine)) * (scoreLinePanelPrefabTransform.sizeDelta.y + offsetBetweenLines));
            scoreLinePanel.ScoreLine = scoreLine;
        }

    }
}
