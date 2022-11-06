using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private int vida = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sufferDamage(int damage){
 
        vida-=damage;
        if(vida<=0){
            Destroy(gameObject);
        }
    }
}
