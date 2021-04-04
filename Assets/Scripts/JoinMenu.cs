using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

public class JoinMenu : MonoBehaviour
{
    public GameObject inputField;
    public NetworkManager newtwork;

    public void getHostIp()
    {
        string textInputField = inputField.GetComponent<TMP_InputField>().text;
        newtwork.networkAddress = textInputField;
        // Host Ip
    }

    public void Join()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Join
    }
}
