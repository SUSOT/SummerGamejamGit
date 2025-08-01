using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;

public class ClearPlayerReaderBoard : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text leaderboardText;

    [SerializeField] private Transform rankBoardTrm;

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

    public async void OnSubmit()
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
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, Random.Range(0,10));
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

        leaderboardText.text = "로딩 중...";

        try
        {
            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = 50 });

            leaderboardText.text = "";
            foreach (var entry in scores.Results)
            {
                string name = entry.PlayerName ?? entry.PlayerId;
                leaderboardText.text += $"{entry.Rank + 1}. {name}\n";
            }
        }
        catch (System.Exception e)
        {
            leaderboardText.text = "리더보드 불러오기 실패";
            Debug.LogError(e.Message);
        }
    }
}
