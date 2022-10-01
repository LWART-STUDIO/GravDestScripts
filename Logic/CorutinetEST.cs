using System;
using System.Collections;
using UnityEngine;

namespace Custom.Logic
{
    public class CorutinetEST : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(StartWork());
        }

        private IEnumerator StartWork()
        {
            Coroutine corutine=StartCoroutine(Work());
            
            yield return new WaitForSeconds(10f);
            StopCoroutine(corutine);
        }

        private IEnumerator Work()
        {
            while (true)
            {
                Debug.Log("Damage");
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
