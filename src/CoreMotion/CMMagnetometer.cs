//
// CMMagnetometer.cs: Support classes
//
// Copyright 2011-2014, Xamarin Inc.
//
// Authors:
//   Miguel de Icaza 
//
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace CoreMotion {

	// CMMagnetometer.h
#if !NET
	[Mac (10,15)]
#else
	[SupportedOSPlatform ("macos10.15")]
#endif
	[StructLayout (LayoutKind.Sequential)]
	public struct CMMagneticField {
		public double X, Y, Z;

		public override string ToString ()
		{
			return string.Format ("({0},{1},{2})", X, Y, Z);
		}
	}
}
