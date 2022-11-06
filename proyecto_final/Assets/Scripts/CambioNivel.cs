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
            switch (Scene)
            {
                case 0:
                 LevelLoader.LoadLevel("Pasado");
                break;

                case 1:
                 LevelLoader.LoadLevel("Futuro");
                break;

                case 2:
                 LevelLoader.LoadLevel("Presente");
                break;
            }
        }
    }
}