using UnityEngine;

public class Gun : MonoBehaviour
{
    //target which the gun should rotate round
    public GameObject target;
    //camera which has no relative motion or rotation to target
    public Camera cam;
    //projectile which will shoot from the gun
    public GameObject projectile;
    //muzzle where the projectile will begin
    public GameObject muzzle;
    //Bullet info
    public float launchVelocity = 100f;
    public float damage = 10f;
    //LayerMask for raycasting
    [SerializeField] private LayerMask playerMask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            makeRaycast();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        AimGunWithMouse();

    }

    void AimGunWithMouse()
    {
        //Create a plane that the mouse can project onto
        Plane plane = new Plane(Vector3.up, target.transform.position);
        //Create a ray that will project onto the plane
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //Distance from the camera to the hit point on the plane
        float distance;
        //Store the hit point
        Vector3 hit = new Vector3(0, 0, 0);
        //If there is a hit with the mouse raycast and the plane
        if (plane.Raycast(ray, out distance))
        {   //Save the hit point
            hit = ray.GetPoint(distance);
            //Draw a ray for debug purposes
            Debug.DrawRay(ray.origin, ray.direction * distance);
        }

        //Find the direction between the player and the mouse position
        Vector3 mousePos = hit - target.transform.position;
        mousePos.Normalize();

        // Calculate the angle used for rotating the gun object
        float angle = Mathf.Atan2(mousePos.z, mousePos.x) * Mathf.Rad2Deg;
        // Use a quaternion to get rotation by angle, around an axis
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
    }
    private void makeRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.right, out hit, Mathf.Infinity, playerMask))
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * hit.distance, Color.red, 10f);
            Debug.Log("Hit: " + hit.transform.name);
        }
        else
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * 1000f, Color.white);
            Debug.Log("No Hit");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(launchVelocity, 0, 0);
        Destroy(bullet, 5f);
    }
}
