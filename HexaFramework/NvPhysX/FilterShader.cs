﻿using PhysX;

namespace HexaFramework.NvPhysX
{
    public class FilterShader : SimulationFilterShader
    {
        public override FilterResult Filter(int attributes0, FilterData filterData0, int attributes1, FilterData filterData1)
        {
            return new FilterResult
            {
                FilterFlag = FilterFlag.Default,
                // Cause PhysX to report any contact of two shapes as a touch and call SimulationEventCallback.OnContact
                PairFlags = PairFlag.ContactDefault | PairFlag.NotifyTouchFound | PairFlag.NotifyTouchLost
            };
        }
    }
}