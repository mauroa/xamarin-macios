
#if !__WATCHOS__

using System;
using Foundation;
using ObjCRuntime;
using SpriteKit;
using OpenTK;
using NUnit.Framework;

namespace MonoTouchFixtures.SpriteKit {

	[TestFixture]
	[Preserve (AllMembers = true)]
	public class PhysicsWorldTest {
		[SetUp]
		public void VersionCheck ()
		{
			TestRuntime.AssertSystemVersion (PlatformName.iOS, 8, 0, throwIfOtherPlatform: false);
			TestRuntime.AssertSystemVersion (PlatformName.MacOSX, 10, 10, throwIfOtherPlatform: false);
		}

		[Test]
		public void SampleFields ()
		{
			using (var scene = new SKScene ()) {
				using (var world = scene.PhysicsWorld) {
					var v = world.SampleFields (new Vector3 (1, 2, 3));
					Assert.AreEqual (0, v.X, "#x1");
					Assert.AreEqual (0, v.Y, "#y1");
					Assert.AreEqual (0, v.Z, "#z1");
				}
			}
		}
	}
}

#endif // !__WATCHOS__
