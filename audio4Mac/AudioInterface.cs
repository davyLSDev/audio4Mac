/* ? helpful info:
 * http://stackoverflow.com/questions/3880148/can-i-use-osx-augraph-from-monomac
 * http://stackoverflow.com/questions/3556886/nssound-like-framework-that-works-but-doesnt-require-dealing-with-a-steep-lear
 * 		Audio Queue services, QTkit 
 * Miquel and Mike Hutchitson give info here:
 * http://stackoverflow.com/questions/3880148/can-i-use-osx-augraph-from-monomac
 */

using System;

namespace SimpleAudio
{
	public class AudioInterface
	{
		public int ERR = 0;
		public AudioInterface ()
		{
		}
		public int Play()
		{
			return ERR;
		}
		public int Record()
		{
			return ERR;
		}
		public int Pause()
		{
			return ERR;
		}
		public int  Resume()
		{
			return ERR;
		}
	}
}
