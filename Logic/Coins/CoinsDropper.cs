using Custom.Extensions;
using Unity.Mathematics;
using UnityEngine;


namespace Custom.Logic.Coins
{
    public class CoinsDropper : MonoBehaviour
    {
        [SerializeField] private GameObject _coin =>Resources.Load<GameObject>("Prefabs/Coins/Coin");
        [SerializeField] private float _radius;

        public void Drop()
        {
            Vector3 pos = Tools.RandomPointInXZCircle(transform.position, _radius);
            CoinMover coinMover = Instantiate(_coin, transform.position, quaternion.identity).GetComponent<CoinMover>();
            coinMover.Fly(pos);
        }

        [NaughtyAttributes.Button()]
        public void DropAmpount(int amount=10)
        {
            for (int i = 0; i < amount; i++)
            {
                Drop();
            }
        }
        
    }
}
