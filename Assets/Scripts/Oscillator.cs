using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float speed;
    [SerializeField] float dealayTime = 2f;

    Vector3 startPosition;
    Vector3 endPosition;
    float movementFactor;


    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }
    void Update()
    {
        Invoke("movement", dealayTime);
    }

    private void movement()
    {
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
