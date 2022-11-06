using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponEnemyController : MonoBehaviour
{

    [Header ("References")]
    [SerializeField] private Transform weaponMuzzle;// tranform de la boquilla del arma
    
    [Header ("General")]
    [SerializeField] private LayerMask hittableLayers;// capas que el enemy puede apuntar, lo ideal seria a todos menos a otros enemigos

    [Header ("Shoot Parameters")]// Parametros de disparo
    [SerializeField] private float fireRange=200;// el rango que mide el raycast del arma
    [SerializeField] private float recoilForce=4f;// variable que sirve para hacer una animacion de recoil, 4f para pistolas, 1f para fusiles
    [SerializeField] private float fireRate=0.8f;// tiempo de espera entre disparo. Cambiar el valor para que pase de arma automatica a manual.

    [Header ("HipFire Recoil")]// configuracion del recoil
    public float recoilX;// sirve para saber cuanto se va a levantar la mira del arma
    public float recoilY;// cuanto se va a desviar la mira hacia los lados
    public float recoilZ;// mejor dejarlo en 0. Es el giro de la camara, pero funciona mejor con el player

    
    [Header ("Settings Recoil")]
    public float snappiness;//dejarlo en 6
    public float returnTime;//dejarlo en 2, son el tiempo en que el arma se recupera del recoil y vuelve a su posicion normal


    [Header ("Sounds & Visuals")]
    [SerializeField] private ParticleSystem flashEffect;// poner el prefab del sistema de particulas. Destello de luz que hace al dispara
    [SerializeField] private GameObject hitImpact; // poner el prefab del hitImpact. particulas que se muestran donde impacto la bala, se puede sacar para optimizar
    [SerializeField] private TrailRenderer bulletTrail; // poner el prefab del bullet trail. Particulas que simulan el rastro de las balas
    [SerializeField] private AudioClip ShootSfx;
    [SerializeField] private GameObject bulletHolePrefab;// poner el prefab del bulletHole. Sprite que muestra donde impacto la bala, se puede sacar para optimizar

    private Vector3 currentRotation;// variables para el recoil
    private Vector3 targetRotation;// variables para el recoil

    private float lastTimeShoot=0;// variable que sirve para verificar la ultima vez que se disparó. Se utiliza con rateFire

    private AudioSource audioSource;



    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        // if(shootType == ShootType.Manual){
        //     if (Input.GetButtonDown("Fire1") && !isReloading){
        //         tryShoot();
        //     }
        // }
        // if(shootType == ShootType.Automatic){
            if (Input.GetKey(KeyCode.P)){// codigo de prueba
                tryShoot();
            }
        // }
        // if(Input.GetKeyDown(KeyCode.R) && totalAmmo>=1 && currentAmmo!=magazineCapacity){
        //     if(!isReloading){
        //         StartCoroutine(reload());
        //     }
        // }
            
        transform.localPosition = Vector3.Lerp(transform.localPosition,Vector3.zero,Time.deltaTime*5f);//hace que vuelva a su posicion inicial

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnTime * Time.deltaTime); 
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    private void tryShoot(){// llamar esta funcion para disparar
        if(lastTimeShoot+fireRate<Time.time){

            handleShoot();

        }
    }
    private void handleShoot(){
        recoilFire();
        audioSource.PlayOneShot(ShootSfx);
        flashEffect.Play();
        addRecoil();
        RaycastHit hit;
        // al weaponMuzzle.forward se le puede sumar un valor aleatorio para que no acierte siempre al player
        if(Physics.Raycast(weaponMuzzle.position, weaponMuzzle.forward, out hit, fireRange,hittableLayers)){
            TrailRenderer trail = Instantiate(bulletTrail,weaponMuzzle.position,Quaternion.identity);
            GameObject hitImpactClone = Instantiate(hitImpact, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
            Destroy(hitImpactClone,0.5f);
            StartCoroutine(spawnTrail(trail,hit));
            //codigo que hace daño al player abajo
        }
        lastTimeShoot=Time.time;
    }

    private void addRecoil(){
        transform.Rotate(recoilForce*4,0f,0f);
        transform.position = transform.position + transform.forward * (recoilForce/50f); 
    }

    private IEnumerator spawnTrail(TrailRenderer trail,RaycastHit hit){
        float time=0;
        Vector3 startPosition = trail.transform.position;

        while (time<1){
            trail.transform.position = Vector3.Lerp(startPosition,hit.point,time);
            time+= Time.deltaTime /trail.time;
            
            yield return null;
        }
        trail.transform.position = hit.point;
        
        GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        Destroy(bulletHoleClone,5f);
        Destroy(trail.gameObject,trail.time);
    }

    public void recoilFire(){

        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

}
