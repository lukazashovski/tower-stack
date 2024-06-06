using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 2.0f;
    private float targetY;

    void Start()
    {
        targetY = transform.position.y;
    }

    void Update()
    {
        Vector3 position = transform.position;
        position.y = Mathf.Lerp(position.y, targetY, Time.deltaTime * speed);
        transform.position = position;
    }

    public void MoveUp()
    {
        targetY += 1;
    }
}
