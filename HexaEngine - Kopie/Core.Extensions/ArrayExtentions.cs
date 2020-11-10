// <copyright file="ArrayExtentions.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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

        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            // Handles negative ends.
            if (end < 0)
            {
                end = source.Length + end;
            }
            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static T[] Add<T>(this T[] array, T value)
        {
            T[] newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[array.Length] = value;
            return newArray;
        }

        public static T[] AddToStart<T>(this T[] array, T value)
        {
            T[] newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 1);
            newArray[0] = value;
            return newArray;
        }

        public static string ArrayToString(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] ToBytes(this string str) => Encoding.UTF8.GetBytes(str);
    }
}