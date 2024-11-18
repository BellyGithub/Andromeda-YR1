using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    Rigidbody2D rb2D;

    public float jumpforce = 2.0f;
    public float jumpholdforce = 0.01f;

    public float movespeed = 6;
    public LayerMask ground;
    
    [SerializeField]
    private bool _isrunning = false;
    public bool IsRunning {get 
    {
        return _isrunning;
    }
    private set 
    {
        _isrunning = value;
        animator.SetBool("isrunning",value);
    }
    }
    [SerializeField]
    private bool _isattacking = false;
    public bool IsAttacking{get 
    {
        return _isattacking;
    }
    private set 
    {
        _isattacking = value;
        animator.SetBool("isattacking",value);
    }
    }
    float xMovement;
    float jumpTime = 0.0f;
    bool isGround;
    PlayerGravitySwitch GSscript;
    GameObject GameManager;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameManager = GameObject.Find("PlayerCharacter");
        GSscript = GameManager.GetComponent<PlayerGravitySwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        xMovement = Input.GetAxis("Horizontal");
        rb2D.linearVelocity = new Vector2(xMovement * movespeed, rb2D.linearVelocity.y);
        if (xMovement < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xMovement > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        isGround = rb2D.IsTouchingLayers(ground);

        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            jumpTime = Time.time + 0.2f;
            if (GSscript.isGravityUpwards == false)
            {
                rb2D.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
            }
            else
            {
                rb2D.AddForce(new Vector2(0, -jumpforce), ForceMode2D.Impulse);
            }
        }
        if (Input.GetKey(KeyCode.Space) && Time.time < jumpTime)
        {
            if (GSscript.isGravityUpwards == false)
            {
                rb2D.AddForce(new Vector2(0, jumpholdforce), ForceMode2D.Impulse);
            }
            else
            {
                rb2D.AddForce(new Vector2(0, -jumpholdforce), ForceMode2D.Impulse);
            }
        }
    }
}
