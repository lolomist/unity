using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinMenu : MonoBehaviour
{
    GameObject inputField;

    public void getHostIp()
    {
        string textInputField = inputField.GetComponent<TMP_InputField>().text;
        // Host Ip
    }

    public void Join()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Join
    }
}
