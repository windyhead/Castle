namespace BuildACastle
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using UnityEngine.UI;
    using TMPro;
    
    public enum ConstructType
    {
        Hut,
        Barracks
    }

    [RequireComponent(typeof(Selectable))]
    public class Construct : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText = default;
        [SerializeField] private Slider _progressBar = default;
        [SerializeField] private GameObject _mainBuilding = default;
        [SerializeField] private GameObject _bluePrint = default;
        
        public Action <Construct> OnConstructionFinished;
        public ConstructStats Stats{ get; private set; } public bool IsReady { get; private set; }
        public List<ResourceType> Resources { get; } = new List<ResourceType>();
        
        private Selectable _selectable;
        public void Init(ConstructStats stats)
        {
            _selectable = GetComponent<Selectable>();
            Stats = stats;
            SetResources();
            SetProgressBar();
            GetComponent<Collider>().enabled = true;
            _nameText.text = stats.ConstructName;
            _bluePrint.gameObject.SetActive(true);
            _mainBuilding.gameObject.SetActive(false);
        }
        
        public void FinishConstruction()
        {
            IsReady = true;
            _bluePrint.gameObject.SetActive(false);
            _mainBuilding.gameObject.SetActive(true);
            _progressBar.gameObject.SetActive(false);
            OnConstructionFinished?.Invoke(this);
        }

        public void AddResource(ResourceType resource)
        {
            Resources.Remove(resource);
        }

        public void AddConstructionProgress()
        {
            UpdateProgressBar();
            if (Resources.Count == 0)
                FinishConstruction();
        }

        public void Selected(bool isSelected)=>_selectable.Selected(isSelected);

        private void SetResources()
        {
            foreach (var resource in Stats.Resources)
                for (int i = 0; i < resource.Number; i++)
                    Resources.Add(resource.Type);
        }

        private void SetProgressBar()
        {
            _progressBar.maxValue = Resources.Count;
            _progressBar.value = 0;
        }

        private void UpdateProgressBar()=>_progressBar.value = _progressBar.maxValue - Resources.Count;
    }
}
