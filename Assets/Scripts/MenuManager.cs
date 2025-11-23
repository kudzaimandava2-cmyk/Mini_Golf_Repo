using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputPlayerName;
    public PlayerRecord playerRecord;
    public Button buttonStart;
    public Button buttonAddPlayer;

    private void Start()
    {
        // Disable start button until at least one player is added
        if (buttonStart != null)
            buttonStart.interactable = playerRecord != null && playerRecord.playerList.Count > 0;
    }

    public void ButtonAddPlayer()
    {
        if (playerRecord == null || inputPlayerName == null) return;
        if (string.IsNullOrWhiteSpace(inputPlayerName.text)) return;

        playerRecord.AddPlayer(inputPlayerName.text.Trim());

        // Enable start button safely
        if (buttonStart != null)
            buttonStart.interactable = true;

        // Clear input field safely
        inputPlayerName.text = "";
        inputPlayerName.ForceLabelUpdate(); // ensures TMP mesh is updated safely

        // Disable add player button if max reached
        if (playerRecord.playerList.Count >= playerRecord.playerColours.Length)
        {
            if (buttonAddPlayer != null)
                buttonAddPlayer.interactable = false;
        }
    }

    public void ButtonStart()
    {
        if (playerRecord == null || playerRecord.levels.Length == 0) return;

        SceneManager.LoadScene(playerRecord.levels[0]);
    }
}
