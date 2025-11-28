using System.Collections;
using UnityEngine;

public class NormalBulletBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private LayerMask WhatDestroysBullet;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetStraightVelocity();

        StartCoroutine(BulletLifespan());
    }

    private void OnCollisionEnter(Collision other)
    {
        //if ((WhatDestroysBullet.value & other.gameObject.layer) > 0)
        //{
        //    Debug.Log("destroyed");
        //    Destroy(gameObject);
        //}

        if (other.gameObject.GetComponent<EnemyController>())
        {
            other.gameObject.GetComponent<EnemyController>().die();
            //Destroy(other.gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

    private IEnumerator BulletLifespan()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

}
