using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BetterPlayerController : Tribute
{
    [SerializeField] private bool runningEnabled;

    private bool interacting;
    [SerializeField] private int lookDistance;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform menu;
    [SerializeField] private Transform hud;
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

    public static int ATTACK_METER_SPEED = 180;

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
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            if (attackTime <= 0)
            {
                StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = Color.clear;
            }
        }
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
                TextMeshProUGUI interact = StaticData.findDeepChild(menu, "Interact").GetComponent<TextMeshProUGUI>();
                interact.gameObject.SetActive(true);
                interact.text = hit.collider.GetComponent<Interactable>().interactNote(this);
                interact.color = Color.white;
                viewedInteractable = hit.collider.gameObject;
            }
            else
            {
                StaticData.findDeepChild(menu, "Interact").gameObject.SetActive(false);
                viewedInteractable = null;
            }

            if (Input.GetKeyDown(KeyCode.P) && hud != null)
            {
                if (combatMode)
                {
                    StaticData.findDeepChild(hud, "NormalHUD").gameObject.SetActive(true);
                    StaticData.findDeepChild(hud, "CombatHUD").gameObject.SetActive(false);
                }
                else
                {
                    StaticData.findDeepChild(hud, "NormalHUD").gameObject.SetActive(false);
                    StaticData.findDeepChild(hud, "CombatHUD").gameObject.SetActive(true);
                }
                combatMode = !combatMode;
            }

            if (combatMode)
            {
                StaticData.findDeepChild(hud, "Pointer").Rotate(0, 0, Time.deltaTime * ATTACK_METER_SPEED);
                RaycastHit enemHit;
                if (Physics.Raycast(headCam.transform.position, headCam.transform.forward,
                    out enemHit, attackRange(), interactableLayer)
                    && enemHit.collider.GetComponent<ArenaEntity>() != null)
                {
                    ArenaEntity entity = enemHit.collider.GetComponent<ArenaEntity>();
                    initializeHitGame(entity, enemHit.distance);
                }
                else
                {
                    initializeHitGame(null, 0);
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !combatMode)
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
                drop();
            }

            if (viewedInteractable != null && Input.GetKeyDown(KeyCode.Mouse0) && attackTime <= 0)
            {
                interacting = true;
                StaticData.findDeepChild(menu, "Interact").gameObject.SetActive(false);
                animator.SetBool("Forward", false);
                animator.SetBool("Backward", false);
                animator.SetBool("Right", false);
                animator.SetBool("Left", false);
                if (combatMode && viewedInteractable.GetComponent<ArenaEntity>() != null)
                {
                    RaycastHit enemHit;
                    if (Physics.Raycast(headCam.transform.position, headCam.transform.forward,
                        out enemHit, attackRange(), interactableLayer)
                        && enemHit.collider.GetComponent<ArenaEntity>() != null)
                    {
                        calculateHit(viewedInteractable.GetComponent<ArenaEntity>(), enemHit.distance);
                    }
                    else
                    {
                        Debug.LogError("Something went wrong with the attack");
                    }
                }
                else
                {
                    viewedInteractable.GetComponent<Interactable>().menu(this);
                }
            }
        }
    }

    private void initializeHitGame(ArenaEntity target, float distance)
    {
        Transform attMeter = StaticData.findDeepChild(hud, "AttackMeter");
        if (target == null)
        {
            attMeter.gameObject.SetActive(false);
            return;
        }
        else
        {
            attMeter.gameObject.SetActive(true);
        }

        float normHit = Mathf.Clamp((getAccuracy(distance) - target.getAvoidance(distance)) / 100.0f, 0, 1);
        float critHit = normHit / 2;
        float lethHit = normHit / 10;

        StaticData.findDeepChild(attMeter, "Hit2").GetComponent<Image>().fillAmount = 1;
        StaticData.findDeepChild(attMeter, "Crit2").GetComponent<Image>().fillAmount
            = 1 - ((normHit - critHit) / 2);
        StaticData.findDeepChild(attMeter, "Lethal").GetComponent<Image>().fillAmount
            = 1 - (((normHit - critHit) / 2) + ((critHit - lethHit) / 2));
        StaticData.findDeepChild(attMeter, "Crit1").GetComponent<Image>().fillAmount
            = 1 - (((normHit - critHit) / 2) + ((critHit - lethHit) / 2) + lethHit);
        StaticData.findDeepChild(attMeter, "Hit1").GetComponent<Image>().fillAmount
            = 1 - (((normHit - critHit) / 2) + critHit);
        StaticData.findDeepChild(attMeter, "Miss").GetComponent<Image>().fillAmount
            = 1 - normHit;
    }

    private void calculateHit(ArenaEntity target, float distance)
    {
        float position = 1 - ((StaticData.findDeepChild(hud, "Pointer").rotation.eulerAngles.z % 360) / 360);

        float normHit = Mathf.Clamp((getAccuracy(distance) - target.getAvoidance(distance)) / 100.0f, 0, 1);
        float critHit = normHit / 2;
        float lethHit = normHit / 10;

//        float onHit2 = 1;
        float onCrit2 = 1 - ((normHit - critHit) / 2);
        float onLethal = 1 - (((normHit - critHit) / 2) + ((critHit - lethHit) / 2));
        float onCrit1 = 1 - (((normHit - critHit) / 2) + ((critHit - lethHit) / 2) + lethHit);
        float onHit1 = 1 - (((normHit - critHit) / 2) + critHit);
        float onMiss = 1 - normHit;
        if (position < onMiss)
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = Color.black;
            startAttack(0);
        }
        else if (position < onHit1)
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = new Color(1, 192f/255, 0);
            startAttack(1);
        }
        else if (position < onCrit1)
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = Color.red;
            startAttack(2);
        }
        else if (position < onLethal)
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = Color.cyan;
            startAttack(3);
        }
        else if (position < onCrit2)
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = Color.red;
            startAttack(2);
        }
        else
        {
            StaticData.findDeepChild(hud, "HitNote").GetComponent<Image>().color = new Color(1, 192f / 255, 0);
            startAttack(1);
        }
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
