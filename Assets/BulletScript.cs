using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public void ShootWhenActive(Vector3 direction, float bulletSpeed)
    {
        Destroy(this.gameObject,2f);
        this.GetComponent<Rigidbody>().AddForce((direction-transform.position)*bulletSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bot"))
            Destroy(this.gameObject);
        else Destroy(this.gameObject);
    }
}
