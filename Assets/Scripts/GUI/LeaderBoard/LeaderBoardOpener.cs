using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Utils.LB;

public class LeaderBoardOpener : MonoBehaviour
{
    [SerializeField] private GameObject _leaderBoard;
    [SerializeField] private GameObject _authWindow;
    [SerializeField] private LeaderboardYG _leaderboardYG;

    public void CheckAuthLeader()
    {
        if (YandexGame.auth)
        {
            _leaderBoard.SetActive(true);

            YandexGame.onGetLeaderboard += CheckLeader;
        }
        else
        {
            _authWindow.SetActive(true);
        }
    }

    public void Authorization()
    {
        YandexGame.AuthDialog();
    }

    private void CheckLeader(LBData lBData)
    {
        if(lBData.thisPlayer.score < YandexGame.savesData.leaderScore)
        {
            _leaderboardYG.NewScore(YandexGame.savesData.leaderScore);
            _leaderboardYG.UpdateLB();
        }
    }

    private void OnDisable()
    {
        YandexGame.onGetLeaderboard -= CheckLeader;
    }
}
