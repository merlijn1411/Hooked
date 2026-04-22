using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GameFile
{
    public List<ExternalFiles> ImportantFiles = new List<ExternalFiles>();

    public void Add(int id, SpriteRenderer spriteRenderer, Animator animator)
    {
        var newFile = new ExternalFiles
        {
            ID = id,
            Index = ImportantFiles.Count,
            PlayerSpriteRenderer = spriteRenderer,
            PlayerAnimator = animator
        };

        ImportantFiles.Add(newFile);
    }
}

[Serializable]
public class ExternalFiles
{
    public int ID;
    public int Index;
    public int level;
    public SpriteRenderer PlayerSpriteRenderer;
    public Animator PlayerAnimator;
}
