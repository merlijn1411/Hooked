using System;
using System.Collections.Generic;

[Serializable]
public class GameFile
{
    public int UnlockedLevel;
    public List<ExternalFiles> PlayerInfo = new List<ExternalFiles>();

    private Dictionary<string, ExternalVariables> dict;

    public void BuildDictionary()
    {
        dict = new Dictionary<string, ExternalVariables>();
        foreach (var e in PlayerInfo)
        {
            dict[e.key] = e.Value;
        }
    }
    
    public void AddPlayer(string id, ExternalVariables playerCharacter)
    {
        if (dict == null) BuildDictionary();
        
        dict![id] = playerCharacter;
        
        var index = PlayerInfo.FindIndex(e => e.key == id);
        
        if (index >= 0)
        {
            PlayerInfo[index] = new ExternalFiles
            {
                key = id,
                Value = playerCharacter
            };
        }
        else
        {
            PlayerInfo.Add(new ExternalFiles
            {
                key = id,
                Value = playerCharacter
            });
        }
        
    }

    public void DeletePlayer(string key)
    {
        if (dict == null)
            BuildDictionary();

        dict.Remove(key);
        
        PlayerInfo.RemoveAll(player => player.key == key);
    }

    public void DeleteAllPlayers()
    {
        if (dict == null)
            BuildDictionary();

        PlayerInfo.Clear();
        dict = new Dictionary<string, ExternalVariables>();
    }

    public void AddUnlockedLevels(int unlockedLevels)
    {
        UnlockedLevel += unlockedLevels;
    }
}

[Serializable]
public class ExternalFiles
{
    public string key;
    public ExternalVariables Value;
    
}

[Serializable]
public class ExternalVariables
{
    public int Index;
    public string ID;
}
