using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public BallController ball;
    public TextMeshProUGUI labelPlayerName;

    private PlayerRecord playerRecord;
    private int playerIndex;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record")?.GetComponent<PlayerRecord>();
        if (playerRecord == null || playerRecord.playerList.Count == 0) return;

        playerIndex = 0;
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        if (ball != null && playerIndex < playerRecord.playerColours.Length)
        {
            ball.SetupBall(playerRecord.playerColours[playerIndex]);
        }

        if (labelPlayerName != null && playerIndex < playerRecord.playerList.Count)
        {
            labelPlayerName.ForceMeshUpdate(); // TMP-safe initialization
            labelPlayerName.text = playerRecord.playerList[playerIndex].name;
        }
    }

    public void NextPlayer(int previousPutts)
    {
        if (playerRecord == null) return;

        playerRecord.AddPutts(playerIndex, previousPutts);

        if (playerIndex < playerRecord.playerList.Count - 1)
        {
            playerIndex++;

            if (ball != null && playerIndex < playerRecord.playerColours.Length)
                ball.SetupBall(playerRecord.playerColours[playerIndex]);

            if (labelPlayerName != null && playerIndex < playerRecord.playerList.Count)
            {
                labelPlayerName.ForceMeshUpdate();
                labelPlayerName.text = playerRecord.playerList[playerIndex].name;
            }
        }
        else
        {
            if (playerRecord.levelIndex >= playerRecord.levels.Length - 1)
            {
                Debug.Log("Scoreboard");
            }
            else
            {
                playerRecord.levelIndex++;
                SceneManager.LoadScene(playerRecord.levels[playerRecord.levelIndex]);
            }
        }
    }
}
