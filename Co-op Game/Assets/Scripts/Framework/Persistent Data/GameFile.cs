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
    
    public void Add(string id, ExternalVariables playerCharacter, int level)
    {
        if (dict == null)
            BuildDictionary();
        
        dict[id] = playerCharacter;
        
        var index = PlayerInfo.FindIndex(e => e.key == id);

        UnlockedLevel += level;
        
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
