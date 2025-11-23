using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BallController ball;
    public TextMeshProUGUI labelPlayerName;

    private PlayerRecord playerRecord;
    private int playerIndex;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        playerIndex = 0;
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        ball.SetupBall(playerRecord.playerColours[playerIndex]);
        labelPlayerName.text = playerRecord.playerList[playerIndex].name;
    }
}
