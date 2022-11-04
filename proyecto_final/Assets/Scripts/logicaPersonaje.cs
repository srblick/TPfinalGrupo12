using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaPersonaje : MonoBehaviour
{
    public int velCorrer;    

    public bool conArma;

    public float velocidadMovimiento = 5.0f;
    public float velocidadInicial = 5.0f;
    public float velocidadRotacion = 200.0f;
    private Animator anim;
    public float x,y;

    public Rigidbody rb;
    public float fuerzaDeSalto = 4; 
    public bool puedoSaltar;

    public bool estoyAtacando;
    public bool avanzoSolo;
    public float impulsoDeGolpe = 10f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if(estoyAtacando == false)
        {
            transform.Rotate(0,x * Time.deltaTime*velocidadRotacion,0);
            transform.Translate(0,0,y * Time.deltaTime * velocidadMovimiento);
        }
 

        if (avanzoSolo)
        {
            rb.velocity = transform.forward * impulsoDeGolpe;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && puedoSaltar && !estoyAtacando)
        {
            velocidadMovimiento = velCorrer;
            /*if (y > 0)
            {
                anim.SetBool("correr", true);
            }
            else
            {
                anim.SetBool("correr", false);
            }*/
        }
        else
        {
            //anim.SetBool("correr", false);
            if (puedoSaltar)
            {
                velocidadMovimiento = velocidadInicial;
            }
        }

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Return) && puedoSaltar && estoyAtacando == false)
        {
            if (conArma)
            {
                anim.SetTrigger("golpeo2");
                estoyAtacando = true;
            }
            else
            {
                anim.SetTrigger("golpeo");
                estoyAtacando = true;
            }
        }

        anim.SetFloat("VelX",x);
        anim.SetFloat("VelY",y);

        if(puedoSaltar)
        {
            if(!estoyAtacando)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetBool("salte",true);
                    rb.AddForce(new Vector3(0,fuerzaDeSalto,0),ForceMode.Impulse);
                }
            }

            anim.SetBool("tocoSuelo",true);
        }
        else
        {
            EstoyCayendo();
        }
    }
    public void EstoyCayendo()
    {
        anim.SetBool("tocoSuelo", false);
        anim.SetBool("salte", false);
    }
    public void DejeDeGolpear()
    {
        estoyAtacando = false;

    }
    public void AvanzoSolo()
    {
        avanzoSolo = true;
    }
    public void DejoDeAvanzar()
    {
        avanzoSolo = false;
    }
}