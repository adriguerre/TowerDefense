using System;
using UnityEngine;


namespace GameResources
{
    public class ResourcesManager : ISingleton<ResourcesManager>
    {

        private int _foodAvailable;
        private int _woodAvailable;
        private int _stoneAvailable;
        private int _ironAvailable;
        private int _goldAvailable;
        
        private int _siege1Available;
        private int _siege2Available;
        private int _siege3Available;
        
        public delegate void OnVariableChangeDelegate(double currentValue, double newValue, ResourceType resourceType);
        public event OnVariableChangeDelegate onVariableChange;

        [SerializeField] private bool usingResourcesInTestingMode = false;
        
        #region Properties
        
        public int FoodAvailable   
        {
            get { return _foodAvailable;}
            set
            {
                if (onVariableChange != null)
                {
                    onVariableChange(_foodAvailable, value, ResourceType.Food);
                    //Save Data
                }
                _foodAvailable = (int) Mathf.Clamp(value, 0, Single.PositiveInfinity);
            }
        }
        
        public int WoodAvailable   
        {
            get { return _woodAvailable;}
            set
            {
                if (onVariableChange != null)
                {
                    onVariableChange(_woodAvailable, value, ResourceType.Wood);
                    //Save Data
                }
                _woodAvailable = (int) Mathf.Clamp(value, 0, Single.PositiveInfinity);
            }
        }
        
        public int StoneAvailable   
        {
            get { return _stoneAvailable;}
            set
            {
                if (onVariableChange != null)
                {
                    onVariableChange(_stoneAvailable, value, ResourceType.Stone);
                    //Save Data
                }
                _stoneAvailable = (int) Mathf.Clamp(value, 0, Single.PositiveInfinity);
            }
        }
        
        public int IronAvailable   
        {
            get { return _ironAvailable;}
            set
            {
                if (onVariableChange != null)
                {
                    onVariableChange(_ironAvailable, value, ResourceType.Iron);
                    //Save Data
                }
                _ironAvailable = (int) Mathf.Clamp(value, 0, Single.PositiveInfinity);
            }
        }
        
        public int GoldAvailable   
        {
            get { return _goldAvailable;}
            set
            {
                if (onVariableChange != null)
                {
                    onVariableChange(_goldAvailable, value, ResourceType.Gold);
                    //Save Data
                }
                _goldAvailable = (int) Mathf.Clamp(value, 0, Single.PositiveInfinity);
            }
        }
        
        #endregion


        private void Awake()
        {
            base.Awake();
                
            //SaveManager.Instance.onDataLoaded += OnResourcesDataLoaded;
        }

        private void Update()
        {
            
        }

        public void IncreaseResources()
        {
            FoodAvailable += 2;
            WoodAvailable += 5;

            StoneAvailable += 1;
            IronAvailable += 1;
            GoldAvailable += 1;
        }      
        public void DecreaseResources()
        {
            FoodAvailable -= 2;
            WoodAvailable -= 5;

            StoneAvailable -= 1;
            IronAvailable -= 1;
            GoldAvailable -= 1;
        }
        public bool TryToSpendResources(int food, int wood, int stone, int iron, int gold)
        {
            if (usingResourcesInTestingMode)
            {
                return true;
            }
            bool canSpendResources =  FoodAvailable <= food && WoodAvailable <= wood && 
                                      StoneAvailable <= stone && IronAvailable <= iron && 
                                      GoldAvailable <= gold;

            if(canSpendResources)
            {
                FoodAvailable -= food;
                WoodAvailable -= wood;
                StoneAvailable -= stone;
                IronAvailable -= iron;
                GoldAvailable -= gold;
                return true;
            }

            return false;
        }
        /*
        private void OnResourcesDataLoaded(object sender, ResourcesDataDTO e)
        {
            LoadData(e);
        }

        private void LoadData(ResourcesDataDTO e)
        {
            PrestigePointsAvailable = e.prestigePointsAvailable;
            SoldiersAvailable = e.soldiersAvailable;
            SoldiersProduction = e.soldiersProduction;
            MineralsAvailable = e.mineralsAvailable;
            MineralsProduction = e.mineralsProduction;
            Material_A_Available = e.material_A_Available;
            Material_A_Production = e.material_A_Production;
            Material_B_Available = e.material_B_Available;
            Material_B_Production = e.material_B_Production;
            Material_C_Available = e.material_C_Available;
            Material_C_Production = e.material_C_Production;
        }
        */

    }
}