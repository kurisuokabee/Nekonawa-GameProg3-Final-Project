using UnityEngine;

public class GunMode : MonoBehaviour, IPhone
{   
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ObjectFactory factory;

    [SerializeField] private float fireRate = 5f; // bullets per second
    private float nextFireTime = 0f;

    public void Enter()
    {
        Debug.Log("Gun Mode Activated");
    }

    public void Exit()
    {
        Debug.Log("Gun Mode Deactivated");
    }

    public void Use()
    {   
        if (Time.time < nextFireTime)
            return;

        Shoot();
        nextFireTime = Time.time + (1f / fireRate);
    }

    private void Shoot()
    {
        GameObject bulletGO = factory.SpawnObject(ObjectType.Bullet, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null) bullet.SetFactory(factory);

        if (rb != null) rb.linearVelocity = firePoint.right * 15f;
    }
}
