using UnityEngine;

public class RecoilCamera : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private PlayerWeaponManager player_script;

    // [Header ("HipFire Recoil")]
    // [SerializeField] private float recoilX;
    // [SerializeField] private float recoilY;
    // [SerializeField] private float recoilZ;
    // [SerializeField] private float aimRecoilX;
    // [SerializeField] private float aimRecoilY;
    // [SerializeField] private float aimRecoilZ;
    
    // [Header ("Settings Recoil")]
    // [SerializeField] private float snappiness;
    // [SerializeField] private float returnTime;
    
    private Vector3 currentRotation;
    private Vector3 targetRotation;
    private bool isAiming;
    private int index;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isAiming = player_script.isAiming;
        index = (player_script.activeWeaponIndex>=0)?player_script.activeWeaponIndex : 0;

        
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, player_script.startingWeapons[index].returnTime * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, player_script.startingWeapons[index].snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void recoilFire(){
        if (isAiming){
            targetRotation += new Vector3(
                player_script.startingWeapons[index].aimRecoilX, 
                Random.Range(-player_script.startingWeapons[index].aimRecoilY, 
                player_script.startingWeapons[index].aimRecoilY), 
                Random.Range(-player_script.startingWeapons[index].aimRecoilZ, 
                player_script.startingWeapons[index].aimRecoilZ));
        }else{
            targetRotation += new Vector3(
                player_script.startingWeapons[index].recoilX, 
                Random.Range(-player_script.startingWeapons[index].recoilY, 
                player_script.startingWeapons[index].recoilY), 
                Random.Range(-player_script.startingWeapons[index].recoilZ, 
                player_script.startingWeapons[index].recoilZ));
        }
        
    }
}
