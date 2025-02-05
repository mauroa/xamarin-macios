//
// Unit tests for AVPlayerItemVideoOutput
//
// Authors:
//	Sebastien Pouliot <sebastien@xamarin.com>
//
// Copyright 2016 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

using System;
using AVFoundation;
using CoreVideo;
using Foundation;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.AVFoundation {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PlayerItemVideoOutputTest {

		[Test]
		public void Ctor_CVPixelBufferAttributes ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			var attributes = new CVPixelBufferAttributes () {
				PixelFormatType = CVPixelFormatType.CV32BGRA
			};
			using (var output = new AVPlayerItemVideoOutput (attributes))
				Assert.That (output.Handle, Is.Not.EqualTo (IntPtr.Zero), "valid");
		}
	}
}

#endif // !__WATCHOS__
