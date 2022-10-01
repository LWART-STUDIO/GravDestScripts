using System;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

namespace Custom.Logic.Object
{
    public class ObjectDespawner : MonoBehaviour
    {
        [SerializeField] private MagneticObject[] _movingObjects;
        [SerializeField] private List<bool> activeobj;
        [SerializeField] private int minObj = 10;


        private void Awake()
        {
            activeobj = new List<bool>();
            _movingObjects = GetComponentsInChildren<MagneticObject>();
            
            foreach (MagneticObject magneticObject in _movingObjects)
            {
                bool t = true;
                activeobj.Add(t);
            }

            
        }

        private void Update()
        {
            int t = 0;
            for (int i = 0; i < _movingObjects.Length; i++)
            {
                activeobj[i] = _movingObjects[i].isActiveAndEnabled;
                if (activeobj[i] == true)
                {
                    t++;
                }
                
            }
            if (t <= minObj)
            {
             DespawnObjects();   
            }
        }

        [NaughtyAttributes.Button()]
        public void DespawnObjects()
        {
            foreach (MagneticObject magneticObject in _movingObjects)
            {

                magneticObject.ResetAll();
                magneticObject.gameObject.SetActive(true);
            }
        }
    }
}
