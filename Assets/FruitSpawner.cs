using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitSpawner : MonoBehaviour {

	public GameObject fruitPrefab;
	public Transform[] spawnPoints;

	public float minDelay = .1f;
	public float maxDelay = 1f;

	private int count;  // this is for the text display ----> it will be used for recieving data from the arduino

	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnFruits ());
	}

	IEnumerator SpawnFruits ()
	{
		while (true) {
		
			float delay = Random.Range (minDelay, maxDelay);
			yield return new WaitForSeconds (delay);

			int spawnIndex = Random.Range (0, spawnPoints.Length);
			Transform spawnPoint = spawnPoints [spawnIndex];
		
			GameObject spawnedFruit = Instantiate(fruitPrefab,spawnPoint.position,spawnPoint.rotation);
            if (spawnedFruit != null)
            {
                Destroy(spawnedFruit, 5f);
            }

        }
	
	}



}
