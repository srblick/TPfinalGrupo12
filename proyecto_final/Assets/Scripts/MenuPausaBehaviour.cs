using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausaBehaviour : MonoBehaviour
{
    private void OnEnable() {
        // Detiene el tiempo para que el jugador no muera cuando esta en pausa.
        Time.timeScale = 0f;
    }

    private void OnDisable() {
        // Vuelve activa el tiempo para seguir jugando.
        Time.timeScale = 1f;
    }
    // Cambia la escena de acuerdo al parametro scene.
    public void MainMenuButton(string scene){
        SceneManager.LoadScene(scene);
    }

}
