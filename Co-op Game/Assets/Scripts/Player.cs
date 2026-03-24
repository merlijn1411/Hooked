using UnityEngine;

public class Player : MonoBehaviour
{
    //DIT IS VOOR LATER
    //public float speed = 5f;

    public float stepSize = 1f;
    public void MoveLeft()
    {
        transform.Translate(Vector3.left * stepSize);
        //transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    public void MoveRight()
    {
        transform.Translate(Vector3.right * stepSize);
        //transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void Jump()
    {
        Debug.Log("Jump!");
    }
}
