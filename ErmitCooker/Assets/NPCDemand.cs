using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDemand : MonoBehaviour
{
    public List<Item> itemsToPick;
    public SpriteRenderer itemShowed;
    private Item askedItem;
    public float patienceTime;

    private void Start()
    {
        int i = Random.Range(0, itemsToPick.Count);
        askedItem = itemsToPick[i];
        itemShowed.sprite = askedItem.sprite;
    }

    private void Update()
    {
        patienceTime -= Time.deltaTime;
        if( patienceTime <= 0 )
        {
            rageQuit();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if(UIManager.Instance.UIPlayer.inventory.inventory.Contains(askedItem))
            {
                UIManager.Instance.UIPlayer.inventory.RemoveItem(askedItem);
                happyQuit();
            }
        }
    }

    private void happyQuit()
    {
        UIManager.Instance.UIPlayer.UpdateReputBar(1);
        DestroyNPC();
    }

    private void rageQuit()
    {
        UIManager.Instance.UIPlayer.UpdateReputBar(-1);
        DestroyNPC();
    }

    private void DestroyNPC()
    {
        UIManager.Instance.UIPlayer.npcSpawner.addPosition(gameObject.transform.position);
        Destroy(gameObject);
    }
}
