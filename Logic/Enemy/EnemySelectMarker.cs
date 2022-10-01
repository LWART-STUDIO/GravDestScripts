using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemySelectMarker : MonoBehaviour
    {
        [SerializeField]private GameObject _selectMarker;
        public bool Selected;
        public void Select()
        {
            Selected = true;
            _selectMarker?.SetActive(true);
        }

        public void Diselect()
        {
            Selected = false;
            _selectMarker?.SetActive(false);
        }
    }
}
