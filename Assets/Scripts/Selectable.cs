using UnityEditor;

namespace BuildACastle
{
   using System;
    using UnityEngine;

    public class Selectable: MonoBehaviour
    {
        [SerializeField] private GameObject selection = default;

        private void Awake()
        {
            DeSelected();
        }

        public void Selected()
        {
           selection.SetActive(true);
        }

        public void DeSelected()
        {
           selection.SetActive(false);
        }
    }
}
