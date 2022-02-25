using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;

    private void Start()
    {
        cam = GameObject.Find("MainCamera").transform;
    }
    void FixedUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
