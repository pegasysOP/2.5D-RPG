using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Database containing all of the effects in battles such as status conditions
public class EffectsDatabase
{
    public static Dictionary<StatusID, StatusCondition> StatusConditions { get; }
    = new Dictionary<StatusID, StatusCondition>()
    {
        {
            StatusID.STN,
            new StatusCondition()
            {
                ID = StatusID.STN,
                Name = "Stunned",
                Description = "When stunned there is a 50% chance that attacks will fail",
                InitialMessage = "has been stunned"
            }
        }
    };
}

public enum StatusID
{
    None,
    STN
}

public class StatusCondition
{
    public StatusID ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string InitialMessage { get; set; }
}