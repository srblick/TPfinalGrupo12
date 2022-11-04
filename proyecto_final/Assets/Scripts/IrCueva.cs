using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrCueva : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;
    public Transform Spawn;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player.transform.position = Spawn.position;
        }
    }    
}