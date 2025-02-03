using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BetterPlayerController : Tribute
{
    [SerializeField] private bool runningEnabled;

    private bool interacting;
    [SerializeField] private int lookDistance;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform menu;
    private GameObject viewedInteractable;

    private CameraRig[] cameras;
    private int currentCamIdx;
    private Transform visionPoint;

    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    private float xRotation;
    private float yRotation;

    private bool mouseMode;

    private bool quickConstructedAlready;

    // Start is called before the first frame update
    void Start()
    {
        if (!quickConstructedAlready)
        {
            quickConstruct();
        }
    }

    public void quickConstruct()
    {
        quickConstructedAlready = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        entityCollider = GetComponent<Collider>();
        model = StaticData.findDeepChild(transform, "model").gameObject;
        animator = GetComponent<Animator>();

        tributeData = StaticData.playerData;
        tributeData.actualObject = this;

        CameraRig headCam = StaticData.findDeepChild(transform, "HeadCam").GetComponent<CameraRig>();
        CameraRig backCam = StaticData.findDeepChild(transform, "BackCam").GetComponent<CameraRig>();
        CameraRig frontCam = StaticData.findDeepChild(transform, "FrontCam").GetComponent<CameraRig>();
        cameras = new CameraRig[] { headCam, backCam, frontCam };
        cameras[currentCamIdx].gameObject.SetActive(true);
        visionPoint = StaticData.findDeepChild(transform, "VisionPoint");
        foreach (CameraRig rig in cameras)
        {
            rig.quickConstruct();
        }

        rightHand = StaticData.findDeepChild(transform, "RightHand");
        leftHand = StaticData.findDeepChild(transform, "LeftHand");
        back1 = StaticData.findDeepChild(transform, "Back1");
        back2 = StaticData.findDeepChild(transform, "Back2");

        updateCameras();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentStamina = MAX_STAMINA;
        currentFoodFullness = MAX_FOOD_FULLNESS;
        currentWaterFullness = MAX_WATER_FULLNESS;
        currentSleep = MAX_SLEEP;
    }

    // Update is called once per frame
    void Update()
    {
        if (interacting)
        {
            if (!viewedInteractable.GetComponent<Interactable>().stillInteracting)
            {
                interacting = false;
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                viewedInteractable.GetComponent<Interactable>().Z();
            }
        }
        else
        {
            float moveVertical = 0;
            float moveHorizontal = 0;
            if (Input.GetKey(KeyCode.W))
            {
                moveVertical = tributeData.speed;
                animator.SetBool("Backward", false);
                animator.SetBool("Forward", true);
                model.transform.rotation = transform.rotation;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveVertical = -tributeData.speed;
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", true);
                model.transform.rotation = transform.rotation;
            }
            else
            {
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", false);
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveHorizontal = -tributeData.speed;
                animator.SetBool("Right", false);
                animator.SetBool("Left", true);
                model.transform.rotation = transform.rotation;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveHorizontal = tributeData.speed;
                animator.SetBool("Left", false);
                animator.SetBool("Right", true);
                model.transform.rotation = transform.rotation;
            }
            else
            {
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
            }
            if (moveVertical == 0 && moveHorizontal == 0)
            {
                currentStamina = Mathf.Min(currentStamina + Time.deltaTime, MAX_STAMINA);
            }

            if (runningEnabled && currentStamina > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                moveVertical *= 2;
                moveHorizontal *= 2;
                animator.SetBool("Run", true);
                if (Mathf.Abs(moveVertical) > 0 || Mathf.Abs(moveHorizontal) > 0)
                {
                    currentStamina = Mathf.Min(currentStamina - Time.deltaTime, MAX_STAMINA);
                }
            }
            else
            {
                animator.SetBool("Run", false);
            }

            float upVector = rb.velocity.y;
            rb.velocity = (transform.forward * moveVertical) + (transform.right * moveHorizontal) + (transform.up * upVector);


            animator.SetBool("Airborne", !isGrounded());


            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switchCamera();
            }

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            visionPoint.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            updateCameras();

            RaycastHit hit;
            BoxCollider headCam = cameras[0].GetComponent<BoxCollider>();
            bool boxHit = Physics.BoxCast(headCam.bounds.center, headCam.bounds.extents, headCam.transform.forward,
                out hit, Quaternion.Euler(new Vector3(0, 0, 0)),
                headCam.bounds.extents.y + lookDistance, interactableLayer);
            if (boxHit)
            {
                /*
                if (hit.collider.GetComponent<Item>() != null)
                {
                    interactNote.GetComponent<TextMeshProUGUI>().text = hit.collider.name.Replace("(Clone)", "") + " (Click)";
                }
                else if (hit.collider.GetComponent<Interactable>().interactType == Interactable.InteractType.COACH)
                {
                    interactNote.GetComponent<TextMeshProUGUI>().text = "Train " + hit.collider.GetComponent<Coach>().taughtSkill;
                }
                else if (hit.collider.GetComponent<Interactable>().interactType == Interactable.InteractType.TRIBUTE)
                {
                    interactNote.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<TrainingTribute>().data.name;
                }
                else
                {
                    interactNote.GetComponent<TextMeshProUGUI>().text = hit.collider.GetComponent<Interactable>().interactType + " (Click)";
                }
                */
                TextMeshProUGUI interact = StaticData.findDeepChild(menu, "Interact").GetComponent<TextMeshProUGUI>();
                interact.gameObject.SetActive(true);
                interact.text = hit.collider.GetComponent<Interactable>().interactNote(this);
                viewedInteractable = hit.collider.gameObject;
            }
            else
            {
                StaticData.findDeepChild(menu, "Interact").gameObject.SetActive(false);
                viewedInteractable = null;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                interacting = true;
                StaticData.findDeepChild(menu, "Interact").gameObject.SetActive(false);
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", false);
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
                viewedInteractable = GetComponent<PlayerInteractable>().gameObject;
                GetComponent<PlayerInteractable>().menu(this);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //Drop item in hand
            }

            if (viewedInteractable != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                interacting = true;
                StaticData.findDeepChild(menu, "Interact").gameObject.SetActive(false);
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", false);
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
                viewedInteractable.GetComponent<Interactable>().menu(this);
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            mouseMode = !mouseMode;
            if (mouseMode)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        */
    }

    private void updateCameras()
    {
        foreach (CameraRig rig in cameras)
        {
            rig.updatePosition(visionPoint);
        }
    }

    public void hijack(Interactable interactable)
    {
        interacting = true;
        viewedInteractable = interactable.gameObject;
        animator.SetBool("Forward", false);
        animator.SetBool("Backward", false);
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
    }
    public void liberate()
    {
        interacting = false;
        viewedInteractable = null;
    }
    private void switchCamera()
    {
        cameras[currentCamIdx].gameObject.SetActive(false);
        currentCamIdx = (currentCamIdx + 1) % cameras.Length;
        cameras[currentCamIdx].gameObject.SetActive(true);
    }

    public void setupForArena()
    {
        runningEnabled = true;
    }
}
