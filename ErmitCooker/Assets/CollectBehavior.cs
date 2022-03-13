using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBehavior : MonoBehaviour
{
    public GameObject pressCIcon;
    public Item loot;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pressCIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pressCIcon.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if( collision.tag == "Player" && Input.GetKeyDown(KeyCode.C) && GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING)
        {
            if( gameObject.transform.parent != null )
            {
                bananaSpawner spawn = gameObject.transform.parent.gameObject.GetComponent<bananaSpawner>();
                if (spawn != null)
                {
                    spawn.DecreaseNumber();
                }
            }
            
            UIManager.Instance.UIPlayer.inventory.AddItem(loot);
            Destroy(gameObject);
        }
    }

}
