using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform target;
    public float yOffset = 2f;
    public float downOffset = 10f;
    public float downLerpSpeed = 2f;

    private float currentYOffset;
    private float targetYOffset;

    void Start()
    {
        currentYOffset = yOffset;
        targetYOffset = yOffset;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            targetYOffset = yOffset - downOffset;
        }
        else
        {
            targetYOffset = yOffset;
        }

        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, downLerpSpeed * Time.deltaTime);

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + currentYOffset, -10f);

        transform.position = Vector3.Slerp(transform.position, desiredPosition, FollowSpeed * Time.deltaTime);
    }
}
