using System;
using System.Collections;
using Custom.Logic.Audio;
using DG.Tweening;
using Engine.Coin;
using Engine.DI;
using UnityEngine;
using UnityEngine.Rendering;

namespace Custom.Logic.Coins
{
    public class CoinMover : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private float _timeSpeed = 2.5f;

        [SerializeField] private bool isStarted;
        [SerializeField] private AudioClip _clip;
        private Vector3 start;
        private Vector3 end;
        private float time;
        private ICoinsData _coinsData;
        private bool _triggert;
        private const string path = "Audio/Coin Flung";

        private void Awake()
        {
            _coinsData = DIContainer.GetAsSingle<ICoinsData>();
        }

        private void Update()
        {
            if (!isStarted) return;
            DoMoveToPoint();
           
        }

        private void DoMoveToPoint()
        {
            time += Time.deltaTime * _timeSpeed;
            Vector3 pos = Vector3.Lerp(start, end, time);
            pos.y += curve.Evaluate(time);
            transform.position = pos;
            if (time >= 1f)
            {
                // AudioManager._instance.playSound(_grassBlockAudio.clip, _grassBlockAudio.volume);
                //  _playerInfo.AddMoney(DebugUI.Value);
                isStarted=false;
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if(_triggert)
                return;
            if (other.CompareTag("Player"))
            {
                
                Collect();
            }
        }

        public void Collect()
        {
            _triggert = true;
            _coinsData.AddCoins(1);
           // GameObject obj = new GameObject();
           // AudioPlay audioPlay= obj.AddComponent<AudioPlay>();
           // audioPlay.PlayClip(path,transform.position);
            Destroy(gameObject);
        }

        public void Fly(Vector3 endPoint)
        {
            start = transform.position;
            end = endPoint;
            isStarted = true;
            enabled = true;
            time = 0f;
        }
        
    }
}
