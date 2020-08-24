// <copyright file="RawColor4Extention.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Extensions
{
    using SharpDX.Mathematics.Interop;

    public static class RawColor4Extention
    {
        public static bool IsSame(this RawColor4 thisColor, RawColor4 other) => thisColor.A == other.A && thisColor.R == other.R && thisColor.G == other.G && thisColor.B == other.B;
    }
}