using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
// just a test scritp that moves a bloon to the right til it meets a testturn
public class BloonTestMover : MonoBehaviour
{
    [SerializeField] private float unitsDown = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private string turnObjectName = "TestTurn";

    private enum Phase { MovingRight, MovingDown, Done }
    private Phase phase = Phase.MovingRight;

    private Vector2 phaseTarget;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    private void FixedUpdate()
    {
        switch (phase)
        {
            case Phase.MovingRight:
                rb.MovePosition(rb.position + Vector2.right * speed * Time.fixedDeltaTime);
                break;

            case Phase.MovingDown:
                Vector2 newPos = Vector2.MoveTowards(rb.position, phaseTarget, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
                if (newPos == phaseTarget)
                {
                    phase = Phase.Done;
                }
                break;

            case Phase.Done:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (phase != Phase.MovingRight)
        {
            return;
        }

        if (other.gameObject.name != turnObjectName)
        {
            return;
        }

        phase = Phase.MovingDown;
        phaseTarget = rb.position + Vector2.down * unitsDown;
    }
}