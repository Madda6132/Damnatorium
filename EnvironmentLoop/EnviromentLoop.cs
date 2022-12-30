using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TrainTrack
{
    public class EnviromentLoop : MonoBehaviour
    {
        [SerializeField] List<GameObject> enviromentPiecePrefab = new List<GameObject>();
        [SerializeField] GameObject startObj = null;
        [SerializeField] GameObject endObj = null;
        //Pool of Pieces
        Queue<GameObject> reserveEnviromentPiecePool = new Queue<GameObject>();
        Queue<GameObject> activeEnviromentPiecePool = new Queue<GameObject>();

        Transform currentSpawnPoint;
        float timePastSenceSpawning = 0;
        private void Awake()
        {
            startObj.transform.LookAt(endObj.transform);
            endObj.transform.LookAt(startObj.transform);
            currentSpawnPoint = startObj.transform;

            startObj.GetComponent<EnviromentLoopStart>().SetSettings(this);
            endObj.GetComponent<EnviromentLoopEnd>().SetSettings(this);

            CreateEP();


        }
        private void Update()
        {
            timePastSenceSpawning += Time.deltaTime;
            if (timePastSenceSpawning > 0.5f) CreateEP();
            
        }

        public void CreateEP()
        {
            
            if (reserveEnviromentPiecePool.Count > 0)
            {
                GameObject obj = reserveEnviromentPiecePool.Dequeue();
                if (timePastSenceSpawning > 0.5f)
                {
                    currentSpawnPoint = startObj.transform;

                }
                obj.transform.position = currentSpawnPoint.position;
                TrackLocation(obj);
                obj.SetActive(true);
                
            }
            else
            {
                int ePIndex = Random.Range(0, enviromentPiecePrefab.Count);
                GameObject currentEP = Instantiate(enviromentPiecePrefab[ePIndex], currentSpawnPoint.position, 
                    startObj.transform.rotation, transform);
                TrackLocation(currentEP);
                currentEP.GetComponent<EnviromentPiece>().SetSetting(endObj.transform);


            }

            timePastSenceSpawning = 0;
        }


        private void TrackLocation(GameObject obj)
        {
            activeEnviromentPiecePool.Enqueue(obj);
            currentSpawnPoint = obj.GetComponent<EnviromentPiece>().spawnPos;
        }


        public void HideAndPoolFirstPiece()
        {
            activeEnviromentPiecePool.Peek().SetActive(false);
            reserveEnviromentPiecePool.Enqueue(activeEnviromentPiecePool.Dequeue());
        }
    }
}