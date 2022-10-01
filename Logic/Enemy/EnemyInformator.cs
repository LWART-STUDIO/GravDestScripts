using System;
using Custom.Levels;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemyInformator : MonoBehaviour
    {
        [SerializeField] private EnemySelectMarker _enemySelectMarker;
        [SerializeField] private EnemyAI _enemyAI;
        [SerializeField] private EnemyType _enemyType;
        private LevelControl _levelControl;
        public bool Selected => _enemySelectMarker.Selected;

        private void Awake()
        {
            _levelControl = FindObjectOfType<LevelControl>();
        }


        public void Killed()
        {
            _levelControl.EnemyKilled();
        }

        public void Spawn()
        {
            
        }
    }
}
