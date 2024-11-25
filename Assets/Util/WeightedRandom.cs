using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.Extensions;

namespace Custom.WeightedRandom
{
    [System.Serializable]
    public class WeightedRandom
    {
        public AnimationCurve weightCurve;
        public float min = 0;
        public float max = 1;

        public float Get()
        {
            return weightCurve.Evaluate(Random.value).Remap(0, 1, min, max, true);
        }
    }
}