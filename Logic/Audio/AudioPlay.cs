using System.Collections;
using UnityEngine;

namespace Custom.Logic.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlay : MonoBehaviour
    {
        [SerializeField]private AudioClip _clip;
        private AudioSource _audioSource;
        
        public void PlayClip(string path, Vector3 position)
        {
            _clip = Resources.Load<AudioClip>(path);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _clip;
            _audioSource.Play();
            StartCoroutine(DestroyWhait());
        }

        private IEnumerator DestroyWhait()
        {
            yield return new WaitForSeconds(_clip.length);
            Destroy(gameObject);
        }
    }
}
