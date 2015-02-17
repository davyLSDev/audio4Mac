/* ? helpful info:
 * http://stackoverflow.com/questions/3880148/can-i-use-osx-augraph-from-monomac
 * http://stackoverflow.com/questions/3556886/nssound-like-framework-that-works-but-doesnt-require-dealing-with-a-steep-lear
 * 		Audio Queue services, QTkit 
 * Miquel and Mike Hutchitson give info here:
 * http://stackoverflow.com/questions/3880148/can-i-use-osx-augraph-from-monomac
 */
// CHECK some of these may not be needed!
using MonoMac;
using MonoMac.Foundation;
using MonoMac.AVFoundation;
using MonoMac.AppKit;
// using MonoMac.AudioUnitWrapper;
using MonoMac.AudioToolbox;
using MonoMac.AudioUnit;

namespace SimpleAudio
{
	public class AudioInterface
	{
		public int ERR = 0;
		public NSSound playback = new NSSound ("../../media/applause_y.wav", byRef: false);

		public AudioInterface ()
		{
			NSApplication.Init ();
		}
		public int Play()
		{
			playback.Play ();
			return ERR;
		}
		public int Record()
		{
			return ERR;
		}
		public int Pause()
		{
			playback.Pause ();
			return ERR;
		}
		public int  Resume()
		{
			playback.Resume ();
			return ERR;
		}
	}
}
