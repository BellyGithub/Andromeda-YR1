using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    public bool isGravityUpwards = false;
    public GameObject player;

    void SwitchGravity()
    {
        isGravityUpwards = !isGravityUpwards;
        Physics2D.gravity = new Vector2 (Physics2D.gravity.x, -Physics2D.gravity.y);
        Rotate rotateScript = player.GetComponent<Rotate>();
        rotateScript.Rotate180();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchGravity();
        }
    }
}
