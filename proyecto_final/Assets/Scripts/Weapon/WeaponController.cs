using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType{
    Manual,
    Automatic
}
public class WeaponController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private Transform weaponMuzzle;// transform de la boquilla del arma
    [SerializeField] private Animator animator;
    
    [Header ("General")]
    [SerializeField] private LayerMask hittableLayers;//capas que puede disparar

    [Header ("Shoot Parameters")]
    [SerializeField] private ShootType shootType;// tipo de disparo entre automatico y manual
    [SerializeField] private float fireRange=200; // rango de disparo
    [SerializeField] private float recoilForce=4f;// fuerza de retroceso
    [SerializeField] private float fireRate=0.8f;// tiempo entre disparo
    public int totalAmmo=30;// total de municion
    public int magazineCapacity=8;// capacidad del cargador

    [Header ("HipFire Recoil")]// configuracion de recoil 
    public float recoilX;//sirve para saber cuanto se lavanta la mira
    public float recoilY;//sirve para mover la mira hacia los lados
    public float recoilZ;//sirve para girar un poco la camara
    public float aimRecoilX;// las mismas variables version apuntado
    public float aimRecoilY;
    public float aimRecoilZ;
    
    [Header ("Settings Recoil")]// variables para medir el tiempo en el cual el recoil vuelve la posicion normal
    public float snappiness;
    public float returnTime;

    [Header ("Reload Parameters")]//tiempo que tarda en recargar
    [SerializeField] float reloadTime=1.5f;

    [Header ("Sounds & Visuals")]//variables que añaden particulas o sonidos
    [SerializeField] private ParticleSystem flashEffect;
    [SerializeField] private GameObject hitImpact;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private AudioClip reloadSfx;
    [SerializeField] private AudioClip bulletFall;
    // [SerializeField] private AudioClip shootSfx;
    [SerializeField] private AudioClip dryBulletsSfx;
    [SerializeField] private GameObject bulletHolePrefab;

    public int currentAmmo {get; private set;}
    private float currentRecoilForce;
    private float lastTimeShoot=float.NegativeInfinity;
    private bool isReloading=false;
    private Transform cameraPlayerTransform;
    private AudioSource audioSource;
    private RecoilCamera recoilCamera_script;
    private PlayerWeaponManager player_script;

    //funcion que se ejecuta antes que el start
    private void Awake() {
        currentAmmo=magazineCapacity;// igual la capacidad a las balas actuales
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo,totalAmmo);// se invoca al evento actualizar balas
    }
    // private void OnDisable() {// no estaria funcionando esto :/
    //     lastTimeShoot=2.5f; // para que espere la animacion de salida antes de disparar

    // }
    void Start()
    {   
        if(GetComponent<Animator>() != null){
            animator= GetComponent<Animator>();// se asigna el componente animator
        }
        
        currentRecoilForce=recoilForce;
        player_script = GameObject.Find("Player").GetComponent<PlayerWeaponManager>();
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        audioSource = GetComponent<AudioSource>();
        recoilCamera_script = GameObject.Find("CameraRecoil").GetComponent<RecoilCamera>();
    }

    void Update()
    {
        if(player_script.isAiming){// si está apuntando se activa la animacion y se reduce la fuerza del recoil
            currentRecoilForce = recoilForce/2;
            animator.SetBool("isAiming",true);
        }else{
            currentRecoilForce = recoilForce;
            animator.SetBool("isAiming",false);
        }
        if(shootType == ShootType.Manual){// si el tipo de tiro es manual se utiliza un getButtonDown para leer un sola vez el clic
            if (Input.GetButtonDown("Fire1") && !isReloading){
                tryShoot();// se llama al metodo intetar disparar
            }
        }
        if(shootType == ShootType.Automatic){//si el tipo de tiro es automatico se utiliza getButton para leer mientras este presiona el clic
            if (Input.GetButton("Fire1") && !isReloading){
                tryShoot();
            }
        }
        if(Input.GetButtonDown("Fire1")&&currentAmmo<=0){//si intenta disparar sin balas se reproduce el sonido del gatillo
            audioSource.PlayOneShot(dryBulletsSfx);
        }
        if(Input.GetKeyDown(KeyCode.R) && totalAmmo>=1 && currentAmmo!=magazineCapacity){
            if(!isReloading){
                StartCoroutine(reload());
            }
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition,Vector3.zero,Time.deltaTime*5f);
    }

    //Metodo que intenta disparar
    private void tryShoot(){
        if(lastTimeShoot+fireRate<Time.time){//si el tiempo del ultima vez que disparo mas el tiempo entre disparo es menor al tiempo actual
            if(currentAmmo>=1){//si la cantidad de balas es mayor a 0
                handleShoot();//dispara
                currentAmmo-=1;//se resta un bala
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo,totalAmmo);//se invoca el evento que actualiza las balas
            }
        }
    }

    //Método que dispara
    private void handleShoot(){
        recoilCamera_script.recoilFire();// llama al metodo recoil del script de la camara
        audioSource.Play();// se reproduce el sonido de disparo
        audioSource.PlayOneShot(bulletFall);// se reproduce el sonido de la bala cayendo
        flashEffect.Play();// se activa las particulas de la boquilla
        addRecoil();// se el efecto visual del retroceso

        RaycastHit hit;//se define un raycasthit
        //se configura el rayo para que salga del centro de la camara hacia adelanto en un rango determinado
        if(Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange,hittableLayers)){
            TrailRenderer trail = Instantiate(bulletTrail,weaponMuzzle.position,Quaternion.identity);// se instancia la cola de bala
            GameObject hitImpactClone = Instantiate(hitImpact, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));//se instancia las particulas del impacto de bala
            Destroy(hitImpactClone,0.5f);// se destruyen las particulas despues de un cierto tiempo
            StartCoroutine(spawnTrail(trail,hit));// inicia corrutina y se pasa como agurmento el trail y hit
            //codigo que hace daño al enemigo
            if(hit.transform.tag == "Enemy"){// si impacta al enemigo se llama a la funcion que daña al enemigo
                hit.transform.GetComponent<EnemyBehaviour>().sufferDamage(1);
            }else{
                
            }
        }
        lastTimeShoot=Time.time;// se asigna el tiempo actual a una variable
    }

    //metodo que añade el visual de retroceso al arma
    private void addRecoil(){
        transform.Rotate(currentRecoilForce*4,0f,0f);
        transform.position = transform.position + transform.forward * (currentRecoilForce/50f); 
    }

    //IEnumarator que añade la el efecto de cola a la bala
    private IEnumerator spawnTrail(TrailRenderer trail,RaycastHit hit){
        float time=0;// se define una variable tiempo y se iniciliza en 0
        Vector3 startPosition = trail.transform.position;

        while (time<1){
            trail.transform.position = Vector3.Lerp(startPosition,hit.point,time);// se mueve el trail desde la boquilla hasta el punto de impacto
            time+= Time.deltaTime /trail.time;
            
            yield return null;
        }
        trail.transform.position = hit.point;
        
        GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        Destroy(bulletHoleClone,5f);
        Destroy(trail.gameObject,trail.time);
    }

    //IEnumerator para recargar el arma
    private IEnumerator reload(){
        player_script.setIsAiming(false);// se desactiva el apuntado
        animator.SetTrigger("Reloading");//inicia la animacion
        isReloading=true;//se le asigna un valor true a la variable
        int neededAmmo = magazineCapacity-currentAmmo;//la cantidad de bala que necesita es igual al cagador menos balas actualas
        audioSource.PlayOneShot(reloadSfx);// se reproduce el sonido de recarga
        yield return new WaitForSeconds(reloadTime);// se epera el tiempo de recarga
        //condiciones para calcular las balas que necesita recargar
        if(totalAmmo>magazineCapacity){
            totalAmmo-= neededAmmo;
            currentAmmo+= neededAmmo;
        }else if(totalAmmo>0){
            if(neededAmmo<totalAmmo){
                totalAmmo-= neededAmmo;
                currentAmmo+= neededAmmo;
            }else{
                currentAmmo+= totalAmmo;
                totalAmmo= 0;
            }
        }
        
        isReloading=false;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo,totalAmmo);
    }

    public void reloadAmmo(int bullets){
        totalAmmo += bullets;
    }
    public void hide(){
        animator.SetTrigger("Hiding");
    }
}
