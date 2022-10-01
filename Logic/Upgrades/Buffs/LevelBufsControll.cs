using System;
using System.Collections.Generic;
using System.Linq;
using apps;
using Custom.Extensions;
using Engine.DI;
using Main;
using Main.Level;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Upgrades
{
    public class LevelBufsControll : MonoBehaviour,ILevelBufsControll
    {
        [SerializeField] private List<LevelBuff> _levelsBuffs;
        public List<LevelBuff> LevelsBuffs => _levelsBuffs;
        private Player.Player _player;

        private void Awake()
        {
            foreach (LevelBuff buff in _levelsBuffs)
            {
                InitBuff(buff.BuffId);
            }
        }

        private void Start()
        {
            _player = DIContainer.GetAsSingle<ILevelsManager>().level.PlayerInformation.Player;
        }

        public void Inject()
        {
            DIContainer.RegisterAsSingle<ILevelBufsControll>(this);
        }

        private void OnEnable()
        {
            LevelStatueStarted.Subscribe(this);
        }

        private void OnDisable()
        {
            LevelStatueStarted.Unsubscribe(this);
        }

        public void InitBuff(BuffId buffId)
        {
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            buff.Level = 1;

        }

        public int GetBuffLevel(BuffId buffId)
        {
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            return buff.Level;
        }

        public Damage GetDamageFromBuff(BuffId buffId)
        {
            Damage damage = _player.Damage;
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            damage.value = buff.BaseDamage + (buff.Level * buff.MultiPlier) - buff.MultiPlier;
            return damage;
        }

        public Sprite GetBuffIcon(BuffId buffId)
        {
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            return buff._buffSprite;
        }


        public string GetBuffName(BuffId buffId)
        {
            switch (buffId)
            {
                case (BuffId.Health):
                    return "HEALTH";
                case (BuffId.Fire):
                    return "FIRE";
                case (BuffId.MoveSpeed):
                    return "MOVING";
                case (BuffId.Freeze):
                    return "FREEZE";
                case (BuffId.AmountOfObjects):
                    return "MAX COUNT";
                case (BuffId.Poison):
                    return "POISON";
                default:
                    return "";
            }
        }

        public List<LevelBuff> GetThreeRandomBuffs()
        {
            return  Tools.GetRandom(_levelsBuffs.ToArray(), 3).ToList();
        }

        public void BuyBuff(BuffId buffId)
        {
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            if (buff.Level >= buff.MaxLevel)
                return;
            buff.Level++;
            EventsLogger.CustomEvent($"Buff_Bought_{buffId}:Level{buff.Level}");

        }

        public  float DebuffTime(BuffId buffId)
        {
            LevelBuff buff = _levelsBuffs.Find(x => x.BuffId == buffId);
            return buff.DebuffTick;
        }

        public void LevelStarted()
        {
            foreach (LevelBuff buff in _levelsBuffs)
            {
                InitBuff(buff.BuffId);
            }
        }
    }
}
