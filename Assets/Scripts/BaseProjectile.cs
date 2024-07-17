using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Recorder.OutputPath;

public class BaseProjectile : MonoBehaviour
{
    public float speed = 15f;  // ����ü �ӵ�
    public float hitOffset = 0f;  // �浹 �� ������
    public bool UseFirePointRotation;  // �浹 �� ȸ�� ���� ����
    public Vector3 rotationOffset = new Vector3(0, 0, 0);  // ȸ�� ������
    public GameObject hit;  // �浹 ȿ�� ������Ʈ
    public GameObject flash;  // �߻� ȿ�� ������Ʈ
    private Rigidbody2D rb;  // 2D ������ٵ�
    private CircleCollider2D cc;  // circle collider
    public GameObject[] Detached;  // �и��� ������Ʈ �迭

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }


    void FixedUpdate()
    {
        if (speed != 0)
        {
            //rb.velocity = transform.forward * speed;
            //transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    public void Destroy()
    {
        var hitInstance = Instantiate(hit, transform.position, transform.rotation);

        //Destroy hit effects depending on particle Duration time
        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            Destroy(hitInstance, hitPs.main.duration);
        }
        else
        {
            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitInstance, hitPsParts.main.duration);
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }

        Destroy(gameObject);
    }
}
