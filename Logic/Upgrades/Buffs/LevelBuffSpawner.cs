using System.Collections.Generic;
using Custom.Levels;
using Engine.DI;
using UnityEngine;

namespace Custom.Logic.Upgrades.Buffs
{
    public class LevelBuffSpawner : MonoBehaviour
    {
        private ILevelBufsControll _levelBufsControll;
        private List<LevelBuff> _levelBuffs;
        [SerializeField] private List<Transform> _spawnPoints;
        private List<GameObject> _buffsObjects;
        private LevelControl _levelControl;
        private void Start()
        {
            _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
            _levelControl = FindObjectOfType<LevelControl>();
        }

        public void SpawnBuffs()
        {
            _levelBuffs = new List<LevelBuff>();
            _buffsObjects = new List<GameObject>();
            _levelBuffs = _levelBufsControll.GetThreeRandomBuffs();
            for (int index = 0; index < _levelBuffs.Count; index++)
            {
                LevelBuff levelBuff = _levelBuffs[index];
                GameObject buffObject =
                    Instantiate(levelBuff.BuffObject, _spawnPoints[index].position, Quaternion.Euler(0,90,-90),transform);
                LevelBuffObject levelBuffObject = buffObject.GetComponent<LevelBuffObject>();
                levelBuffObject.Init(this,levelBuff.BuffId);
                _buffsObjects.Add(buffObject);
            }
            /*LevelBuff levelBuff = _levelBufsControll.LevelsBuffs[Random.Range(0, _levelBufsControll.LevelsBuffs.Count)];
            GameObject buffObject =
                Instantiate(levelBuff.BuffObject, _spawnPoints[0].position, Quaternion.identity);
            LevelBuffObject levelBuffObject = buffObject.GetComponent<LevelBuffObject>();
            levelBuffObject.Init(this,levelBuff.BuffId);
            _levelBuffs.Add(levelBuff);*/
            
        }

        public void DestroyBuffsObjects()
        {
            foreach (GameObject buffsObject in _buffsObjects)
            {
                Destroy(buffsObject);
            }

            _levelControl._buffsCollected = true;
        }
    }
}
