// Imported from: https://github.com/NielsKuhnel/NrtManager/tree/master/Lucene.Net.Contrib.Management

using System;

namespace EDFI.Modules.Search.Lucene
{
    public static class DisposeUtil
    {
        public static void PostponeExceptions(params Action[] disposeActions)
        {
            Exception firstException = null;
            foreach (var d in disposeActions)
            {
                try
                {
                    d();
                }
                catch (Exception ex)
                {
                    firstException = firstException ?? ex;
                }
            }

            if (firstException != null) throw firstException;
        }
    }
}