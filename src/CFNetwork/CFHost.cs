//
// MonoMac.CoreServices.CFHost
//
// Authors:
//      Martin Baulig (martin.baulig@xamarin.com)
//
// Copyright 2012-2015 Xamarin Inc. (http://www.xamarin.com)
//

using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using CoreFoundation;
using Foundation;
using ObjCRuntime;

// CFHost is in CFNetwork.framework, no idea why it ended up in CoreServices when it was bound.
#if XAMCORE_4_0
namespace CFNetwork {
#else
namespace CoreServices {
#endif

#if !NET
	[Deprecated (PlatformName.WatchOS, 8, 0, message: Constants.UseNetworkInstead)]
	[Deprecated (PlatformName.TvOS, 15, 0, message: Constants.UseNetworkInstead)]
 	[Deprecated (PlatformName.iOS, 15, 0, message: Constants.UseNetworkInstead)]
 	[Deprecated (PlatformName.MacCatalyst, 15, 0, message: Constants.UseNetworkInstead)]
 	[Deprecated (PlatformName.MacOSX, 12, 0, message: Constants.UseNetworkInstead)]
#else
	[UnsupportedOSPlatform ("ios15.0")]
	[UnsupportedOSPlatform ("tvos15.0")]
	[UnsupportedOSPlatform ("maccatalyst15.0")]
	[UnsupportedOSPlatform ("macos12.0")]
#if __MACCATALYST__
	[Obsolete ("Starting with maccatalyst15.0 use 'Network.framework' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#elif IOS
	[Obsolete ("Starting with ios15.0 use 'Network.framework' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#elif TVOS
	[Obsolete ("Starting with tvos15.0 use 'Network.framework' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#elif MONOMAC
	[Obsolete ("Starting with macos12.0 use 'Network.framework' instead.", DiagnosticId = "BI1234", UrlFormat = "https://github.com/xamarin/xamarin-macios/wiki/Obsolete")]
#endif
#endif
	class CFHost : INativeObject, IDisposable {
		internal IntPtr handle;

		CFHost (IntPtr handle)
		{
			this.handle = handle;
		}

		~CFHost ()
		{
			Dispose (false);
		}
		
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		public IntPtr Handle {
			get { return handle; }
		}
		
		protected virtual void Dispose (bool disposing)
		{
			if (handle != IntPtr.Zero){
				CFObject.CFRelease (handle);
				handle = IntPtr.Zero;
			}
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHostRef __nonnull */ IntPtr CFHostCreateWithAddress (
			/* CFAllocatorRef __nullable */ IntPtr allocator, /* CFDataRef __nonnull */ IntPtr addr);

		public static CFHost Create (IPEndPoint endpoint)
		{
			// CFSocketAddress will throw the ANE
			using (var data = new CFSocketAddress (endpoint))
				return new CFHost (CFHostCreateWithAddress (IntPtr.Zero, data.Handle));
		}

		[DllImport (Constants.CFNetworkLibrary)]
		extern static /* CFHostRef __nonnull */ IntPtr CFHostCreateWithName (
			/* CFAllocatorRef __nullable */ IntPtr allocator, /* CFStringRef __nonnull */ IntPtr hostname);

		public static CFHost Create (string name)
		{
			// CFString will throw the ANE
			using (var ptr = new CFString (name))
				return new CFHost (CFHostCreateWithName (IntPtr.Zero, ptr.Handle));
		}
	}
}
