using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigosBehaviour : MonoBehaviour
{
        public Transform[] points;
        private int destPoint = 0;
        private UnityEngine.AI.NavMeshAgent agent;
        [SerializeField]
        private GameObject enemy;


        void Start () {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            // Desactivo autoBracking para que no frene cuando
            // cuando se acerca a un punto de destino
            agent.autoBraking = false;

            GotoNextPoint(); // elije el primer destino

            // Genera enemidos cada 3 segundos, comienza a los 10 segundos.
            InvokeRepeating("GenerateEnemy", 3.0f, 10f);   
        }


        void GotoNextPoint() {
            // Retorna sin hacer nada si el array de puntos esta vacio.
            if (points.Length == 0)
                return;

            // configuro al agente para que valla a su proximo destino. 
            agent.destination = points[destPoint].position;

            // hago un random entre 0 y el tamano del vector de puntos
            // y guardo en desPoint para el proximo destino.
            destPoint = Random.Range(0,points.Length);
        }

        private void GenerateEnemy(){
            // Instancio un nuevo enemigo
            GameObject newEnemy = Instantiate(enemy);
            // Le asigno a la posicion de enemigo la posicion del GeneradorDeEnemigos 
            newEnemy.transform.position = transform.position;   
        }


        void Update () {
            // Cuando el agente se acerca a su destino llamo a 
            // GotoNextPoint para que elija el siguiente punto destino.
            if (agent.remainingDistance < 0.5f){
                GotoNextPoint();
            }
        }
}