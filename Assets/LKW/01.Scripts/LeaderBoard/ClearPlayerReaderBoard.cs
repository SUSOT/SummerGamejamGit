using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;
using LKW._01.Scripts.LeaderBoard;

public class ClearPlayerReaderBoard : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text leaderboardText;

    [SerializeField] private Transform boxParent;
    [SerializeField] private GameObject rankBoxPrefab;

    private string leaderboardId = "gamejam_Leaderboard"; // Unity Dashboard에서 만든 리더보드 ID

    private int temp = 1;
    async void Start()
    {
        await UnityServices.InitializeAsync();

        // 이미 로그인 되어 있는지 확인
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Signed in. PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError($"Authentication failed: {ex}");
            }
            catch (RequestFailedException ex)
            {
                Debug.LogError($"Request failed: {ex}");
            }
        }

        // 로그인 성공 후 리더보드 호출
        await RefreshLeaderboard();
    }

    public async void OnSubmit(float surviveTime)
    {
        Debug.Log("Submit");
        
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }

        try
        {
            // 점수는 고정값 1 (클리어 표시용)
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, surviveTime);
            Debug.Log($"점수 등록 완료: {playerName}");
            await RefreshLeaderboard();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"점수 등록 실패: {e.Message}");
        }
    }

    public async Task RefreshLeaderboard()
    {
        int number = 1;
        
        for (int i = boxParent.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        leaderboardText.text = "로딩 중...";

        try
        {
            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = 10 });
            
            leaderboardText.text = "";
            
            foreach (var entry in scores.Results)
            {
                RankBox rankBox = Instantiate(rankBoxPrefab, boxParent).GetComponent<RankBox>();

                string name = RemoveAfterHash(entry.PlayerName);
                
                int minutes = Mathf.FloorToInt((int)entry.Score / 60);
                int seconds = Mathf.FloorToInt((int)entry.Score % 60);
                
                string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
                
                rankBox.SetRankBox(name, timeText, number++);
            }
        }
        catch (System.Exception e)
        {
            leaderboardText.text = "리더보드 불러오기 실패";
            Debug.LogError(e.Message);
        }
    }

    public static string RemoveAfterHash(string original)
    {
        if (string.IsNullOrEmpty(original))
            return original;

        int hashIndex = original.IndexOf('#');
        if (hashIndex >= 0)
        {
            return original.Substring(0, hashIndex);
        }

        return original;
    }
}
