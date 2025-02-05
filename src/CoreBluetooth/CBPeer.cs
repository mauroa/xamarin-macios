#if IOS
using ObjCRuntime;
using Foundation;
using System;
using CoreFoundation;
using System.Runtime.Versioning;

#nullable enable

namespace CoreBluetooth {
	public partial class CBPeer  {
#if !NET
		[Deprecated (PlatformName.iOS, 7, 0)]
		[Obsoleted (PlatformName.iOS, 9, 0)]
#else
		[UnsupportedOSPlatform ("ios7.0")]
#endif
		public virtual CBUUID UUID { 
			get {
				return CBUUID.FromCFUUID (_UUID);	
			}
		}
	}
}
#endif
