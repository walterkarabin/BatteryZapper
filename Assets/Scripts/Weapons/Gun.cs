using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //target which the gun should rotate round
    public GameObject target;
    //camera
    public Camera cam;
    //Max fire range
    public float maxRange;
    //muzzle where the projectile will begin
    public GameObject muzzle;
    //Bullet info
    public float damage = 10f;
    //LayerMask for raycasting
    [SerializeField] private LayerMask layerMask;

    //Line renderer
    private LineRenderer laserLine;

    //Shot speed
    private float lastShot;
    private float shotSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
        cam = Camera.main;
        laserLine = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //If the button pressed is associated with Fire1 in input manager
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            laserLine.enabled = true;
        }
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - lastShot > shotSpeed)
            {
                laserLine.enabled = false;

            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            laserLine.enabled = false;
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
        //checks if the ray hits and object using the layerMask
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.right, out hit, 100f, layerMask))
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * hit.distance, Color.red, 10f);
            Debug.Log("Hit: " + hit.transform.name);
        }
        else
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * 1000f, Color.white, 1f);
            Debug.Log("No Hit");
        }
    }

    //Call function for EnemyAI reference object to take damage
    private void DealDamage(GameObject obj)
    {
        obj.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
    }

    //Shoots a raycast to deal damage to enemies and batteries
    void Shoot()
    {
        if (Time.time - lastShot > shotSpeed)
        {
            laserLine.SetPosition(0, muzzle.transform.position);
            RaycastHit hit;
            //checks if the ray hits and object using the layerMask
            if (Physics.Raycast(muzzle.transform.position, muzzle.transform.right, out hit, 10f, layerMask))
            {
                laserLine.SetPosition(1, hit.point);
                //Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * hit.distance, Color.green, 10f);
                if (hit.transform.tag == "Enemy")
                {
                    DealDamage(hit.transform.gameObject);
                }
                if (hit.transform.tag == "Battery")
                {
                    hit.collider.GetComponent<Battery>().LoseCharge();
                }
            }
            else
            {
                laserLine.SetPosition(1, muzzle.transform.position + (muzzle.transform.right * maxRange));
                //Debug.DrawRay(muzzle.transform.position, muzzle.transform.right * 10f, Color.yellow, 1f);
            }
            lastShot = Time.time;
        }
    }
    private IEnumerator ShotEffect()
    {
        // Turn on our line renderer
        laserLine.enabled = true;

        //Wait for .07 seconds
        yield return 0.07f;

        // Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
}
