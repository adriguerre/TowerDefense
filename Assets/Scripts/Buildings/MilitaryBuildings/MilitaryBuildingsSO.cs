using BuildingsTest;
using UnityEngine;

namespace Buildings.MilitaryBuildings
{
    [CreateAssetMenu(fileName = "CivilianBuildingsSO", menuName = "ScriptableObjects/MilitaryBuildings/MilitaryBuildings")]
    public class MilitaryBuildingsSO : IBuildingsSO
    {
        public bool singleTarget;
        public bool canPlaceInRoad;
    }
}