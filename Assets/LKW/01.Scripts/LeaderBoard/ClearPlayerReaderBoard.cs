using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using System.Threading.Tasks;
using LKW._01.Scripts.LeaderBoard;
using UnityEngine.Serialization;
using DG.Tweening;
using EasyTransition;

public class ClearPlayerReaderBoard : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Text leaderboardText;
    [SerializeField] private Transform boxParent;
    [SerializeField] private GameObject rankBoxPrefab;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image ReaderBoard;
    [SerializeField] private Image RegisterPannel;
    

    private string leaderboardId = "gamejam_Leaderboard";

    private int clearTime = 0;

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
    }

    public async void OnSubmit()
    {
        Debug.Log("Submit");
        
        string playerName = playerNameInput.text;
        
        if(playerName.Length > 8) return;
        
        if (!string.IsNullOrEmpty(playerName))
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        }

        try
        {
            // 점수는 고정값 1 (클리어 표시용)
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, clearTime);
            Debug.Log($"점수 등록 완료: {playerName}");
            await RefreshLeaderboard();
            RegisterPannel.transform.DOScale(0, 0.5f).OnComplete(()=>
            {
                ReaderBoard.transform.DOScale(1, 0.8f);
                
            });
            
        }
        catch (System.Exception e)
        {
            Debug.LogError($"점수 등록 실패: {e.Message}");
        }
    }

    public void ToTile()
    {
        DemoLoadScene.instance.LoadScene("Title");
    }
    
    public async Task RefreshLeaderboard()
    {
        int number = 1;
        
        for (int i = boxParent.childCount - 1; i >= 0; i--)
        {
            Destroy(boxParent.GetChild(i).gameObject);
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

    public void SetClearTime(int time)
    {
        clearTime = time;
        
        int minutes = Mathf.FloorToInt(clearTime / 60);
        int seconds = Mathf.FloorToInt(clearTime % 60);
                
        string timeTxt = string.Format("{0:00}:{1:00}", minutes, seconds);
        
         timeText.text = timeTxt;
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
