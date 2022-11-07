using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fusion para poder gestionar Scenas
using UnityEngine.SceneManagement;

public class CambioNivel : MonoBehaviour
{   
    public void OnTriggerEnter(Collider other)
    {
        //Detecta la Escena/Nivel en la que estamos y devulve su numero de indice
        int Scene = SceneManager.GetActiveScene().buildIndex;

        if (other.tag == "Player")
        {
            //Cambia la Escena segun el indice
            switch (Scene)
            {
                case 1:
                 LevelLoader.LoadLevel("Pasado");
                break;

                case 2:
                 LevelLoader.LoadLevel("Futuro");
                break;

                case 3:
                 LevelLoader.LoadLevel("Fin");
                break;
            }
        }
    }
}