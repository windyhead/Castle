namespace BuildACastle
{
   using UnityEngine.UI;
   using UnityEngine;

   public class BuildingPanel : MonoBehaviour
   {
      [SerializeField] private GameHandler _gameHandler = default;
      [SerializeField] private Button hut = default;
      [SerializeField] private Button barracks = default;

      private void Awake()
      {
         hut.onClick.AddListener(BuildHut);
         barracks.onClick.AddListener(BuildBarracks);
      }

      private void BuildHut() => _gameHandler.StartConstruction(ConstructType.Hut);
      private void BuildBarracks() => _gameHandler.StartConstruction(ConstructType.Barracks);
   }
}
