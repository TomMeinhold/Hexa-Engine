using System;

namespace HexaFramework.Resources
{
    public class Resource : IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
        }

        ~Resource()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}