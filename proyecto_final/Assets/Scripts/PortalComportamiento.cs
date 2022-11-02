using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Funcion para usar cambio de escenas
using UnityEngine.SceneManagement;

public class PortalComportamiento : MonoBehaviour
{
    //Portal
    public GameObject portalParticulas;
    public GameObject zonaInteraccion;
    public CambioNivel cambiarNivel;
    public bool entrar;

    void Start()
    {
        portalParticulas.SetActive(false);
        zonaInteraccion.SetActive(false);
        //cambiarNivel.enabled = false;
        //cambiarNivel = portalParticulas.GetComponent<CambioNivel>(); 
    }

    void Update()
    {
        activarPortal();
    }

    public void activarPortal()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
           portalParticulas.SetActive(true);
           zonaInteraccion.SetActive(true);
           //cambiarNivel.enabled = true; 
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
           portalParticulas.SetActive(false);
           zonaInteraccion.SetActive(false);
           //cambiarNivel.enabled = false; 
        }
    }

}