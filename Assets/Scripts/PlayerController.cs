using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public float jumpSpeed = 256.0f;
    public TextMeshProUGUI countText;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private int jumpCount;
    public GameObject winTextObject;
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.position.y <= 0.6)
            {
                movementY = 0;
                jumpCount = 0;
            }
            if (jumpCount < 2)
            {
                movementY = jumpSpeed;
                jumpCount++;
            }
        }
        movementY -= (movementY * 4 * Time.deltaTime);
        Vector3 movement = new Vector3(movementX * speed, movementY, movementZ * speed);
        rb.AddForce(movement);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        //since y is being used for jumping, i call the movement perpendicular to x "z" and not "y"
        movementZ = movementVector.y;
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 8)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            //destroy the current object
            Destroy(gameObject);
            //Update winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }
}
