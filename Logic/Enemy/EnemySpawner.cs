using System;
using System.Collections;
using System.Collections.Generic;
//using Custom.Logic.UI;
using Engine;
using Engine.DI;
using example1;
using Main;
using Main.Level;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Custom.Logic
{
    public class EnemySpawner : MonoBehaviour,ILevelStarted,ILevelCompleted, ILevelFailed
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _count=10;
        [SerializeField] private float _delay=3f;
        [SerializeField] private float _spawnDistance = 30f;
        private ILevel _levelInfo;
        [SerializeField] private Collider _floorColider;
        [SerializeField] private Transform[] _spawnPoints;


        public void OnEnable()
        {
            
            LevelStatueStarted.Subscribe(this);
            LevelStatueFailed.Subscribe(this);
      
        }
        public void OnDisable()
        {
            LevelStatueStarted.Unsubscribe(this);
            LevelStatueFailed.Unsubscribe(this);
        }

        public void SetUp(int count, float delay)
        {
            _count = count;
            _delay = delay;
            
        }
        

        public void LevelStarted()
        {
            _levelInfo = DIContainer.GetAsSingle<ILevelsManager>().level;
           // SetUp(_levelInfo.WaveInfos[_levelInfo.CurrentWave].NumberOfEnemys,_levelInfo.WaveInfos[_levelInfo.CurrentWave].DelayBetweenSpawn);
            //StartCoroutine(SpawningEnemy());
           
        }

        

        public void LevelCompleted()
        {
           StopAllCoroutines();
        }

        public void LevelFailed()
        {
            StopAllCoroutines();
        }
    }
}
