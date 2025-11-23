using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BallController ball;

    private PlayerRecord playerRecord;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
    }

    private void SetupPlayer()
    {

    }
}
