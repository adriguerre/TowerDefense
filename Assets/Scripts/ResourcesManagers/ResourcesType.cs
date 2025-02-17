
using System;

namespace GameResources
{
    public enum ResourceType
    {
        Undefined, Food, Wood, Stone, Iron, Gold
    }
    [Serializable]
    public class ResourceCost
    {
        public ResourceType resourceType;
        public int cost;
    }

    [Serializable]
    public class ResourceProduced
    {
        public ResourceType resourceProduced;
        public int resourceProducedBaseLevel1;
        public int resourceProducedBaseLevel2;
        public int resourceProducedBaseLevel3;

        public int timeToProduceResourceBaseLevel1;
        public int timeToProduceResourceBaseLevel2;
        public int timeToProduceResourceBaseLevel3;
    }

}

