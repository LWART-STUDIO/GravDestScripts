using Custom.Logic.Upgrades;
using Engine.DI;
using UnityEngine;

namespace Custom.Logic.Player
{
    public class BuffContainer : MonoBehaviour
    {
        private ILevelBufsControll _levelBufsControll;

        private void Start()
        {
            _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
        }
    }
    

}
