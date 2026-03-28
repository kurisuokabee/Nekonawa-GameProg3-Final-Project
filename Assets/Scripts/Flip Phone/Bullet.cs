using UnityEngine;

public class Bullet : MonoBehaviour
{   
    [SerializeField] float life = 5f;
    float t;

    ObjectFactory factory;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        t = 0f;
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= life) ReleaseBullet();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") )
        {
            collision.GetComponent<Enemy>().TakeDamage(10);

            ReleaseBullet();
        }
    }

    void ReleaseBullet()
    {   
        //Checking if the object is active in hierarchy to avoid double releasing
        if(!gameObject.activeInHierarchy) return;

        rb.linearVelocity = Vector3.zero;
        factory.Release(ObjectType.Bullet, gameObject);
    }

    public void SetFactory(ObjectFactory factory)
    {
        this.factory = factory;
    }
}
