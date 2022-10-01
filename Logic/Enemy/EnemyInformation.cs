using System.Collections.Generic;
using Custom.Extention;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemyInformation : MonoBehaviour
    {
        public KdTree<EnemyAI> EnemyAi = new KdTree<EnemyAI>();
        [SerializeField] private List<EnemyAI> _enemyAi = new List<EnemyAI>();

        public void NewWave()
        {
            _enemyAi.Clear();
            EnemyAi.Clear();
            _enemyAi.AddRange(FindObjectsOfType<EnemyAI>());
            EnemyAi.AddAll(_enemyAi);
            foreach (var enemyAI in _enemyAi)
            {
                enemyAI.LevelStarted();
            }
        }
        private void Start()
        {
            _enemyAi.AddRange(FindObjectsOfType<EnemyAI>());
            EnemyAi.AddAll(_enemyAi);
        }

        public void RemoveEnemyAI(EnemyAI enemyAI)
        {
            _enemyAi.Remove(enemyAI);
            EnemyAi.Clear();
            EnemyAi.AddAll(_enemyAi);
        }
    }
}
