using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bananaSpawner : MonoBehaviour
{
    public GameObject collectible ;

    public Timer timer;

    public int nbEntities = 2;
    public int maxEntities = 10;

	private float frequency = 10.0f ;
	private float t = .0f;

    // Update is called once per frame
    void Update()
    {
		t += Time.deltaTime ;

		if( t >= frequency )
		{
			t = 0.0f ;
			if ( (nbEntities < maxEntities) && (nbEntities >= 2))
			{
            	Spawn();
        	}
		}
        
        
    }

    public void Spawn(){
		Transform bananaLeader = GameObject.FindGameObjectsWithTag("Leader")[0].transform;
        Vector2 spawnPosition = bananaLeader.position;
        GameObject g = Instantiate(collectible, spawnPosition, Quaternion.identity, gameObject.transform);
		g.transform.tag = "Untagged";
		
        nbEntities++;
    }


	public void DecreaseNumber(){
        nbEntities--;
    }
}
