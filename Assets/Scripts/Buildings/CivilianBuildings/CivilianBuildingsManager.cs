using System.Collections.Generic;
using BuildingsTest;
using NUnit.Framework;
using UnityEngine;

namespace Buildings.CivilianBuildings
{
    public class CivilianBuildingsManager : IBuildingManager
    {

        public static CivilianBuildingsManager Instance;
        //We will use this dicctionary to have control over all the buildings in the map, we will use this to destroy buildings when neccesary

        protected override void Awake()
        {
            base.Awake();
            if (Instance != null)
            {
                Destroy(gameObject);
                Debug.LogError("There ia already a CivilianBuildingsManager");
            }
            Instance = this;
        }
        

    }
    

}