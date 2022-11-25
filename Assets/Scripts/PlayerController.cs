using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 10f;
    public float jumpForce = 4f;

    private Rigidbody2D _body;
    private SpriteRenderer _renderer;
    private Animator _animator;

    private float _speed;
    
    private bool _isGrounded;
    private bool _isJumping;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        _speed = Input.GetAxisRaw("Horizontal") * runSpeed;
        _animator.SetFloat(Speed, Mathf.Abs(_speed));

        _animator.SetBool(IsMoving, _body.velocity.x > 0.1f);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _renderer.flipX = false;
            MoveForward();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _renderer.flipX = true;
            MoveBackward();
        }
        if (Input.GetKey(KeyCode.UpArrow) && _isGrounded)
        {
            _isJumping = true;
        }
    }

    private void MoveForward()
    {
        var right = transform.right;
        _body.velocity += new Vector2(right.x * runSpeed, right.y * runSpeed) * Time.deltaTime;
    }

    private void MoveBackward()
    {
        var right = transform.right;
        _body.velocity -= new Vector2(right.x * runSpeed, right.y * runSpeed) * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool(IsJumping, false);
            _animator.SetBool(IsGrounded, true);
            _isGrounded = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _animator.SetBool(IsGrounded, false);
            _isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _isJumping = false;
            _animator.SetBool(IsJumping, true);
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
