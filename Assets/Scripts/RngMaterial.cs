using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RngMaterial : NetworkBehaviour {
    [SyncVar]
    private Color c;
    public List<Color> TintColors;

    [SerializeField]
    public Renderer mesh; 

    // Start is called before the first frame update
    void Start()
    {

        CmdColor();
    }

    [ClientRpc]
    public void RpcUpdateColor()
    {
        mesh.material.color = c;
    }

    [Command]
    public void CmdColor()
    {
        RpcUpdateColor();
    }
}