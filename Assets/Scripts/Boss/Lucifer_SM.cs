using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucifer_SM : BossStateMachine
{
    public Rigidbody2D boss_rb;
    public Enemy enemy;
    public PolygonCollider2D _collider;

    [HideInInspector] public Vector3 presentPlayerPos;
    [HideInInspector] public Vector3 lookPlayerPos;
    [HideInInspector] public bool canChangeAttack;
    [HideInInspector] public string AttackName;
    [HideInInspector] public int moveSpeed;

    public ObjectFactory objectFactory;
    public GameObject _Point;
    public List<GameObject> firePoints = new List<GameObject>();

    [SerializeField] GameObject prefab;

    float bulletSpeed = 15f;

    List<string> ListOfAttackNames = new List<string>();

    Transform player;

    // Rotation offset 
    Quaternion flipRot => transform.rotation * Quaternion.Euler(0, 0, 180f);

    void Start()
    {
        canChangeAttack = false;

        player = Utilities.Player.transform;

        ListOfAttackNames.Add("DashSpinAttack");
        ListOfAttackNames.Add("ArrowAttack");
        ListOfAttackNames.Add("ArrowPointAttack");
        ListOfAttackNames.Add("DashAttack");

        PickAnAttack();

     
    }

    // ---------------------------
    // ATTACK PICKING
    // ---------------------------
    public void PickAnAttack()
    {
        if (ListOfAttackNames.Count <= 1) return;

        string newAttack;

        do
        {
            newAttack = ListOfAttackNames[Random.Range(0, ListOfAttackNames.Count)];
        }
        while (newAttack == AttackName);

        AttackName = newAttack;
    }

    // ---------------------------
    // LOOK AT PLAYER
    // ---------------------------
    public void LookAtPlayer(float rotationSpeed)
    {
        lookPlayerPos = player.position;

        Vector2 dir = (lookPlayerPos - transform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    // ---------------------------
    // BULLET SPAWN 
    // ---------------------------
    GameObject SpawnBullet(Vector3 pos, Quaternion rot, Vector2 velocity)
    {
        GameObject bulletGO = objectFactory.SpawnObject(ObjectType.Icicle, pos, rot);

        if (bulletGO.TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = velocity;

        if (bulletGO.TryGetComponent(out IcicleBullet bullet))
            bullet.SetFactory(objectFactory);

        return bulletGO;
    }

    // ---------------------------
    // TRIANGLE POINTS
    // ---------------------------
    public List<GameObject> TrianglePoints()
    {
        List<GameObject> trianglePoints = new List<GameObject>();

        var path = _collider.GetPath(0);

        for (int i = 0; i < path.Length; i++)
        {
            Vector2 local = path[i];

            Vector2 worldPos =
                (Vector2)(transform.rotation * local) +
                (Vector2)transform.position;

            GameObject go = Instantiate(prefab, worldPos, transform.rotation, transform);

            trianglePoints.Add(go);
        }

        return trianglePoints;
    }

    // ---------------------------
    // ATTACKS
    // ---------------------------
    public IEnumerator ArrowAttack()
    {
        List<GameObject> bullets = new List<GameObject>();

        foreach (GameObject fp in firePoints)
        {
            var bullet = SpawnBullet(
                fp.transform.position,
                flipRot,
                fp.transform.up * bulletSpeed
            );

            bullets.Add(bullet);
        }

        yield return new WaitForSeconds(1.5f);

        Vector2 target = player.position;

        foreach (GameObject b in bullets)
        {
            if (b.TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 dir = (target - (Vector2)b.transform.position).normalized;
                rb.linearVelocity = dir * (bulletSpeed + 5);
            }
        }
    }

    public void DashSpinAttack()
    {
        foreach (GameObject fp in firePoints)
        {
            SpawnBullet(
                fp.transform.position,
                flipRot,
                fp.transform.up * bulletSpeed
            );
        }
    }

    public IEnumerator ArrowPointAttack()
    {
        List<GameObject> bullets = new List<GameObject>();

        foreach (GameObject fp in firePoints)
        {
            var bullet = SpawnBullet(fp.transform.position, flipRot, Vector2.zero);
            bullets.Add(bullet);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (GameObject b in bullets)
        {
            if (b.TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 dir = (_Point.transform.position - b.transform.position).normalized;
                rb.linearVelocity = dir * (bulletSpeed - 5);
            }
        }
    }

    public void StationaryAttack()
    {
        foreach (GameObject fp in firePoints)
        {
            SpawnBullet(
                fp.transform.position,
                flipRot,
                fp.transform.up * (bulletSpeed - 14)
            );
        }
    }

    public void DashAttack()
    {
        foreach (GameObject fp in firePoints)
        {
            Vector2 dir = (transform.position - fp.transform.position).normalized;

            SpawnBullet(
                fp.transform.position,
                flipRot,
                dir * (bulletSpeed - 5)
            );
        }
    }

    // ---------------------------
    // POINT SPAWN
    // ---------------------------
    public GameObject Point()
    {
        Vector2 newPos = (Vector2)transform.position + (Vector2)transform.up * 5f;

        GameObject go = Instantiate(prefab, newPos, transform.rotation, transform);

        return go;
    }

    // ---------------------------
    // DESTROY CHILDREN (SAFE)
    // ---------------------------
    public void DestroyAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.name.Contains("Lucifer Prefab"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}