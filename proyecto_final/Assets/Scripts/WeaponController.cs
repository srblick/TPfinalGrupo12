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
    [SerializeField] private Transform weaponMuzzle;
    
    [Header ("General")]
    [SerializeField] private LayerMask hittableLayers;

    [Header ("Shoot Parameters")]
    [SerializeField] private ShootType shootType;
    [SerializeField] private float fireRange=200;
    [SerializeField] private float recoilForce=4f;
    [SerializeField] private float fireRate=0.8f;
    public int totalAmmo=30;
    public int magazineCapacity=8;

    [Header ("HipFire Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float aimRecoilX;
    public float aimRecoilY;
    public float aimRecoilZ;
    
    [Header ("Settings Recoil")]
    public float snappiness;
    public float returnTime;

    [Header ("Reload Parameters")]
    [SerializeField] float reloadTime=1.5f;

    [Header ("Sounds & Visuals")]
    [SerializeField] private ParticleSystem flashEffect;
    [SerializeField] private GameObject hitImpact;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private AudioClip reloadSfx;
    [SerializeField] private AudioClip bulletFall;
    [SerializeField] private GameObject bulletHolePrefab;

    public int currentAmmo {get; private set;}
    private float currentRecoilForce;
    private float lastTimeShoot=0;
    private bool isReloading=false;
    private Transform cameraPlayerTransform;
    private AudioSource audioSource;
    private RecoilCamera recoilCamera_script;
    private PlayerWeaponManager player_script;

    private void Awake() {
        currentAmmo=magazineCapacity;
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo,totalAmmo);
    }
    void Start()
    {   
        currentRecoilForce=recoilForce;
        player_script = GameObject.Find("Player").GetComponent<PlayerWeaponManager>();
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        audioSource = GetComponent<AudioSource>();
        recoilCamera_script = GameObject.Find("CameraRecoil").GetComponent<RecoilCamera>();
    }

    void Update()
    {
        if(player_script.isAiming){
            currentRecoilForce = recoilForce/2;
        }else{
            currentRecoilForce = recoilForce;
        }
        if(shootType == ShootType.Manual){
            if (Input.GetButtonDown("Fire1") && !isReloading){
                tryShoot();
            }
        }
        if(shootType == ShootType.Automatic){
            if (Input.GetButton("Fire1") && !isReloading){
                tryShoot();
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && totalAmmo>=1 && currentAmmo!=magazineCapacity){
            if(!isReloading){
                StartCoroutine(reload());
            }
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition,Vector3.zero,Time.deltaTime*5f);
    }

    private void tryShoot(){
        if(lastTimeShoot+fireRate<Time.time){
            if(currentAmmo>=1){
                handleShoot();
                currentAmmo-=1;
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo,totalAmmo);
            }
        }
    }
    private void handleShoot(){
        recoilCamera_script.recoilFire();
        audioSource.Play();
        audioSource.PlayOneShot(bulletFall);
        // Instantiate(flashEffect, weaponMuzzle.position,Quaternion.Euler(weaponMuzzle.forward),transform);
        flashEffect.Play();
        addRecoil();

        RaycastHit hit;
        if(Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange,hittableLayers)){
            TrailRenderer trail = Instantiate(bulletTrail,weaponMuzzle.position,Quaternion.identity);
            GameObject hitImpactClone = Instantiate(hitImpact, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
            Destroy(hitImpactClone,0.5f);
            StartCoroutine(spawnTrail(trail,hit));
            //codigo que hace daño al enemigo
        }
        lastTimeShoot=Time.time;
    }

    private void addRecoil(){
        transform.Rotate(currentRecoilForce*4,0f,0f);
        transform.position = transform.position + transform.forward * (currentRecoilForce/50f); 
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

    private IEnumerator reload(){
        isReloading=true;
        int neededAmmo = magazineCapacity-currentAmmo;
        Debug.Log("Recargando");
        audioSource.PlayOneShot(reloadSfx);
        yield return new WaitForSeconds(reloadTime);
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
        Debug.Log("Recargada");
    }

    public void reloadAmmo(int bullets){
        totalAmmo += bullets;
    }
}
