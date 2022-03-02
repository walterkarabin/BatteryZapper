using UnityEngine.SceneManagement;
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

    //Player Health Stats
    private float currentHealth;
    private float maxHealth = 100f;
    public HealthBar healthBar;


    //Camera functionality
    [SerializeField] private GameObject cam;

    //Weapon objects
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject laserGun;

    //SpawnManager
    public SpawnManager spawnManager;
    private Battery batty;

    // * Better comments extension:
    // The tags (!, ?, TODO, *) all go at the beginning of the comment
    // This is a regular comment
    // ! this is an alert
    // ? this is a query
    // TODO this is a todo comment
    // * this is a highlighted comment

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        moveSpeed = 0.4f;
        maxVelocity = 2f;
        gun.SetActive(true);
        laserGun.SetActive(false);
        rigidbodyComponent = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<HealthBar>();
        healthBar.SetMaxCharge((maxHealth / maxHealth));
        SetHealthBar();
    }


    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////
        if (Input.GetKeyDown(KeyCode.P))
        {
            spawnManager.SpawnEnemies();

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            spawnManager.SpawnBatteries();

        }
        ////////////////////////////////

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

    public void SetHealthBar()
    {
        healthBar.SetCharge((currentHealth / maxHealth));
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0f)
        {
            currentHealth -= damage;
        }
        else
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        SetHealthBar();
    }
}
