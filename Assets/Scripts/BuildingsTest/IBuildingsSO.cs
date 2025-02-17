using GameResources;
using NaughtyAttributes;
using UnityEngine;

namespace BuildingsTest
{
    public abstract class IBuildingsSO : ScriptableObject
    {
        private int[] sizeValues = new int[] { 4, 6 };
    
        public int buildingID;
        [ShowAssetPreview]
        public Sprite buildingIcon;
        public string buildingName;
        [TextArea(10,30)]
        public string buildingDescription;
        public GameObject buildingPrefab;
        public int timeToBuild;
        public int timeToUpgrade;
        [Dropdown("sizeValues")]
        public int buildSize;
    
        public ResourceCost buildingCost1;
        //this can be null, indicating that only cost 1 resource
        public ResourceCost buildingCost2;
    }
}