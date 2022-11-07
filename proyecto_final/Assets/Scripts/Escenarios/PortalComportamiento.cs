using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Funcion para usar cambio de escenas
using UnityEngine.SceneManagement;

public class PortalComportamiento : MonoBehaviour
{
    //Vincular variables de Premio
    public Premio premio1;
    public Premio premio2;
    //Portal
    public GameObject portalParticulas;
    public GameObject zonaInteraccion;
    public int resultado;

    void Start()
    {
        //Detecta la Escena/Nivel en la que estamos y devulve su numero de indice
        int Scene = SceneManager.GetActiveScene().buildIndex;

        if(Scene == 1)
        {
            //Activa las funciones del portal
            portalParticulas.SetActive(true);
            zonaInteraccion.SetActive(true);
        }
        else
        {
            //Desactiva las funciones del portal
            portalParticulas.SetActive(false);
            zonaInteraccion.SetActive(false);
        }

        
    }

    void Update()
    {
        //Leer variable puntos de script Premio
        int puntosPremio1 = premio1.puntos;
        int puntosPremio2 = premio2.puntos;

        //Suma los puntos obtenidos
        resultado = puntosPremio1 + puntosPremio2;

        activarPortal();
    }

    public void activarPortal()
    {
        if (resultado == 2)
        {
           portalParticulas.SetActive(true);
           zonaInteraccion.SetActive(true);
        }
    }
}