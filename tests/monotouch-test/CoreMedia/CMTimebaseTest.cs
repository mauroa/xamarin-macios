//
// Unit tests for CMTimebase
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012-2014 Xamarin Inc All rights reserved.
//
using System;
using Foundation;
using CoreMedia;
using ObjCRuntime;
using NUnit.Framework;

namespace MonoTouchFixtures.CoreMedia {
	
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class CMTimebaseTest
	{
#if !__WATCHOS__
		[Test]
		public void DefaultValues ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			var htc = CMClock.HostTimeClock;
			using (var tb = new CMTimebase (htc)) {
				Assert.AreEqual (0, tb.EffectiveRate, "EffectiveRate");
				Assert.AreEqual (0, tb.Rate, "Rate");

				using (var m = tb.GetMaster ()) {
					Assert.That (m.Handle, Is.Not.EqualTo (IntPtr.Zero), "GetMaster");
				}
				using (var m = tb.GetMasterClock ()) {
					Assert.That (m.Handle, Is.Not.EqualTo (IntPtr.Zero), "GetMasterClock");
				}
				Assert.Null (tb.GetMasterTimebase (), "GetMasterTimebase");
			}
		}
#endif

		[Test]
		public void SetAnchorTime ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var tb = new CMTimebase (CMClock.HostTimeClock)) {
				Assert.AreEqual (CMTimebaseError.None, tb.SetAnchorTime (new CMTime (1000000, 200), new CMTime (-1, -2)));
				var cmt = tb.GetTime (new CMTimeScale (int.MaxValue), CMTimeRoundingMethod.QuickTime);
				Assert.AreEqual (5000, cmt.Seconds);
			}
		}

		[Test]
		public void AddTimer ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var tb = new CMTimebase (CMClock.HostTimeClock)) {
				var timer = NSTimer.CreateRepeatingTimer (CMTimebase.VeryLongTimeInterval, delegate { });

				Assert.AreEqual (CMTimebaseError.None, tb.AddTimer (timer, NSRunLoop.Current), "#1");
				Assert.AreEqual (CMTimebaseError.None, tb.SetTimerNextFireTime (timer, new CMTime (100, 2)), "#2");

				tb.RemoveTimer (timer);
			}
		}

#if !__WATCHOS__
		[Test]
		public void GetMasterTests ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var tb = new CMTimebase (CMClock.HostTimeClock)) {
				var masterTB = tb.GetMasterTimebase ();
				AssertNullOrValidHandle (masterTB, "GetMasterTimebase");

				var masterClock = tb.GetMasterClock ();
				AssertNullOrValidHandle (masterClock, "GetMasterClock");

				var master = tb.GetMaster ();
				AssertNullOrValidHandle (master, "GetMaster");

				var masterUlt = tb.GetUltimateMasterClock ();
				AssertNullOrValidHandle (masterUlt, "GetUltimateMasterClock");
			}
		}
#endif

		[Test]
		public void CopyMasterTests ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 8, throwIfOtherPlatform: false);

			using (var tb = new CMTimebase (CMClock.HostTimeClock)) {
				var masterTB = tb.CopyMasterTimebase ();
				AssertNullOrValidHandle (masterTB, "CopyMasterTimebase");

				var masterClock = tb.CopyMasterClock ();
				AssertNullOrValidHandle (masterClock, "CopyMasterClock");

				var master = tb.CopyMaster ();
				AssertNullOrValidHandle (master, "CopyMaster");

				var masterUlt = tb.CopyUltimateMasterClock ();
				AssertNullOrValidHandle (masterUlt, "CopyUltimateMasterClock");
			}
		}

		// A returned item should be null if not valid in that context or have a valid handle
		void AssertNullOrValidHandle (INativeObject o, string description)
		{
			if (o == null)
				return;
			Assert.AreNotEqual (IntPtr.Zero, o.Handle, "AssertNullOrValidHandle - " + description);
		}
	}
}
