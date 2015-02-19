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

using System;
using System.IO;


/* note in libpalaso/Palaso.Media/AudioFactory.cs is where the audio interface is
 * chosen based on if the platform is MONO or not. If it is, then ALSA is used and AudioFactory returns 
 * "AudioAlsaSession (filepath)", otherwise windows returns "AudioIrrKlangSession(filePath)";
 *  there is a AudioGStreamerSession, and AudioNullSession ...
 *  looked into using GStreamer ... it uses QTKit, 
 * 	looked into SDL / and C# wrapper for that HMMMM
 */

namespace SimpleAudio
{
	public class AudioInterface
	/* This opening to this class will look more like: 
	public class AudioAVFoundationSession : ISimpleAudioSession */
	{
		// first attempt ... note some overloading of methods from here to the new AudioAVFoundationSession work
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

		/* The following is in template form from libpalaso/Palaso.Media/AlsaAudio/AlsaAudioSession.cs 
		 * ... what needs to be implemented for libpalaso Media, and later Palaso.MediaTest et. al. 
		 * Might not need all this stuff ... just sucked it in for some sense of completenss. */
		/// <summary>
		/// Implementation of ISimpleAudioSession that uses the AVFoundation sound
		/// library on Mac. (or maybe s/th else?) 
		/// </summary>

		DateTime _startRecordingTime = DateTime.MinValue;
		DateTime _stopRecordingTime = DateTime.MinValue;
		// AlsaAudioDevice _device;
		AVAudioRecorder _recordingDevice;
		AVAudioPlayer _playbackDevice;
		#region Construction and Disposal

		/// <summary>
		/// Initialize a new instance of the <see cref="Palaso.Media.AudioAlsaSession"/> class.
		/// </summary>
		public AudioInterface (string filePath)
		{
			FilePath = filePath;
			// _device = new AlsaAudioDevice();
			_recordingDevice = new AVAudioRecorder ();
			_playbackDevice = new AVAudioPlayer ();
		}

		#endregion

		#region Implementation of ISimpleAudioSession

		/// <summary>
		/// Gets the path to the sound file, as established by the constructor.
		/// </summary>
		public string FilePath
		{
			get;
			private set;
		}

		/// <summary>
		/// Start recording.
		/// </summary>
		public void StartRecording()
		{
			if (!CanRecord)
				// throw new ApplicationException("AlsaAudioSession: Already recording or playing on the ALSA sound device");
				throw new ApplicationException("AVAudioRecorder: Already recording or playing on the AVFoundation sound device");
			_stopRecordingTime = DateTime.MinValue;
			_startRecordingTime = DateTime.Now;
			// bool fOk = _device.StartRecording();
			bool fOk = _recordingDevice.PrepareToRecord ();
			if (!fOk)
				// throw new Exception("AlsaAudioSession: Cannot open the ALSA sound device");
				throw new Exception("AVRecorder: Cannot open the AVFoundation sound device");
			fOk = _recordingDevice.Record ();
			if (!fOk)
				// this exception was not part of the ALSA implementation
				throw new Exception("AVRecorder: Cannot record on the AVFoundation sound device");
		}

		/// <summary>
		/// Stop the recording and save it as a WAVE file.
		/// </summary>
		public void StopRecordingAndSaveAsWav()
		{
			if (!IsRecording)
				// throw new ApplicationException("AlsaAudioSession: Not recording on the ALSA sound device");
				throw new ApplicationException("AVRecorder: Not recording on the AVFoundation sound device");
			// _device.StopRecording();
			_recordingDevice.Stop ();
			_stopRecordingTime = DateTime.Now;
			SaveAsWav(FilePath);
		}

		/// <summary>
		/// Get the length of the most recent recording in milliseconds.
		/// </summary>
		public double LastRecordingMilliseconds
		{
			get
			{
				if (_startRecordingTime == DateTime.MinValue || _stopRecordingTime == DateTime.MinValue)
					return 0.0;
				else
					return (_stopRecordingTime - _startRecordingTime).TotalMilliseconds;
			}
		}

		/// <summary>
		/// true iff recording is underway.
		/// </summary>
		public bool IsRecording
		{
			// get { return _device.IsRecording; }
			get { return _recordingDevice.Recording; }
		}

		/// <summary>
		/// true iff playing a WAVE file
		/// </summary>
		public bool IsPlaying
		{
			// get { return _device.IsPlaying; }
			get { return _playbackDevice.Playing; }
		}

		/// <summary>
		/// true iff neither recording nor playing.
		/// </summary>
		public bool CanRecord
		{
			get { return !IsPlaying && !IsRecording; }
		}

		/// <summary>
		/// true iff either playing or recording.
		/// </summary>
		public bool CanStop
		{
			get { return IsPlaying || IsRecording; }
		}

		/// <summary>
		/// true iff neither playing nor recording.
		/// </summary>
		public bool CanPlay
		{
			get { return !IsPlaying && !IsRecording; }
		}

		/// <summary>
		/// Play the sound file set by the constructor.
		/// </summary>
		public void Play()
		{
			if (!CanPlay)
				throw new ApplicationException("AVAudioPlayer: Already recording or playing on the AVFoundation sound device");
			if (!File.Exists(FilePath))
				throw new FileNotFoundException(String.Format("AVAudioPlayer: {0} does not exist", FilePath));
			// bool fOk = _device.StartPlaying(FilePath);
			bool fOk = _playbackDevice.Play (); // *** note we have to load up the 'FilePath' somewhere ...
			if (!fOk)
				throw new Exception("AVAudioPlayer: Cannot open the AVFoundation sound device");
		}

		/// <summary>
		/// Saves the sound recording as a WAVE file.  (I don't see why this is a separate interface method.)
		/// </summary>
		public void SaveAsWav(string filePath)
		{
			// _device.SaveAsWav(filePath);
			_recordingDevice.Stop (); // not sure this is what we want!
		}

		/// <summary>
		/// Stop playing the sound file.
		/// </summary>
		public void StopPlaying()
		{
			// _device.StopPlaying();
			_playbackDevice.FinishedPlaying;
		}

		#endregion
	}
}
