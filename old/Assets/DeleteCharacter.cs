using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCharacter : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }

}
