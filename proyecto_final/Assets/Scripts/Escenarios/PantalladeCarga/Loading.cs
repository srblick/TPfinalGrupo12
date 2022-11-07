using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    void Start()
    {
        //Toma el nombre de la Escena que va cargar
        string levelToLoad = LevelLoader.nextLevel;

        //Carga el nivel de forma asincronica
        StartCoroutine(this.MakeTheLoad(levelToLoad));
    }

    IEnumerator MakeTheLoad(string level)
    {
        //Hace esperar 5 segundos antes de cambiar el Escenario
        yield return new WaitForSeconds(5f);

        //Avisa cuando todos los recursos del nivel estan cargados
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        //Cuando la carga este terminada cambiara el Escenario (luego de los 5 segundos)
        while (operation.isDone == false)
        {
            yield return null;
        }
    }
}