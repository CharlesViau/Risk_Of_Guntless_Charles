using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody2D))]

public class Move : MonoBehaviour
{
    public bool isAI;
    private Rigidbody2D rb2D;
    [SerializeField] float Speed;
    public Vector2 movVector { get; private set; }
    private Direction direction;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if(isAI)
        {
            GetComponent<TimerEvent>().StartEvent();
        }
    }

    public void MoveInDirection(Vector2 moveDir)
    {
        rb2D.velocity = new Vector2(moveDir.x, moveDir.y) * Speed;
    }

    public void AIMoveInDirection()
    {

        rb2D.velocity = new Vector2(movVector.x, movVector.y) * Speed;
    }

    public void MovePlayer(CallbackContext v)
    {
        movVector = v.ReadValue<Vector2>();
        MoveInDirection(movVector);
    }

    public void AIChooseDirection()
    {
        direction = (Direction)Random.Range(1, 4);
        switch (direction)
        {
            case Direction.Up:
                movVector = new Vector2(0, 1);
                break;
            case Direction.Down:
                movVector = new Vector2(0, -1);
                break;
            case Direction.Left:
                movVector = new Vector2(-1, 0);
                break;
            case Direction.Right:
                movVector = new Vector2(1, 0);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isAI)
        { 
            AIChooseDirection();
        }
    }

}
