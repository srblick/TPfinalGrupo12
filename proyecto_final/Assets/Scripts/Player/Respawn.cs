using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Respawn : MonoBehaviour
{

  [SerializeField] Transform spawnPoint;

  [SerializeField] float spawnValue;


 /* void Update()
  {
    if(player.transform.position.y < -spawnValue)
    {
        RespawnPoint();
    }
  }*/

  public void RespawnPoint()
  {
      GetComponent<PlayerController>().enabled = false;
      GetComponent<CharacterController>().enabled = false;
      transform.position = spawnPoint.position;
      GetComponent<PlayerController>().enabled = true;
      GetComponent<CharacterController>().enabled = true;
  }
}
