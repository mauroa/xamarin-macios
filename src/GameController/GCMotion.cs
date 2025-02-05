//
// GCMotion.cs: extensions to GCMotion iOS API
//
// Authors:
//   TJ Lambert (t-anlamb@microsoft.com)
//
// Copyright 2019 Microsoft Corporation.

using System;
using System.Runtime.Versioning;

using ObjCRuntime;
using Foundation;

namespace GameController {

#if !NET
	[Introduced (PlatformName.iOS, 13, 0)]
	[Introduced (PlatformName.MacOSX, 10, 15)]
	[Introduced (PlatformName.TvOS, 13, 0)]
#else
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos10.15")]
#endif
	public struct GCAcceleration {
		public double X;
		public double Y;
		public double Z;
	}
#if !NET
	[Introduced (PlatformName.iOS, 13, 0)]
	[Introduced (PlatformName.MacOSX, 10, 15)]
	[Introduced (PlatformName.TvOS, 13, 0)]
#else
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos10.15")]
#endif
	public struct GCRotationRate {
		public double X;
		public double Y;
		public double Z;
	}

#if !NET
	[Introduced (PlatformName.iOS, 13, 0)]
	[Introduced (PlatformName.MacOSX, 10, 15)]
	[Introduced (PlatformName.TvOS, 13, 0)]
#else
	[SupportedOSPlatform ("ios13.0")]
	[SupportedOSPlatform ("tvos13.0")]
	[SupportedOSPlatform ("macos10.15")]
#endif
	public struct GCQuaternion {
		public double X;
		public double Y;
		public double Z;
		public double W;
	}
}
