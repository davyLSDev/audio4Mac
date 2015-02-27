using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using MonoMac;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.AVFoundation;
using MonoMac.AudioToolbox;
// using MonoMac.AudioUnit;
// using MonoMac.AudioUnitWrapper;

namespace SimpleAudio
{
	public class AudioInterface
	/* This opening to this class will look more like: 
	public class AudioAVFoundationSession : ISimpleAudioSession */
	{
		/* The following is in template form from libpalaso/Palaso.Media/AlsaAudio/AlsaAudioSession.cs 
		 * ... what needs to be implemented for libpalaso Media, and later Palaso.MediaTest et. al. 
		 * Might not need all this stuff ... just sucked it in for some sense of completenss. */
		/// <summary>
		/// Implementation of ISimpleAudioSession that uses the AVFoundation sound
		/// library on Mac. (or maybe s/th else?) 
		/// </summary>

		DateTime _startRecordingTime = DateTime.MinValue;
		DateTime _stopRecordingTime = DateTime.MinValue;

		AVAudioRecorder _recordingDevice;
		AVAudioPlayer _playbackDevice;
		NSUrl _pathURL;
		NSError _audioDeviceError;
		AudioSettings _audioDeviceSettings;
		#region Construction and Disposal

		/// <summary>
		/// Initialize a new instance of the <see cref="Palaso.Media.AudioAlsaSession"/> class.
		/// </summary>
		public AudioInterface (string filePath) // Likely to be renamed for libpalaso to: 'AudioAVFoundationSession'
		{
			FilePath = filePath;
			NSApplication.Init ();

			_pathURL = new NSUrl (filePath);
			_audioDeviceError = new NSError ();
			// Check out these settings against what we actually have in libPalaso
			_audioDeviceSettings = new AudioSettings {
				Format = AudioFormatType.LinearPCM,
				AudioQuality = AVAudioQuality.High,
				SampleRate = 44100f,
				NumberChannels = 1
			};

			 _recordingDevice = AVAudioRecorder.Create (_pathURL, _audioDeviceSettings, out _audioDeviceError);
			_playbackDevice = new AVAudioPlayer (_pathURL, _audioDeviceError);
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
				throw new ApplicationException("AVRecorder: Not recording on the AVFoundation sound device");
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
			get { return _recordingDevice.Recording; }
		}

		/// <summary>
		/// true iff playing a WAVE file
		/// </summary>
		public bool IsPlaying
		{
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
			_playbackDevice.Stop (); 
		}

		#endregion
	}
}
