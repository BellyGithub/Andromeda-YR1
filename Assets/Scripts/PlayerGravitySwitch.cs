using UnityEngine;

public class PlayerGravitySwitch : MonoBehaviour
{
    public bool isGravityUpwards = false;
    public GameObject player;
    public LayerMask ground;
    private Rigidbody2D playerRb;
    bool isGround;
    void SwitchGravity()
    {
        isGravityUpwards = !isGravityUpwards;
        playerRb.gravityScale *= -1;
        Rotate rotateScript = player.GetComponent<Rotate>();
        rotateScript.Rotate180();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        isGround = playerRb.IsTouchingLayers(ground);
        if (Input.GetKeyDown(KeyCode.F) && isGround)
        {
            SwitchGravity();
        }
    }
}