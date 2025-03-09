using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform Subject;

    Vector2 startPosition;

    float startZ;

    Vector2 travel => (Vector2)cam.transform.position - startPosition;
    float distanceFromSubject => transform.position.z - Subject.position.z;
    float clippingPlane => (cam.transform.position.z + (distanceFromSubject > 0 ? cam.farClipPlane : cam.nearClipPlane) * Time.deltaTime);

    float parallaxFactore => Mathf.Abs(distanceFromSubject) / clippingPlane;

    Vector2 parallaxFactor;
    public void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    public void Update()
    {
        Vector2 newPos = startPosition + travel * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);

    }

}