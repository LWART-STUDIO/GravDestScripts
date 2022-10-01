using System;
using Custom.Logic.Upgrades;
using Engine.DI;
using TMPro;
using UnityEngine;

namespace Custom.Logic.Player
{
    public class BuffDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _posionBlock;
        [SerializeField] private TMP_Text _posionLevel;
        [SerializeField] private GameObject _fireBlock;
        [SerializeField] private TMP_Text _fireLevel;
        [SerializeField] private GameObject _freezeBlock;
        [SerializeField] private TMP_Text _freezeLevel;
        private ILevelBufsControll _levelBufsControll;
        private bool _poison;
        private bool _fire;
        private bool _freeze;

        private void Start()
        {
            _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
        }

        private void Update()
        {
            _fire = _levelBufsControll.GetBuffLevel(BuffId.Fire) > 1;
            _freeze = _levelBufsControll.GetBuffLevel(BuffId.Freeze) > 1;
            _poison = _levelBufsControll.GetBuffLevel(BuffId.Poison) > 1;

            _fireBlock.SetActive(_fire);
            _posionBlock.SetActive(_poison);
            _freezeBlock.SetActive(_freeze);

            _fireLevel.text =( _levelBufsControll.GetBuffLevel(BuffId.Fire) - 1)+"";
            _freezeLevel.text =( _levelBufsControll.GetBuffLevel(BuffId.Freeze) - 1)+"";
            _posionLevel.text =( _levelBufsControll.GetBuffLevel(BuffId.Poison) - 1)+"";
        }
    }
}
