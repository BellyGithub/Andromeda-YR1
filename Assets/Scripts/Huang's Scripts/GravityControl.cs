using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using TMPro;

public class GravityControl : MonoBehaviour
{
    public float gravityStep = 0.01f;
    public float minGravity = 0f;
    private float maxGravity = 0f;
    private float currentGravity;
    public TextMeshProUGUI TextGravity;
    public GameObject gameManager;
    private GravitySwitch gravitySwitchScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxGravity = Mathf.Abs(Physics2D.gravity.y);
        gravitySwitchScript = gameManager.GetComponent<GravitySwitch>();
    }

    void ToggleGravity(bool increase)
    {
        if (increase)
        {
            currentGravity = Mathf.Abs(Physics2D.gravity.y + gravityStep);
            currentGravity = Mathf.Clamp(currentGravity, minGravity, maxGravity);
        }
        else
        {
            currentGravity = Mathf.Abs(Physics2D.gravity.y - gravityStep);
            currentGravity = Mathf.Clamp(currentGravity, minGravity, maxGravity);
        }
        if (gravitySwitchScript.isGravityUpwards)
        {
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, currentGravity);
        }
        else
        {
            Physics2D.gravity = new Vector2(Physics2D.gravity.x, -currentGravity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TextGravity.text = Physics2D.gravity.y.ToString();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ToggleGravity(true);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            ToggleGravity(false);
        }
    }
}
