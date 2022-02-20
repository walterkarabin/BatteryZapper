using UnityEngine;

public class Player : MonoBehaviour
{
    // Player mask allows player to be ignored when checking for interactions
    [SerializeField] private LayerMask playerMask;
    // How fast the player can move around
    [SerializeField][Range(0.3f, 0.6f)] private float moveSpeed;
    [SerializeField][Range(0.0f, 5.0f)] private float maxVelocity;

    //Used for input from Input Manager
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rigidbodyComponent;


    //Camera functionality
    [SerializeField] private GameObject cam;

    //Weapon objects
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject laserGun;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 0.4f;
        maxVelocity = 2f;
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            cam.GetComponent<CameraFollow>().SetCameraMode();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gun.SetActive(true);
            laserGun.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gun.SetActive(false);
            laserGun.SetActive(true);
        }



        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 0.5f, 0);
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    //Fixed update is called once every physics update
    private void FixedUpdate()
    {
        rigidbodyComponent.velocity = new Vector3(rigidbodyComponent.velocity.x + (horizontalInput * moveSpeed),
            rigidbodyComponent.velocity.y,
            rigidbodyComponent.velocity.z + (verticalInput * moveSpeed));
        rigidbodyComponent.velocity = Vector3.ClampMagnitude(rigidbodyComponent.velocity, maxVelocity);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
        }
    }
}
