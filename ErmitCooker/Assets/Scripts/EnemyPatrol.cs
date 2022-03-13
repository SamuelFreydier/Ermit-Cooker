using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	public float speed;
	public Transform[] waypoints;

	private Transform target;
	private int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
		int nbWaypoints = GameObject.Find("routine").transform.childCount;
		waypoints = new Transform[nbWaypoints];
        
		for (int i=0;i<waypoints.Length;i++)
			waypoints[i] = GameObject.Find("routine").transform.GetChild(i).transform;
		target = waypoints[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		int nbBananas = GameObject.Find("BananaHerd").transform.childCount;
		Transform[] bananas = new Transform[nbBananas];
        
		for (int i=0;i<bananas.Length;i++)
			bananas[i] = GameObject.Find("BananaHerd").transform.GetChild(i).transform;

		Transform bananaLeader = election(bananas);

		if (transform.tag == "Leader"){
			Vector3 dir = target.position - transform.position;
			transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

			if(Vector3.Distance(transform.position, target.position) < 0.3f){
				destPoint = (destPoint + 1) % waypoints.Length;
				target = waypoints[destPoint];
			}
		}
		else{
			Vector3 dir = bananaLeader.position - transform.position;
			transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
		}
    }


	Transform election(Transform[] bananas){
		for (int i=0; i<bananas.Length; i++){
            if (bananas[i].tag == "Leader")
				return bananas[i];
        }

		bananas[0].tag = "Leader";
		return bananas[0];
	}

}
