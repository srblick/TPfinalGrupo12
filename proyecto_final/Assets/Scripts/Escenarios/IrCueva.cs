using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrCueva : MonoBehaviour
{
    //Jugador
    public GameObject Player;
    //Lugar de aparicion
    public Transform Spawn;

    void Start() 
    {
        //Busca al objeto con la Etiquta del jugador y lo asigna a la variable Player
        Player = GameObject.FindWithTag("Player");
    }

    public void OnTriggerEnter(Collider other)
    {
        //Ve si el objeto que coliciona es Player
        if (other.tag == "Player")
        {
            //Cambia la posicion del jugador
            Player.GetComponent<CharacterController>().enabled = false;
            Player.GetComponent<PlayerController>().enabled = false;
            Player.transform.position = Spawn.position;
            Player.GetComponent<CharacterController>().enabled = true;
            Player.GetComponent<PlayerController>().enabled = true;
        }
    }    
}