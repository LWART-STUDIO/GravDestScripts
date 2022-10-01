using Custom.Levels;
using Engine.DI;
using Main.Level;
using UnityEngine;

namespace Custom.Logic.Portals
{
    public class EndPortal : MonoBehaviour
    {
        private LevelControl _levelControl;

        private void Awake()
        {
            _levelControl = DIContainer.GetAsSingle<ILevelsManager>().level.LevelControl;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                _levelControl.FinishLevel();
            }
        }
    }
}
