using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Globalization;

/* 
Our tool will need to be able to grab a collection of statblocks and cycle through them. 
It will do this based on the folder that they are stored in. When it cycles through. it will display that current statblock for editing.
when a new stat is added to a stat, it will update all other statblocks in its list to include that stat.
same when a stat is deleted.

*/
[CreateAssetMenu(fileName = "StatBlock", menuName = "Mythic/Stats/StatBlock")]
public class StatBlock : AbstractScriptable
{   
    private List<StatBase> statsList;
    private int statCount = 0;
    public string folder;
    [SerializeReference] public List<StatData> statData = new List<StatData>();
    private Dictionary<string, StatBase> stats = new Dictionary<string, StatBase>();

    private void OnValidate()
    {
        if(statsList.Count == 0) statsList = new List<StatBase>(Resources.LoadAll<StatBase>(folder));
        if(statsList.Count < statData.Count && statsList.Count != 0)
        {
            ProcessStatsList();
            return;
        }    
        if(statData.Count > statCount)
        {
            statData.RemoveAt(statCount);
            return;
        }
        if (!Application.isPlaying)
            {
                statCount = statsList.Count;
                ProcessStatsList();
            }

    }
    private void ProcessStatsList()
    {
            for(var i = statData.Count - 1; i < stats.Count; i++)
            {
                if(i < 0) i = 1;
                this.AddStatToDictionary(statsList[i]);
            }
    }

    private void AddStatToDictionary(StatBase stat)
    {
        if(!stats.TryGetValue(stat.name, out StatBase type))stats.Add(stat.name, stat);
        switch(stat)
        {
            case StatInt:
                statData.Add(new StatDataInt());
                break;
            case StatFloat: 
                statData.Add(new StatDataFloat());
                break;
            case StatString: 
                statData.Add(new StatDataString());
                break;
        }
    }

    public virtual void CreateDictionary()
    {
        stats = new Dictionary<string, StatBase>(StringComparer.OrdinalIgnoreCase);
        var list = new List<StatBase>(Resources.LoadAll<StatBase>(folder)); 
        foreach (var stat in list)
        {
            if (stat == null || string.IsNullOrWhiteSpace(stat.name)) continue;
            stats[stat.name] = stat; // last one wins
        }
    }

    // Caller decides output type at invocation:
    public virtual T GetValue<T>(string statName)
    {
        if (stats == null) CreateDictionary();

        if (!stats.TryGetValue(statName, out var stat))
            throw new KeyNotFoundException($"Stat '{statName}' not found.");

        if (stat is Stat<T> typed)
            return typed.value;

        // Helpful error when you ask for float but stat is int, etc.
        throw new InvalidCastException(
            $"Stat '{statName}' is '{stat.TypeValue.Name}', not '{typeof(T).Name}'."
        );
    }

    public virtual bool TryGetValue<T>(string statName, out T value)
    {
        value = default;

        if (stats == null) CreateDictionary();
        if (!stats.TryGetValue(statName, out var stat)) return false;

        if (stat is Stat<T> typed)
        {
            value = typed.value;
            return true;
        }

        return false;
    }
}

[System.Serializable]public class StatData
{
    
}

[System.Serializable]public class StatDataInt : StatData
{
    public int value;
    public int min;
    public int max;
}

[System.Serializable]public class StatDataFloat : StatData
{
    public int value;
    public int min;
    public int max;
}

[System.Serializable]public class StatDataString : StatData
{
    public string value;
    public int min;
    public int max;
}








