using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainPanelBehaviour : MonoBehaviour
{
    [Header("Options")]
    public Slider volumenFX;
    public Slider volumenMaster;
    public Toggle mute;
    public AudioMixer mixer;
    private float lastVolume;
    [Header("Options")]
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    private void Awake(){
        volumenFX.onValueChanged.AddListener(ChangeVolumenFX);
        volumenMaster.onValueChanged.AddListener(ChangeVolumenMaster);
    }
    // Abre un panel pasado por parametro panel y desactiva los demas
    public void OpenPanel(GameObject panel){
        deactivePanels();
        panel.SetActive(true);
    }

    public void NewGameButton(string scene){
        Debug.Log("Nuevo Juego");
        SceneManager.LoadScene(scene); // carga la escena del juego que se indique
    }

    /** termino el juego cuando se hace click en el boton exit. */
    public void ExitButton(){
        Debug.Log("Salio del Juego");
        Application.Quit(); //Cierra la aplicacion.
    }

    // Cambia el VolumenFX de acuerdo al float que se le pasa.
    public void ChangeVolumenMaster(float volumen){
        mixer.SetFloat("VolMaster", volumen);
    }
 
    // Cambia el VolumenFX de acuerdo al float que se le pasa.
    public void ChangeVolumenFX(float volumen){
        mixer.SetFloat("VolFX", volumen);
    }

    /** Pone en silencio el sonido */
    public void SetMute(){
        if (mute.isOn){ // si se activa el toggle mute
            mixer.GetFloat("VolMaster", out lastVolume); // guardo el volumen actual
            mixer.SetFloat("VolMaster", -80); // bajo el volumen a -80 simulando el mute
        }else
        {
            mixer.SetFloat("VolMaster", lastVolume); // restauro al volumen guardado en lastVolume
        }
    }

    private void deactivePanels(){
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().position = new Vector2(
            Input.mousePosition.x / Screen.width * 20 + Screen.width / 2,
            Input.mousePosition.y / Screen.height * 20 + Screen.height / 2);
    }
}
