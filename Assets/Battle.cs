using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle
{
    public List<ArenaEntity> participants;
    public bool containsPlayer;
    public Battle()
    {
        participants = new List<ArenaEntity>();
    }
    public void addParticipant(ArenaEntity par)
    {
        if (!participants.Contains(par))
        {
            participants.Add(par);
            if (par is Tribute)
            {
                Tribute tr = (Tribute)par;
                Alliance allies = tr.tributeData.alliance;
                if (tr.tributeData == StaticData.playerData)
                {
                    containsPlayer = true;
                }
                foreach (StaticData.TributeData data in allies.members)
                {
                    if (data == StaticData.playerData)
                    {
                        containsPlayer = true;
                    }
                    if (!participants.Contains(data.actualObject))
                    {
                        participants.Add(data.actualObject);
                    }
                }
            }
        }
    }
    public void autoResolve()
    {
        //TODO

    }
}
