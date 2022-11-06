using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_and_Damage : MonoBehaviour
{
	public float vidaActual;
	public int vidaMax;
	public bool invencible = false;
	public float tiempo_invencible = 1f;
	public float tiempo_frenado = 0.3f;
	public Image BarraDeVida; 

	void Start()
	{
		vidaActual = vidaMax;
	}


	void Update()
	{
		VerVida();
	}

	public void RestarVida(int cantidad)
	{
		if (!invencible && vidaActual >0)
		{
			vidaActual -= cantidad;
			StartCoroutine(Invulnerabilidad());
			// StartCoroutine(FrenarVelocidad());
			if (vidaActual == 0)
			{
				GetComponent<Respawn>().RespawnPoint();
				vidaActual = 100;

			}
		}
	}

	IEnumerator Invulnerabilidad()
	{
		invencible = true;
		yield return new WaitForSeconds(tiempo_invencible);
		invencible = false;
	}

	// IEnumerator FrenarVelocidad()
	// {
	// 	var VelocidadActual = GetComponent<logicaPersonaje>().velocidadInicial;
	// 	GetComponent<logicaPersonaje>().velocidadInicial = 0;
	// 	yield return new WaitForSeconds(tiempo_frenado);
	// 	GetComponent<logicaPersonaje>().velocidadInicial = VelocidadActual;
	// }

	public void VerVida()
	{
		BarraDeVida.fillAmount = vidaActual / vidaMax;
	}
}
