using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using UnityEngine.SceneManagement;

public class RoundEnding : NetworkBehaviour
{
    [SerializeField]
    private float roundTime = 30f;

    [SerializeField]
    private float WinScreenTime = 10f;

    [SerializeField]
    private GameObject WinUIPrefab;
    private GameObject WinUIInstance;

    [SerializeField]
    private int sceneNumbers = 3;

    // Start is called before the first frame update
    void Start()
    {
        CmdStartRound();
    }

    // Update is called once per frame
    private void CmdStartRound()
    {
        if(!isLocalPlayer) {
            return;
        }

        StartRound();
    }

    [Command]
    void StartRound()
    {
        StartCoroutine(RoundTimer(roundTime));
    }

    IEnumerator RoundTimer(float time)
    {
        yield return new WaitForSeconds(time);
 
        Debug.Log("Round ended");
        EndRound();
    }

    [Command]
    void EndRound()
    {
        // Delete the entities
        DeleteEntities();

        // Display of the winner of the round
        DisplayRoundWinner();
        StartCoroutine(RoundWinnerTextTimer(WinScreenTime));
    }

    [ClientRpc]
    void DisplayRoundWinner()
    {
        // On regarde qui est le gagnant
        Dictionary<string, PlayerController> players = GameManager.GetPlayers();
        Dictionary<string, PlayerController> winners = new Dictionary<string, PlayerController>();
        int max = 10000;

        for (int i = 0; i < players.Count; i++) {
            if (max == players.Values.ElementAt(i).GetScore())
                winners.Add(players.Keys.ElementAt(i), players.Values.ElementAt(i));
            if (max > players.Values.ElementAt(i).GetScore()) {
                max = players.Values.ElementAt(i).GetScore();
                winners.Clear();
                winners.Add(players.Keys.ElementAt(i), players.Values.ElementAt(i));
            }
        }

        // UI du winner du fin de round
        WinUIInstance = Instantiate(WinUIPrefab);
        WinUI ui = WinUIInstance.GetComponent<WinUI>();
        if (ui == null)
            Debug.LogError("Pas de component WinUI sur WinUIInstance");
        else {
            ui.SetController(GetComponent<PlayerController>());
            ui.DisplayWinner(winners);
        }
        Debug.Log("Display of the winner of the round");
    }

    IEnumerator RoundWinnerTextTimer(float time)
    {
        yield return new WaitForSeconds(time);
 
        Debug.Log("Winner screen ended");

        // Restart sequence of a new round
        RestartRound();
    }

    [ClientRpc]
    void DeleteEntities()
    {
        //Debug.Log("Deleting the entities");

        //Destroy(WinUIInstance);

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Projectile");

        for (int i = 0; i < objects.Length; i++)
            Destroy(objects[i]);
    }

    void RestartRound()
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneNumbers)
            NetworkManager.singleton.ServerChangeScene("level" + 1);
        else
            NetworkManager.singleton.ServerChangeScene("level" + (SceneManager.GetActiveScene().buildIndex + 1));
        Debug.Log("Restarting the round");
        //StartRound();
    }

}