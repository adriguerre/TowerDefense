using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AddressablesManager
{
    
    [System.Serializable]
    public class CivilianBuildingAddressables : AssetReferenceT<CivilianBuildingsSO>
    {
        public CivilianBuildingAddressables(string guid) : base(guid)
        {
        }
    }
}