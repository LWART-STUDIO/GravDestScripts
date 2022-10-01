using UnityEngine;

namespace Custom.Logic.Coins
{
    public class CoinSoundControl : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
            }
        }
    }
}
