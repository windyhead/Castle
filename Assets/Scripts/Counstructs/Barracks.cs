
namespace BuildACastle
{
    
    using UnityEngine;
    using System.Collections.Generic;
    using TMPro;

    public class Barracks : Construct
    {
        [SerializeField] private TextMeshProUGUI _soldiersText = default;
        private readonly List<Unit> soldiers = new List<Unit>();

        private void Awake()
        {
            UpdateSoldiersText();
        }

        public void AddSoldier(Unit newSoldier)
        {
            soldiers.Add(newSoldier);
            UpdateSoldiersText();
        }

        private void UpdateSoldiersText() =>_soldiersText.text = soldiers.Count.ToString();
    }
}