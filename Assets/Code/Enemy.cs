using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Transform target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "playerChar")
        {
            target = other.transform;

            Debug.Log(target);
        }
    }
}
