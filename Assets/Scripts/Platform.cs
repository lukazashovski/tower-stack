using UnityEngine;

public class Platform : MonoBehaviour
{
    private float speed = 5.0f;
    public bool movingRight = true;
    public bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (transform.position.x > 6.0f)
                {
                    movingRight = false;
                }
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (transform.position.x < -6.0f)
                {
                    movingRight = true;
                }
            }
        }
    }

    public void Place()
    {
        isMoving = false;
    }
}
