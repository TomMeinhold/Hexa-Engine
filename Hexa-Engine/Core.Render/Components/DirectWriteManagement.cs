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
            this.Factory = new Factory();
            this.DefaultTextFormat = new TextFormat(this.Factory, "Arial", 12);
        }

        ~DirectWriteManagement()
        {
            this.Dispose(false);
        }

        public bool IsDisposed { get; private set; } = false;

        public Factory Factory { get; private set; }

        public TextFormat DefaultTextFormat { get; private set; }

        public List<TextFormat> TextFormats { get; } = new List<TextFormat>();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.Factory.Dispose();
                    this.DefaultTextFormat.Dispose();
                    foreach (TextFormat format in this.TextFormats)
                    {
                        format.Dispose();
                    }
                }

                this.IsDisposed = true;
            }
        }
    }
}