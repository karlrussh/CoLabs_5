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
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((WhatDestroysBullet.value & other.gameObject.layer) > 0)
        {
            Debug.Log("destroyed");
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = transform.right * bulletSpeed;
    }

}
