using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HostIp : MonoBehaviour
{
    public TMP_InputField Field;
    public string IpStr;

    public void GetHostIp()
    {
        IpStr = Field.GetComponent<TMP_InputField>().text;
        Debug.Log(IpStr);
    }
}
