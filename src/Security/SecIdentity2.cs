//
// SecIdentity2.cs: Bindings the Security's sec_identity_t
//
// The difference between SecIdentity2 and SecIdentity is that the
// SecIdentity2 is a binding for the new sec_identity_t API that was
// introduced on iOS 12/OSX Mojave, while SecIdentity is the older API
// that binds SecIdentityRef.
//
// Authors:
//   Miguel de Icaza (miguel@microsoft.com)
//
// Copyrigh 2018 Microsoft Inc
//
using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using ObjCRuntime;
using Foundation;
using CoreFoundation;

namespace Security {

#if !NET
	[TV (12,0), Mac (10,14), iOS (12,0), Watch (5,0)]
#else
	[SupportedOSPlatform ("ios12.0")]
	[SupportedOSPlatform ("tvos12.0")]
	[SupportedOSPlatform ("macos10.14")]
#endif
	public class SecIdentity2 : NativeObject {
		internal SecIdentity2 (IntPtr handle) : base (handle, false) {}
		public SecIdentity2 (IntPtr handle, bool owns) : base (handle, owns) {}

#if !COREBUILD
		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_identity_create (IntPtr secidentityHandle);

		public SecIdentity2 (SecIdentity identity)
		{
			if (identity == null)
				throw new ArgumentNullException (nameof (identity));

			InitializeHandle (sec_identity_create (identity.Handle));
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_identity_create_with_certificates (IntPtr secidentityHandle, IntPtr arrayHandle);

		public SecIdentity2 (SecIdentity identity, params SecCertificate [] certificates)
		{
			if (identity == null)
				throw new ArgumentNullException (nameof (identity));
			if (certificates == null)
				throw new ArgumentNullException (nameof (certificates));
			using (var nsarray = NSArray.FromObjects (certificates))
				InitializeHandle (sec_identity_create_with_certificates (identity.Handle, nsarray.Handle));
		}

		[DllImport (Constants.SecurityLibrary)]
		extern static /* SecIdentityRef */ IntPtr sec_identity_copy_ref (/* OS_sec_identity */ IntPtr handle);

		public SecIdentity Identity => new SecIdentity (sec_identity_copy_ref (GetCheckedHandle ()), owns: true);

		[DllImport (Constants.SecurityLibrary)]
		extern static IntPtr sec_identity_copy_certificates_ref (IntPtr handle);

		public SecCertificate [] Certificates {
			get {
				var certArray = sec_identity_copy_certificates_ref (GetCheckedHandle ());
				try {
					return NSArray.ArrayFromHandle<SecCertificate> (certArray);
				}
				finally {
					CFObject.CFRelease (certArray);
				}
			}
		}

#if !NET
		[Watch (6,0), TV (13,0), Mac (10,15), iOS (13,0)]
#else
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos10.15")]
#endif
		[DllImport (Constants.SecurityLibrary)]
		[return: MarshalAs (UnmanagedType.I1)]
 		static extern bool sec_identity_access_certificates (IntPtr identity, ref BlockLiteral block);

		internal delegate void AccessCertificatesHandler (IntPtr block, IntPtr cert);
		static readonly AccessCertificatesHandler access = TrampolineAccessCertificates;

		[MonoPInvokeCallback (typeof (AccessCertificatesHandler))]
		static void TrampolineAccessCertificates (IntPtr block, IntPtr cert)
		{
			var del = BlockLiteral.GetTarget<Action<SecCertificate2>> (block);
			if (del != null)
				del (new SecCertificate2 (cert, false));
		}

#if !NET
		[Watch (6,0), TV (13,0), Mac (10,15), iOS (13,0)]
#else
		[SupportedOSPlatform ("ios13.0")]
		[SupportedOSPlatform ("tvos13.0")]
		[SupportedOSPlatform ("macos10.15")]
#endif
		// no [Async] as it can be called multiple times
		[BindingImpl (BindingImplOptions.Optimizable)]
		public bool AccessCertificates (Action</* sec_identity_t */SecCertificate2> handler)
		{
			if (handler == null)
				throw new ArgumentNullException (nameof (handler));

			BlockLiteral block_handler = new BlockLiteral ();
			try {
				block_handler.SetupBlockUnsafe (access, handler);
				return sec_identity_access_certificates (GetCheckedHandle (), ref block_handler);
			}
			finally {
				block_handler.CleanupBlock ();
			}
		}
#endif
	}
}
