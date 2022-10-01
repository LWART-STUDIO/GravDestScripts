using System.Collections.Generic;
using Engine;
using Engine.DI;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Upgrades
{
    public interface ILevelBufsControll:IDependency,ILevelStarted
    {
        List<LevelBuff> LevelsBuffs { get; }
        void BuyBuff(BuffId buffId);
        void InitBuff(BuffId buffId);
        string GetBuffName(BuffId buffId);
        int GetBuffLevel(BuffId buffId);

        Sprite GetBuffIcon(BuffId buffId);
        List<LevelBuff> GetThreeRandomBuffs();
        Damage GetDamageFromBuff(BuffId buffId);
        float DebuffTime(BuffId buffId);
    }
}