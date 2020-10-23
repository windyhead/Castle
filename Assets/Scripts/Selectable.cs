namespace BuildACastle
{
   using System;
    using UnityEngine;

    public class Selectable: MonoBehaviour
    {
        [SerializeField] private GameObject selection = default;

        private void Awake()=>Selected(false);
        public void Selected(bool isSelected)=>selection.SetActive(isSelected);
    }
}
