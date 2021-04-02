using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoundEnding : NetworkBehaviour
{
    [SerializeField]
    private float roundTime = 30f;

    [SerializeField]
    private float WinScreenTime = 10f;

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
        // Display of the winner of the round
        DisplayRoundWinner();
        StartCoroutine(RoundWinnerTextTimer(WinScreenTime));
    }

    [ClientRpc]
    void DisplayRoundWinner()
    {
        Debug.Log("Display of the winner of the round");
    }

    IEnumerator RoundWinnerTextTimer(float time)
    {
        yield return new WaitForSeconds(time);
 
        Debug.Log("Winner screen ended");
        
        // Delete the entities
        DeleteEntities();

        // Restart sequence of a new round
        RestartRound();
    }

    [ClientRpc]
    void DeleteEntities()
    {
        Debug.Log("Deleting the entities");

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Projectile");

        for (int i = 0; i < objects.Length; i++)
            Destroy(objects[i]);
    }

    [Command]
    void RestartRound()
    {
        Debug.Log("Restarting the round");
        StartRound();
    }

}
