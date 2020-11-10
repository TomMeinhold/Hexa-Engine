// <copyright file="DirectWriteManagement.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaEngine.Core.Render.Components
{
    using System;
    using System.Collections.Generic;
    using SharpDX.DirectWrite;

    public class DirectWriteManagement : IDisposable
    {
        public DirectWriteManagement()
        {
            Factory = new Factory();
            DefaultTextFormat = new TextFormat(Factory, "Arial", 12);
        }

        ~DirectWriteManagement()
        {
            Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public Factory Factory { get; private set; }

        public TextFormat DefaultTextFormat { get; private set; }

        public List<TextFormat> TextFormats { get; } = new List<TextFormat>();

        public TextLayout GetTextLayout(string Message, TextFormat textFormat, float width)
        {
            return new TextLayout(Factory, Message, textFormat, width, textFormat.FontSize);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    Factory.Dispose();
                    DefaultTextFormat.Dispose();
                    foreach (TextFormat format in TextFormats)
                    {
                        format.Dispose();
                    }
                }

                IsDisposed = true;
            }
        }
    }
}