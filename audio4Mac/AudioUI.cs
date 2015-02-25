using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace SimpleAudio {
	class MForm : Form {
		AudioInterface Testing = new AudioInterface("../../media/test.wav");
		public MForm() {
			Text = "Audio on Mac OSX Lion +";
			Size = new System.Drawing.Size(250, 250);
		
			System.Windows.Forms.Button systemButton = new System.Windows.Forms.Button();
			systemButton.Location = new System.Drawing.Point(20, 30);
			systemButton.Text = "Your system";
			systemButton.Font = new System.Drawing.Font("Courier", 8F);
			systemButton.Click += new EventHandler(OnClickSystem);

			System.Windows.Forms.Button recordButton = new System.Windows.Forms.Button();
			recordButton.Location = new System.Drawing.Point(80, 60);
			recordButton.Text = "Record";
			recordButton.Click += new EventHandler(OnClickRecord);

			System.Windows.Forms.Button stopRecording = new System.Windows.Forms.Button();
			stopRecording.Location = new System.Drawing.Point(80, 90);
			stopRecording.Text = "Stop Recording";
			stopRecording.Click += new EventHandler(OnClickStopRecordingAndSaveAsWav);

			System.Windows.Forms.Button playButton = new System.Windows.Forms.Button();
			playButton.Location = new System.Drawing.Point(80, 120);
			playButton.Text = "Play!";
			playButton.Click += new EventHandler(OnClickPlay);
			playButton.MouseEnter += new EventHandler(OnEnter);

			System.Windows.Forms.Button resumeButton = new System.Windows.Forms.Button();
			resumeButton.Location = new System.Drawing.Point(80, 150);
			resumeButton.Text = "Resume";
			resumeButton.Click += new EventHandler(OnClickResume);

			System.Windows.Forms.Button quitButton = new System.Windows.Forms.Button();
			quitButton.Location = new System.Drawing.Point(150, 180);
			quitButton.Text = "QUIT";
			quitButton.Click += new EventHandler(OnClickQuit);

			Controls.Add(systemButton);
			Controls.Add(playButton);
			Controls.Add (recordButton);
			Controls.Add(stopRecording);
			// just leave out Controls.Add (resumeButton);
			Controls.Add (quitButton);
			CenterToScreen();
		}
	
		void OnClickSystem(object sender, EventArgs e) {
			/* Ok, this one is using code from M Hutchitson (the website noted in 'PlatformDetection.cs' */
			string machine;
			if (PlatformDetection.IsMac) {
				machine = "You're running on a Mac!";
			} else if (PlatformDetection.IsLinux) {
				machine = "Your machine is Linux based.";
			} else if (PlatformDetection.IsWindows) {
				machine = "Unfortunately, yours is a windows machine.";
			} else {
				machine = PlatformDetection.DetectedOS;
			}
			/* if you need to know what 'uname s' returns, use this
			 * machine = "Your machine is " + PlatformDetection.DetectedOS + " based"; */

			var result = MessageBox.Show (machine, "Your system info:", 
			                              MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		 	public void OnClickPlay(object sender, EventArgs e) {
			/* int ERR;
			ERR = */
			Testing.Play ();
			var result = MessageBox.Show ("This is where you should hear audio.", "some sound hopefuly", 
		                              MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		void OnEnter(object sender, EventArgs e) {
			Console.WriteLine("Button Entered");
		}

			/* void OnClickPause(object sender, EventArgs e) { This is the old one */
			void OnClickStopRecordingAndSaveAsWav (object sender, EventArgs e) { 
			/* int ERR;
			ERR = Testing.Pause (); in the new AudioInterface, there is no Pause method */
			Testing.StopRecordingAndSaveAsWav ();
			var result = MessageBox.Show ("The audio should stop recording and save as wav file.", "no sound hopefuly", 
		                             MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		void OnClickResume(object sender, EventArgs e) {
			/* int ERR;
			ERR = Testing.Resume (); in the new AudioInterface, there is no Resume method */
			var result = MessageBox.Show ("The audio should resume, we hope!", "some sound hopefuly", 
		                             MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		void OnClickRecord(object sender, EventArgs e) {
			/* int ERR;
			ERR = */
			Testing.StartRecording ();
			var result = MessageBox.Show ("Speak into the microphone, we hope to record your voice.", "some sound hopefuly", 
			                              MessageBoxButtons.OK, MessageBoxIcon.Information);	
		}

		void OnClickQuit(object sender, EventArgs e) {
			Close ();	
		}
	
	}
	
	class MApplication {
		[STAThread]
		public static void Main() {
			System.Windows.Forms.Application.Run(new MForm());
		}
	}

}
