using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SimpleAudio
{
	/* This class came from: https://mhut.ch/journal/2010/01/25/integrating_gtk_application_mac
	 * I've modified it a bit to make it useful for detecting Windows, Mac, or Linux platform */
	public static class PlatformDetection
	{
		public readonly static bool IsWindows;
		public readonly static bool IsLinux;
		public readonly static bool IsMac;
		public readonly static string DetectedOS;
		
		static PlatformDetection ()
		{
			IsWindows = Path.DirectorySeparatorChar == '\\';
			if (IsWindows) {
				DetectedOS = "Windows";
			} else {
				DetectedOS = GetUnixType ();
				IsLinux = (DetectedOS == "Linux");
				IsMac = (DetectedOS == "Darwin");
			}
		}
		
		//From Managed.Windows.Forms/XplatUI

		static string GetUnixType ()
		//static bool IsRunningOnMac ()
		{
			IntPtr buf = IntPtr.Zero;
			try {
				buf = System.Runtime.InteropServices.Marshal.AllocHGlobal (8192);
				// This is a hacktastic way of getting sysname from uname ()
				if (uname (buf) == 0) {
					string os = System.Runtime.InteropServices.Marshal.PtrToStringAnsi (buf);
					// if (os == "Darwin")
						return os;
				}
			} catch {
			} finally {
				if (buf != IntPtr.Zero)
					System.Runtime.InteropServices.Marshal.FreeHGlobal (buf);
			}
			return "rubbish"; 
				//false;
		}
		
		[System.Runtime.InteropServices.DllImport ("libc")]
		static extern int uname (IntPtr buf);
	}
}

