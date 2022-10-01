using UnityEngine;

namespace Custom.Logic.Object
{
    public class ObjectParticlesControl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _holdParticals;
        [SerializeField] private ParticleSystem _fireParticals;
        [SerializeField] private ParticleSystem _freezeParticals;
        [SerializeField] private ParticleSystem _posionParticls;

        public void Posioned()
        {
            _posionParticls.Play();
        }
        public void UnPosioned()
        {
            _posionParticls.Stop();
        }

        public void Fired()
        {
            _fireParticals.Play();
        }
        public void UnFired()
        {
            _fireParticals.Stop();
        }

        public void Freezed()
        {
            _freezeParticals.Play();
        }
        public void UnFreezed()
        {
            _freezeParticals.Stop();
        }

        public void Hold()
        {
            _holdParticals.Play();
        }

        public void UnHold()
        {
            _holdParticals.Stop();
        }

    }
}
