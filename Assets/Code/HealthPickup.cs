using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    GameObject uiInfo;
    // Start is called before the first frame update
    void Start()
    {
        uiInfo = GameObject.Find("Canvas");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "playerChar")
        {
            uiInfo.GetComponent<UIcontroller>().health = 100;
            Destroy(gameObject);
        }
    }
}
