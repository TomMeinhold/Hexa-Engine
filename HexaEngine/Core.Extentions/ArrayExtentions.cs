// <copyright file="ArrayExtentions.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extentions
{
    using System;
    using System.Collections.Generic;
    using SharpDX.Mathematics.Interop;

    public static class ArrayExtentions
    {
        public static int GetIndexOfFirst<T>(this T[] ts, T state)
            where T : struct
        {
            int i = 0;
            foreach (T t in ts ?? throw new ArgumentNullException(nameof(ts)))
            {
                if (t.Equals(state))
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public static float ClosestTo(this IEnumerable<float> collection, float target)
        {
            // NB Method will return int.MaxValue for a sequence containing no elements. Apply any
            // defensive coding here as necessary.
            var closest = float.MaxValue;
            var minDifference = float.MaxValue;
            foreach (var element in collection ?? throw new ArgumentNullException(nameof(collection)))
            {
                var difference = Math.Abs(element - target);
                if (minDifference > difference)
                {
                    minDifference = difference;
                    closest = element;
                }
            }

            return closest;
        }

        public static (List<float>, List<float>) SplitCoordinates(this List<RawVector2> vector2s)
        {
            List<float> xs = new List<float>();
            List<float> ys = new List<float>();
            foreach (RawVector2 vec in vector2s ?? throw new ArgumentNullException(nameof(vector2s)))
            {
                xs.Add(vec.X);
                ys.Add(vec.Y);
            }

            return (xs, ys);
        }

        public static float ClosestToX(this IEnumerable<RawVector2> collection, float target)
        {
            // NB Method will return int.MaxValue for a sequence containing no elements. Apply any
            // defensive coding here as necessary.
            var closest = float.MaxValue;
            var minDifference = float.MaxValue;
            foreach (var element in collection ?? throw new ArgumentNullException(nameof(collection)))
            {
                var difference = Math.Abs(element.X - target);
                if (minDifference > difference)
                {
                    minDifference = difference;
                    closest = element.X;
                }
            }

            return closest;
        }
    }
}