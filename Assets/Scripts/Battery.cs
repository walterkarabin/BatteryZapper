using UnityEngine;
using UnityEngine.SceneManagement;

public class Battery : MonoBehaviour
{
    //Level of charge the battery has currently
    private float minChargeLevel = -1f;
    private float chargeLevel;
    private float maxChargeLevel = 1f;
    private bool isFull = false;

    //Reference to parts of the battery
    public GameObject chargeMeter;
    public GameObject screen;
    //Reference to the player transform
    public Transform player;

    //Texture Reference
    Renderer renderChargeLevel;
    Renderer renderScreen;

    //Events for Battery
    public delegate void FullyCharged();
    public static event FullyCharged OnFullyCharged;
    public static event FullyCharged OnNotFullyCharged;
    // Start is called before the first frame update
    private void Awake()
    {
        chargeLevel = minChargeLevel;
        renderChargeLevel = chargeMeter.GetComponent<Renderer>();
        renderScreen = screen.GetComponent<Renderer>();
        player = GameObject.Find("Player").transform;

        renderScreen.material.SetColor("_Color", Color.red);
    }

    private void Update()
    {
        transform.LookAt(player);
    }

    public void Charging(float chargeSpeed)
    {
        if (chargeLevel<maxChargeLevel)
        {
            chargeLevel += chargeSpeed;
            FillMeter(chargeLevel);
        }
        else
        {
            MeterFull();
        }
    }
    public void LoseCharge()
    {
        if (chargeLevel > minChargeLevel)
        {
            chargeLevel -= 0.1f;
            FillMeter(chargeLevel);
        }
        if (chargeLevel < maxChargeLevel)
        {
            MeterNotFull();
        }
    }
    private void MeterNotFull()
    {
        if (isFull == true)
        {
            OnNotFullyCharged?.Invoke();
        }
        isFull = false;
        renderChargeLevel.material.SetFloat("_NotDone", 1f);
        renderScreen.material.SetColor("_Color", Color.red);
        // Event for the battery to adjust the list of charged batteries
    }
    private void MeterFull()
    {
        if(isFull == false)
        {
            OnFullyCharged?.Invoke();
        }
        isFull = true;
        renderChargeLevel.material.SetFloat("_NotDone", 0f);
        renderScreen.material.SetColor("_Color", Color.green);
        Destroy(this.gameObject);
        // Trigger event for list of batteries charged
    }

    private void FillMeter(float chargeLevel)
    {
        renderChargeLevel.material.SetFloat("_FillRate", chargeLevel);
    }

    public bool IsCharged()
    {
        return isFull;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
}
