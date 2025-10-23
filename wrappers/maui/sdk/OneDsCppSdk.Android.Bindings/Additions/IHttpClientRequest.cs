using System;
using Android.Runtime;
using Java.Interop;

namespace Microsoft.Applications.Events
{
    public partial interface IHttpClientRequest : global::Java.Lang.IRunnable
    {
        public partial class Headers
        {
            [Register("remove", "", "")]
            public void Remove()
            {
            }
        }
    }
}