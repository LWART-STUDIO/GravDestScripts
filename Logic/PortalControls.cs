using System.Collections;
using UnityEngine;

namespace Custom.Logic
{
    public class PortalControls : MonoBehaviour
    {
        public GameObject portalOpenGreenPrefab;
        public GameObject portalIdleGreenPrefab;
        public GameObject portalOpenYellowPrefab;
        public GameObject portalIdleYellowPrefab;
        
        private GameObject portalOpen;
        private GameObject portalIdle;

        public void OpenGreenPortal()
        {
            portalOpen = Instantiate(portalOpenGreenPrefab, transform.position, transform.rotation,transform);
            portalIdle = Instantiate(portalIdleGreenPrefab, transform.position, transform.rotation,transform);
            portalIdle.SetActive(false);
            StartCoroutine(PortalWait());
        }

        public void DestroyPortals()
        {
            if(portalIdle!=null)
                Destroy(portalIdle);
            if(portalOpen!=null)
                Destroy(portalOpen);
            Destroy(gameObject);
        }
        public void OpenYellowPortal()
        {
            portalOpen = Instantiate(portalOpenYellowPrefab, transform.position, transform.rotation,transform);
            portalIdle = Instantiate(portalIdleYellowPrefab, transform.position, transform.rotation,transform);
            portalIdle.SetActive(false);
            StartCoroutine(PortalWait());
        }

        private IEnumerator PortalWait()
        {
            portalOpen.SetActive(true);

            yield return new WaitForSeconds(0.8f);

            portalIdle.SetActive(true);
            portalOpen.SetActive(false);
        }
    }
}
