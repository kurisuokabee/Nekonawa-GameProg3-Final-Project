using UnityEngine;

public class IcicleBullet : MonoBehaviour
{   
    [SerializeField] float life = 5f;
    [SerializeField] SpriteRenderer spriteRenderer;
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
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= life) ReleaseBullet();

        if(t >= .07f) spriteRenderer.enabled = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(2);

            ReleaseBullet();
        }

        if (collision.CompareTag("Shield") )
        {
            ReleaseBullet();
        }
    }

    void ReleaseBullet()
    {   
        //Checking if the object is active in hierarchy to avoid double releasing
        if(!gameObject.activeInHierarchy) return;

        rb.linearVelocity = Vector3.zero;
        spriteRenderer.enabled = false;
        factory.Release(ObjectType.Icicle, gameObject);
        
    }

    public void SetFactory(ObjectFactory factory)
    {
        this.factory = factory;
    }
}
