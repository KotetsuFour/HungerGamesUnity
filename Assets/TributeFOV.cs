using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TributeFOV : MonoBehaviour
{
    public List<GameObject> enemiesInView;
    public List<GameObject> knownFreshwaterSources;
    public List<GameObject> muttsInView;
    public List<GameObject> knownEdiblePlants;
    public List<Trap> knownTraps;

    private void Start()
    {
        enemiesInView = new List<GameObject>();
        knownFreshwaterSources = new List<GameObject>();
        muttsInView = new List<GameObject>();
        knownEdiblePlants = new List<GameObject>();
        knownTraps = new List<Trap>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Tribute")
        {
            if (transform.parent.GetComponent<NPCTribute>().tributeData.alliance
                != other.GetComponent<Tribute>().tributeData.alliance)
            {
                enemiesInView.Add(other.gameObject);
            }
        }
        else if (other.tag == "Mutt")
        {
            muttsInView.Add(other.gameObject);
        }
        else if (other.GetComponent<ArenaTile>() != null)
        {
            if (other.GetComponent<ArenaTile>().water == 1)
            {
                if (transform.parent.GetComponent<NPCTribute>().tributeData.alliance.needWater)
                {
                    transform.parent.GetComponent<NPCTribute>().tributeData.alliance.needWater = false;
                    transform.parent.GetComponent<NPCTribute>().tributeData.alliance.waterSupply
                        += transform.parent.GetComponent<NPCTribute>().tributeData.alliance.members.Count;
                }
                if (!knownFreshwaterSources.Contains(other.gameObject))
                {
                    knownFreshwaterSources.Add(other.gameObject);
                }
            }
        }
        else if (other.tag == "FoodPlant")
        {
            StaticData.TributeData data = transform.parent.GetComponent<NPCTribute>().tributeData;
            if (data.alliance.needFood)
            {
                //TODO chance of death from poison
                data.alliance.needFood = false;
                data.alliance.foodSupply += data.alliance.members.Count + (data.getSkillLevel(StaticData.Skill.PLANTS) * 2);
            }
            if (!knownEdiblePlants.Contains(other.gameObject))
            {
                knownEdiblePlants.Add(other.gameObject);
            }
        } else if (other.tag == "Trap")
        {
            Trap trap = other.GetComponent<Trap>();
            if (trap.sprung)
            {
                knownTraps.Remove(trap);
                Alliance allies = transform.parent.GetComponent<NPCTribute>().tributeData.alliance;
                if (trap.meatAcquired > 0)
                {
                    allies.needFood = false;
                    allies.foodSupply += trap.meatAcquired + transform.parent.GetComponent<NPCTribute>().tributeData.getSkillLevel(StaticData.Skill.ANIMALS);
                }
                Destroy(other.gameObject);
            } else
            {
                if (!knownTraps.Contains(trap))
                {
                    knownTraps.Add(trap);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Tribute")
        {
            if (transform.parent.GetComponent<NPCTribute>().tributeData.alliance
                != other.GetComponent<Tribute>().tributeData.alliance)
            {
                enemiesInView.Remove(other.gameObject);
            }
        }
        else if (other.tag == "Mutt")
        {
            muttsInView.Remove(other.gameObject);
        }
    }

    public Vector3 getClosestFreshwaterSource()
    {
        if (knownFreshwaterSources.Count == 0)
        {
            return new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000));
        }
        Vector3 pos = knownFreshwaterSources[0].transform.position;
        foreach (GameObject source in knownFreshwaterSources)
        {
            if ((source.transform.position - transform.position).magnitude < (pos - transform.position).magnitude)
            {
                pos = source.transform.position;
            }
        }
        //Don't go into the water, just get near it
        pos += (transform.position - pos).normalized * Arena.cellSize * 0.75f;
        return pos;
    }
    public Vector3 getClosestFoodSource()
    {
        if (knownEdiblePlants.Count == 0 && knownTraps.Count == 0)
        {
            return new Vector3(Random.Range(-1000, 1000), 0, Random.Range(-1000, 1000));
        }
        Vector3 pos = Vector3.positiveInfinity;
        foreach (GameObject source in knownEdiblePlants)
        {
            if ((source.transform.position - transform.position).magnitude < (pos - transform.position).magnitude)
            {
                pos = source.transform.position;
            }
        }
        foreach (Trap trap in knownTraps)
        {
            if (trap.sprung && 
                (trap.transform.position - transform.position).magnitude <= (pos - transform.position).magnitude)
            {
                pos = trap.transform.position;
            }
        }
        return pos;
    }
    public Vector3 fightFlightDestination()
    {
        if (muttsInView.Count > 0)
        {
            return Vector3.zero;
        }
        if (enemiesInView.Count == 0)
        {
            return Vector3.positiveInfinity;
        }
        Vector3 sum = Vector3.zero;
        Vector3 closest = Vector3.positiveInfinity;
        int dangerRating = 0;
        foreach (GameObject g in enemiesInView)
        {
            sum += g.transform.position;
            StaticData.TributeData data = g.GetComponent<Tribute>().tributeData;
            dangerRating++;
            if (data.age >= 15)
            {
                dangerRating++;
            }
            if ((g.transform.position - transform.position).magnitude < (closest - transform.position).magnitude)
            {
                closest = g.transform.position;
            }
        }
        Alliance myAlliance = transform.parent.GetComponent<Tribute>().tributeData.alliance;
        int safetyRating = 0;
        foreach (StaticData.TributeData member in myAlliance.members)
        {
            safetyRating++;
            if (member.age >= 15)
            {
                safetyRating++;
            }
        }
        sum /= enemiesInView.Count;
        if (dangerRating > safetyRating + 4)
        {
            return transform.position + ((transform.position - sum).normalized * Arena.cellSize);
        }
        else
        {
            if ((transform.position - closest).magnitude <= transform.parent.GetComponent<Tribute>().attackRange())
            {
                Battle battle = new Battle();
                battle.addParticipant(transform.parent.GetComponent<Tribute>());
                foreach (GameObject enem in enemiesInView)
                {
                    battle.addParticipant(enem.GetComponent<Tribute>());
                }
                foreach (GameObject mutt in muttsInView)
                {
                    battle.addParticipant(mutt.GetComponent<Mutt>());
                }
                Arena.currentBattle = battle;
                Arena.timerPaused = true;
            }
            return closest;
        }
    }
}
