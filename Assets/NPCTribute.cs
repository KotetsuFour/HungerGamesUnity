using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCTribute : Tribute
{
    public bool testing;
    private NavMeshAgent agent;
    private HumanNeed needBeingMet;
    [SerializeField] private float distanceFromLeader;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        entityCollider = GetComponent<Collider>();
        model = StaticData.findDeepChild(transform, "model").gameObject;
        animator = GetComponent<Animator>();

        agent.speed = tributeData.speed;

        rightHand = StaticData.findDeepChild(transform, "RightHand");
        leftHand = StaticData.findDeepChild(transform, "LeftHand");
        back1 = StaticData.findDeepChild(transform, "Back1");
        back2 = StaticData.findDeepChild(transform, "Back2");

        currentStamina = MAX_STAMINA;
        currentFoodFullness = MAX_FOOD_FULLNESS;
        currentWaterFullness = MAX_WATER_FULLNESS;
        currentSleep = MAX_SLEEP;
    }

    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            return;
        }
        currentFoodFullness -= Arena.timePassed;
        currentWaterFullness -= Arena.timePassed;
        currentSleep -= Arena.timePassed;
        float foodNeed = 1 - (currentFoodFullness / MAX_FOOD_FULLNESS);
        float waterNeed = 1 - (currentWaterFullness / MAX_WATER_FULLNESS);
        float sleepNeed = 1 - (currentSleep / MAX_SLEEP);

        bool sleeping = tributeData.alliance.wakingTime > Arena.arenaTimer;
        animator.SetBool("Sleep", sleeping);
        agent.enabled = !sleeping && !Arena.timerPaused;
        if (sleeping && !Arena.timerPaused)
        {
            sleepTick(Arena.timePassed);
        }
        else if (tributeData.alliance.members[0] == tributeData)
        {

        }
        else
        {
            agent.stoppingDistance = distanceFromLeader;
            agent.destination = tributeData.alliance.members[0].actualObject.transform.position;
            needBeingMet = HumanNeed.FOLLOW;
        }
        /*
        else if (tributeData.alliance.members[0] == tributeData)
        {
            /*
            Vector3 fightFlightDest = fov.fightFlightDestination();
            if (fightFlightDest == Vector3.positiveInfinity)
            {
                HumanNeed currentNeed = HumanNeed.WANDER;
                float needHeur = 0;
                if (foodNeed >= 0.5f || tributeData.alliance.needFood)
                {
                    currentNeed = HumanNeed.FOOD;
                    needHeur = foodNeed;
                }
                if ((sleepNeed >= 0.75f && sleepNeed > needHeur) || tributeData.alliance.needSleep)
                {
                    currentNeed = HumanNeed.SLEEP;
                    needHeur = sleepNeed;
                }
                if ((waterNeed >= 0.75f && waterNeed >= needHeur) || tributeData.alliance.needWater)
                {
                    currentNeed = HumanNeed.WATER;
                }
                if (needBeingMet != currentNeed)
                {
                    needBeingMet = currentNeed;
                    if (needBeingMet == HumanNeed.WATER)
                    {
                        tributeData.alliance.destination = fov.getClosestFreshwaterSource();
                        Vector3 center = tributeData.alliance.destination;
                        agent.destination = new Vector3(center.x + Random.Range(-3, 4), center.y, center.z + Random.Range(-3, 4));
                    }
                    else if (needBeingMet == HumanNeed.FOOD)
                    {
                        tributeData.alliance.destination = fov.getClosestFoodSource();
                        Vector3 center = tributeData.alliance.destination;
                        agent.destination = new Vector3(center.x + Random.Range(-3, 4), center.y, center.z + Random.Range(-3, 4));
                    }
                    else if (needBeingMet == HumanNeed.SLEEP)
                    {
                        tributeData.alliance.wakingTime = Arena.arenaTimer + (DayNightCycle.timeInADay / 4);
                    }
                    else if (needBeingMet == HumanNeed.WANDER)
                    {
                        //TODO maybe lay a trap or run around or something
                    }
                }
            }
            else
            {
                tributeData.alliance.destination = fightFlightDest;
                agent.destination = tributeData.alliance.destination;
            }
            */
/*        }
        else
        {
            needBeingMet = HumanNeed.FOLLOW;
            if ((agent.destination - tributeData.alliance.destination).magnitude > 5)
            {
                Vector3 center = tributeData.alliance.destination;
                agent.destination = new Vector3(center.x + Random.Range(-3, 4), center.y, center.z + Random.Range(-3, 4));
            }
            if (waterNeed >= 0.5f)
            {
                if (tributeData.alliance.waterSupply > 0) {
                    tributeData.alliance.waterSupply--;
                    currentWaterFullness = MAX_WATER_FULLNESS;
                } else
                {
                    tributeData.alliance.needWater = true;
                }
            }
            if (foodNeed >= 0.75f)
            {
                if (tributeData.alliance.foodSupply > 0)
                {
                    tributeData.alliance.foodSupply--;
                    currentFoodFullness = MAX_FOOD_FULLNESS;
                } else
                {
                    tributeData.alliance.needFood = true;
                }
            }
            if (sleepNeed >= 0.75f)
            {
                tributeData.alliance.needSleep = true;
            }
        }

        //TODO set navmesh target
        bool keepMoving = (transform.position - agent.destination).magnitude <= 0.1;
        if (keepMoving)
        {
            agent.destination = transform.position;
            animator.SetBool("Forward", true);
        } else
        {
            animator.SetBool("Forward", false);
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime, MAX_STAMINA);
        }

        if (currentStamina > 0 && (fov.enemiesInView.Count > 0 || fov.muttsInView.Count > 0
            /*TODO OR there is an arena trap active*//*))
        {
/*            animator.SetBool("Run", true);
            agent.speed = tributeData.speed * 2;
            if (keepMoving)
            {
                currentStamina = Mathf.Min(currentStamina - Time.deltaTime, MAX_STAMINA);
            }
        } else
        {
            animator.SetBool("Run", false);
            agent.speed = tributeData.speed;
        }

        animator.SetBool("Airborne", !isGrounded());
*/
    }
    public void sleepTick(float time)
    {
        currentSleep = Mathf.Min(currentSleep + (time * 8), MAX_SLEEP);
    }

    private enum HumanNeed
    {
        WANDER, GET_SUPPLIES, FOLLOW, FOOD, WATER, SLEEP, KILL, FLEE_TO_CORN, FLEE_FROM_FOE
    }
}
