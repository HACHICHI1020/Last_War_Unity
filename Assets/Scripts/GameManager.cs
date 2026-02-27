using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("레벨")]
    public static GameManager instance;
    public TextMeshProUGUI levelText; //레벨

    [Header("점수")]
    public TextMeshProUGUI scoreText; //점수 텍스트
    public enum ScoreState
    {
        None, ScoreUpdate
    }
    public ScoreState scoreState = ScoreState.None;
    private float score;
    private float targetScore; //목표 점수
    private float scoreSpeed = 100.0f;

    [Header("게임오버")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        switch(scoreState)
        {
            case ScoreState.ScoreUpdate:
                {
                    score = Mathf.MoveTowards(score, targetScore, scoreSpeed * Time.deltaTime);
                    scoreText.text = score.ToString("N0");

                    if(score == targetScore)
                    {
                        scoreState = ScoreState.None;
                    }
                    break;
                }
        }
    }

    //게임 시작 버튼 On
    public void GameStartButtonOn()
    {
        Player.instance.PlayerStart(); //플레이어 시작
        SpawnManager.instance.SpawnStart(); //소환 시작
    }

    public void SpawnLevelTextOn(int lv)
    {
        levelText.text = "LV: " + lv.ToString();
    }

    public void ScoreTextOn()
    {
        //점수 텍스트를 표시한다.
        scoreText.text = score.ToString("N0");

    }

    //점수 증가 처리 함수
    public void ScoreUp(float addScore)
    {
        if(scoreSpeed < addScore)
        {
            scoreSpeed = addScore;
        }
        targetScore += addScore;
        scoreState = ScoreState.ScoreUpdate; //점수 변동상태

    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = scoreText.text;

    }

    public void RetryButtonOn()
    {
        SceneManager.LoadScene(0);
    }
}
