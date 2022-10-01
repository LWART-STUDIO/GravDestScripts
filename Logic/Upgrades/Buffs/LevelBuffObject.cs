using Custom.Logic.Player;
using Engine.DI;
using UnityEngine;

namespace Custom.Logic.Upgrades.Buffs
{
    public class LevelBuffObject : MonoBehaviour
    {
        private LevelBuffSpawner _levelBuffSpawner;
        private ILevelBufsControll _levelBufsControll;
        private BuffId _buffId;
        public void Init(LevelBuffSpawner spawner,BuffId id)
        {
            _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
            _levelBuffSpawner = spawner;
            _buffId = id;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BuffContainer buffContainer))
            {
                _levelBufsControll.BuyBuff(_buffId);
                _levelBuffSpawner.DestroyBuffsObjects();
            }
        }
    }
}
