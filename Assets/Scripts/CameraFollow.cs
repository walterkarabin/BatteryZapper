using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 1f;

    private Vector3 vel;
    private Rigidbody rigidbodyComponent;

    //Used for changing camera follow modes
    [SerializeField] private Quaternion defaultRotation;
    public bool shouldLookAt = true;
    private void Start()
    {
        target = GameObject.Find("Player");
        rigidbodyComponent = target.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        vel.x = rigidbodyComponent.velocity.x;
        vel.z = 1;
        vel.y = 1;

        Vector3 desiredPos = target.transform.position + offset;// + Vector3.Scale(offset, vel);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;

        if (shouldLookAt)
        {
            transform.LookAt(target.transform);
        }
        else
        {
            transform.rotation = defaultRotation;
        }
    }

    public void SetCameraMode()
    {
        if (shouldLookAt)
        {
            //Debug.Log("Changed Mode to Fixed");
            shouldLookAt = false;
        }
        else
        {
            //Debug.Log("Changed Mode to Follow");
            shouldLookAt = true;
        }
    }
}
