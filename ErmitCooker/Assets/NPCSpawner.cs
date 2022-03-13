using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject NPC;
    public float timeNewClient = 1f;
    private float timer = 0f;
    public Collider2D[] colliders;
    public List<Vector3> spawnPoints;

    private void Start()
    {
        UIManager.Instance.UIPlayer.npcSpawner = gameObject.GetComponent<NPCSpawner>();
        float distance = endPoint.position.x - startPoint.position.x;
        float widthNPC = NPC.GetComponent<Collider2D>().bounds.extents.x;
        for( float i = startPoint.position.x + 0.5f; i < startPoint.position.x + distance; i += widthNPC + 3f)
        {
            spawnPoints.Add(new Vector3(i, startPoint.position.y, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if( timer >= timeNewClient )
        {
            timer = 0f;
            if(spawnPoints.Count > 0)
            {
                int randomSeat = Random.Range(0, spawnPoints.Count);
                Vector3 spawnPos = spawnPoints[randomSeat];

                Instantiate(NPC, spawnPos, Quaternion.identity);
                spawnPoints.RemoveAt(randomSeat);
            }
        }
    }

    public void addPosition( Vector3 position )
    {
        spawnPoints.Add(position);
    }
}
