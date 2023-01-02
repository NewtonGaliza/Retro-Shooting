using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float speed;
    private Vector2 moveInput;
    private Vector2 mouseInput;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Camera viewCam;
    [SerializeField] private GameObject bulletImpact;
    public int currentAmmo;
    [SerializeField] private Animator gunAnim;

    private void Awake() 
    {
        instance = this;
    }     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 moveHorizontal = transform.up * -moveInput.x; //to correct the opposite movement
        Vector3 moveVertical = transform.right * moveInput.y;

        //apply move to rigidbody
        rigidBody.velocity = (moveHorizontal + moveVertical) * speed;

        //player view
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - mouseInput.x); 

        // viewCam.transform.localRotation = Quaternion.Euler(viewCam.transform.localRotation.eulerAngles + new Vector3(0f, mouseInput.y, 0f));

        //shooting
        if(Input.GetMouseButtonDown(0))
        {
            if(currentAmmo > 0)
            {
                Ray ray = viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if(Physics.Raycast(ray, out hit))
                {
                    //Debug.Log("looknig at "+ hit.transform.name);
                    Instantiate(bulletImpact, hit.point, transform.rotation);

                    if(hit.transform.tag == "Enemy")
                    {
                        hit.transform.parent.GetComponent<EnemyController>().TakeDamage();
                    }
                }
                else
                {
                    Debug.Log("look at nothing");
                }
                currentAmmo--;
                gunAnim.SetTrigger("Shoot");
            }
        }
    }
}
