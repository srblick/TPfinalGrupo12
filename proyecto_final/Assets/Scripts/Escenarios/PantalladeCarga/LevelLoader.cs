using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Funcion para usar cambio de escenas
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    //Guarda el nombre del siguiente nivel
    public static string nextLevel;

    //Carga una Escena segun su nombre en este caso va a la pantalla de carga
    public static void LoadLevel(string name)
    {
        nextLevel = name;

        SceneManager.LoadScene("LoadScreen");
    }
}
