using System;
using System.Collections;
using SaveManager;
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
        
        private int _foodProduction;
        private int _woodProduction;
        private int _stoneProduction;
        private int _ironProduction;
        private int _goldProduction;
        
        private int _siege1Available;
        private int _siege2Available;
        private int _siege3Available;
        
        [Header("Resources Sprites")] 
        [SerializeField] protected Sprite foodSprite;
        [SerializeField] protected Sprite woodSprite;
        [SerializeField] protected Sprite stoneSprite;
        [SerializeField] protected Sprite ironSprite;
        [SerializeField] protected Sprite goldSprite;
        
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
                
            SaveManager.SaveManager.Instance.onDataLoaded += OnResourcesDataLoaded;
        }

        private void OnResourcesDataLoaded(object sender, ResourcesDataDTO e)
        {
            LoadResourcesData(e);
        }



        private void Start()
        {
            SaveManager.SaveManager.Instance.LoadResourcesDataEncrypted();
            //MilitaryUpgradesManager.Instance.onProductionValueChanged += OnProductionValueChanged;
            StartCoroutine(StartProduction());
        }

        private IEnumerator StartProduction()
        {
            while (true)
            {
                FoodAvailable += _foodProduction;
                WoodAvailable += _woodProduction;

                StoneAvailable += _stoneProduction;
                IronAvailable += _ironProduction;
                GoldAvailable += _goldProduction;
                
                yield return new WaitForSeconds(1f);
            }
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
        
        private void LoadResourcesData(ResourcesDataDTO resourcesDataDto)
        {
            this._foodAvailable = resourcesDataDto.FoodAvailable;
            this._woodAvailable = resourcesDataDto.WoodAvailable;
            this._stoneAvailable = resourcesDataDto.StoneAvailable;
            this._ironAvailable = resourcesDataDto.IronAvailable;
            this._goldAvailable = resourcesDataDto.GoldAvailable;
            this._foodProduction = resourcesDataDto.FoodProduction;
            this._woodProduction = resourcesDataDto.WoodProduction;
            this._stoneProduction = resourcesDataDto.StoneProduction;
            this._ironProduction = resourcesDataDto.IronProduction;
            this._goldProduction = resourcesDataDto.GoldProduction;
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

        public void IncreaseResourceProduction(ResourceType resourceType, int amount)
        {
            switch (resourceType)
            {
                case ResourceType.Undefined:
                    break; 
                case ResourceType.Food:
                    _foodProduction += amount;
                    break; 
                case ResourceType.Wood:
                    _woodProduction += amount;
                    break; 
                case ResourceType.Stone:
                    _stoneProduction += amount;
                    break; 
                case ResourceType.Iron:
                    _ironProduction += amount;
                    break; 
                case ResourceType.Gold:
                    _goldProduction += amount;
                    break; 
            }
        }

        public bool GetIfHasResources(ResourceCost resourceCost)
        {
            switch (resourceCost.resourceType)
            {
                case ResourceType.Undefined:
                    break; 
                case ResourceType.Food:
                    return _foodAvailable >= resourceCost.cost;
                    break; 
                case ResourceType.Wood:
                    return _woodAvailable >= resourceCost.cost;
                    break; 
                case ResourceType.Stone:
                    return _stoneAvailable >= resourceCost.cost;
                    break; 
                case ResourceType.Iron:
                    return _ironAvailable >= resourceCost.cost;
                    break; 
                case ResourceType.Gold:
                    return _goldAvailable >= resourceCost.cost;
                    break; 
            }

            return false;
        }
        public ResourcesDataDTO GetResourcesDataDTO()
        {
            return new ResourcesDataDTO(_foodAvailable, _woodAvailable, _stoneAvailable,
                _ironAvailable, _goldAvailable, _foodProduction, _woodProduction, _stoneProduction, _ironProduction,
                _goldProduction);
        }
        
        
        
        /// <summary>
        /// This is a helper method, used to get sprites from resources
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        public virtual Sprite GetSpriteFromResource(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Undefined:
                    break; 
                case ResourceType.Food:
                    return foodSprite;
                    break; 
                case ResourceType.Wood:
                    return woodSprite;
                    break; 
                case ResourceType.Stone:
                    return stoneSprite;
                    break; 
                case ResourceType.Iron:
                    return ironSprite;
                    break; 
                case ResourceType.Gold:
                    return goldSprite;
                    break; 
            }
            return null;
        }

    }
}