using UnityEngine;
/*  assigned to a prefab, moves in dir of target until it reaches
*/
public class Dart : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float damage = 1f; //how much health it takes away from bloons
    [SerializeField] private int pierce = 2; // how many bloons it can hit before destroying itself
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private string targetTag = "Bloon";

    public float Damage => damage;
    private Vector2 travelDirection;
    private bool fired;

    public void Fire(Vector3 direction)
    {
        travelDirection = ((Vector2)direction).normalized;
        if (travelDirection.sqrMagnitude < 0.0001f)
        {
            travelDirection = transform.right;
        }

        float angle = Mathf.Atan2(travelDirection.y, travelDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle-90f);

        fired = true;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!fired)
        {
            return;
        }

        transform.position += (Vector3)(travelDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(targetTag))
        {
            return;
        }

        pierce--;
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }

        //test \/
        //Destroy(other.gameObject);
    }
}