using System;
using UnityEngine;

namespace SaveManager
{
    [Serializable]
    public class ResourcesDataDTO
    {
        public int FoodAvailable;
        public int WoodAvailable;
        public int StoneAvailable;
        public int IronAvailable;
        public int GoldAvailable;        
        public int FoodProduction;
        public int WoodProduction;
        public int StoneProduction;
        public int IronProduction;
        public int GoldProduction;

        public ResourcesDataDTO(int foodAvailable, int woodAvailable, int stoneAvailable, int ironAvailable, int goldAvailable, int foodProduction, int woodProduction, int stoneProduction, int ironProduction, int goldProduction)
        {
            this.FoodAvailable = foodAvailable;
            this.WoodAvailable = woodAvailable;
            this.StoneAvailable = stoneAvailable;
            this.IronAvailable = ironAvailable;
            this.GoldAvailable = goldAvailable;
            this.FoodProduction = foodProduction;
            this.WoodProduction = woodProduction;
            this.StoneProduction = stoneProduction;
            this.IronProduction = ironProduction;
            this.GoldProduction = goldProduction;
        }
    }
}