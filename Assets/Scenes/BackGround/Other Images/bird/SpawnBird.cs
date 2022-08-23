using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBird : MonoBehaviour
{
    public int randomnumberfromstart;
    public GameObject Bird, BirdRange;

    private GameObject birdContainer;

	public bool ready = true;

    private void Start()
    {
        birdContainer = GameObject.FindGameObjectWithTag("BirdContainer");
    }

    private void Update()
	{
        if (ready)
		StartCoroutine(Spawn());
	}

    IEnumerator Spawn()
    {
		ready = false;

		randomnumberfromstart = Random.Range(5, 20); // 8 30

		yield return new WaitForSeconds(randomnumberfromstart);

		float y = Random.Range(-1f, 6f);
		transform.position = new Vector3(transform.position.x, y, transform.position.z);

        if (Random.Range(0, 3) == 0)
        {
            Instantiate(BirdRange, transform.position, Quaternion.identity, birdContainer.transform);
        }
        else
        {
            Instantiate(Bird, transform.position, Quaternion.identity, birdContainer.transform);
        }

		ready = true;
        
    }
}
