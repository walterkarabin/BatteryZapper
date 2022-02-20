using System;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    //target which the gun should rotate round
    public GameObject target;
    //camera which has no relative motion or rotation to target
    public Camera cam;
    //muzzle where the projectile will begin
    public GameObject muzzle;
    // Laser Gun animations
    public GameObject Barrel;
    public GameObject Hilt;
    public GameObject Tip;
    public GameObject Middle;
    public GameObject Base;
    //Laser info
    public float damage = 10f;
    //LayerMask for raycasting
    [SerializeField] private LayerMask playerMask;

    //Default distance
    [SerializeField] private float defDistanceRay = 200;
    //Line Renderer reference
    public LineRenderer lineRenderer;
    //Texture Reference
    Renderer renderBarrel, renderHilt;



    // Start is called before the first frame update
    void Start()
    {
        renderBarrel = Barrel.GetComponent<Renderer>();
        renderHilt = Hilt.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            ShootLaser();
            DynamicTexture(true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            EraseLaser();
            DynamicTexture(false);
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
            Debug.DrawRay(ray.origin, ray.direction*distance);
        }

        //Find the direction between the player and the mouse position
        Vector3 mousePos = hit - target.transform.position;
        mousePos.Normalize();

        // Calculate the angle used for rotating the gun object
        float angle = Mathf.Atan2(mousePos.z, mousePos.x) * Mathf.Rad2Deg;
        // Use a quaternion to get rotation by angle, around an axis
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
    }
    void ShootLaser()
    {
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.right, out hit, Mathf.Infinity, playerMask))
        {
            Draw2DRay(muzzle.transform.position, hit.point * 0.99f);
        }
        else
        {
            Draw2DRay(muzzle.transform.position, muzzle.transform.right * defDistanceRay);
        }
        DynamicBarrelSize();
    }

    private void DynamicTexture(bool glow)
    {
        if (glow)
        {
            renderBarrel.material.SetInt("_ShouldGlow", 1);
            renderHilt.material.SetInt("_ShouldGlow", 1);
        }
        else
        {
            renderBarrel.material.SetInt("_ShouldGlow", 0);
            renderHilt.material.SetInt("_ShouldGlow", 0);
        }
    }

    private void DynamicBarrelSize()
    {
        float omega1 = 8f;
        float omega2 = 12f;
        float omega3 = 16f;
        float angle1 = Time.time * omega1;
        float angle2 = Time.time * omega2;
        float angle3 = Time.time * omega3;

        Vector3 scaleChange = new Vector3(0, 0, 0);
        scaleChange.y = (Mathf.Sin(angle3)/4f)+ 1.25f;
        scaleChange.x = Tip.transform.localScale.x;
        scaleChange.z = Tip.transform.localScale.z;
        Tip.transform.localScale = scaleChange;

        scaleChange.y = (Mathf.Sin(angle2) / 4f) + 1.25f;
        scaleChange.x = Middle.transform.localScale.x;
        scaleChange.z = Middle.transform.localScale.z;
        Middle.transform.localScale = scaleChange;

        scaleChange.y = (Mathf.Sin(angle1) / 4f) + 1.25f;
        scaleChange.x = Base.transform.localScale.x;
        scaleChange.z = Base.transform.localScale.z;
        Base.transform.localScale = scaleChange;
    }

    void EraseLaser()
    {
        Draw2DRay(new Vector2(0, 0), new Vector2(0, 0));
    }

    private void Draw2DRay(Vector3 startPos, Vector3 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

}
