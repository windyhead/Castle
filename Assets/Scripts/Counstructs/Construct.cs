using TMPro;

namespace BuildACastle
{

    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using UnityEngine.UI;
    
    public enum ConstructType
    {
        Hut,
        Barracks
    }

    [RequireComponent(typeof(Selectable))]
    public class Construct : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI soldiersText;
        public Action <Construct> OnConstructionFinished;
        public ConstructStats Stats{ get; private set; }
        private Selectable _selectable;
        private int soldiers = 0;
        public bool IsReady { get; set; } = false;
        public List<ResourceType> Resources { get; } = new List<ResourceType>();

        public void Init(ConstructStats stats)
        {
            _selectable = GetComponent<Selectable>();
            Stats = stats;
            SetResources();
            GetComponent<Collider>().enabled = true;
            if (soldiersText != null)
            soldiersText.gameObject.SetActive(false);
        }

        public void AddResource(ResourceType resource)
        {
            Resources.Remove(resource);
            if (Resources.Count == 0)
            {
                IsReady = true;
                OnConstructionFinished?.Invoke(this);
            }
        }

        public void  AddSoldier()
        {
            soldiersText.gameObject.SetActive(true);
            soldiers++;
            soldiersText.text = soldiers.ToString();
        }

        public void Selected() => _selectable.Selected();
        public void Deselected() => _selectable.DeSelected();

        private void SetResources()
        {
            foreach (var resource in Stats.Resources)
                for (int i = 0; i < resource.Number; i++)
                    Resources.Add(resource.Type);
        }
    }
}
