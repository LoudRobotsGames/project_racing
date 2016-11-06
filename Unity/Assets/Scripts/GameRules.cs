using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class GameRules :ScriptableObject
{
    public delegate void GameRulesCallback();

    public GameRulesCallback OnFinish;
    public string RulesName = "BasicRules";

    public abstract void Begin();
    public abstract void Update();
}
