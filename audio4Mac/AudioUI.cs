using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace SimpleAudio {
	class MForm : Form {
		AudioInterface Testing = new AudioInterface();
		public MForm() {
			Text = "Audio on Mac";
			Size = new System.Drawing.Size(250, 250);
		
			System.Windows.Forms.Button systemButton = new System.Windows.Forms.Button();
			systemButton.Location = new System.Drawing.Point(20, 30);
			systemButton.Text = "Your system";
			systemButton.Font = new System.Drawing.Font("Courier", 8F);
			systemButton.Click += new EventHandler(OnClickSystem);

			System.Windows.Forms.Button playButton = new System.Windows.Forms.Button();
			playButton.Location = new System.Drawing.Point(20, 60);
			playButton.Text = "Play!";
			playButton.Click += new EventHandler(OnClickPlay);
			playButton.MouseEnter += new EventHandler(OnEnter);

			System.Windows.Forms.Button recordButton = new System.Windows.Forms.Button();
			recordButton.Location = new System.Drawing.Point(20, 90);
			recordButton.Text = "Record";
			recordButton.Click += new EventHandler(OnClickRecord);

			System.Windows.Forms.Button pauseButton = new System.Windows.Forms.Button();
			pauseButton.Location = new System.Drawing.Point(20, 120);
			pauseButton.Text = "Pause!";
			pauseButton.Click += new EventHandler(OnClickPause);

			System.Windows.Forms.Button resumeButton = new System.Windows.Forms.Button();
			resumeButton.Location = new System.Drawing.Point(20, 150);
			resumeButton.Text = "Resume";
			resumeButton.Click += new EventHandler(OnClickResume);

			System.Windows.Forms.Button quitButton = new System.Windows.Forms.Button();
			quitButton.Location = new System.Drawing.Point(20, 180);
			quitButton.Text = "QUIT";
			quitButton.Click += new EventHandler(OnClickQuit);

			Controls.Add(systemButton);
			Controls.Add(playButton);
			Controls.Add (recordButton);
			Controls.Add(pauseButton);
			Controls.Add (resumeButton);
			Controls.Add (quitButton);
			CenterToScreen();
		}
	
		void OnClickSystem(object sender, EventArgs e) {
			/* Ok, this one is using code from M Hutchitson (the website noted in 'PlatformDetection.cs' */
			string machine;
			if (PlatformDetection.IsMac) {
				machine = "Your running on a Mac!";
			}
			else {
				machine = "The routine failed to detect a MacOSX machine!";
			}

			var result = MessageBox.Show (machine, "Your system info:", 
			                              MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void OnClickPlay(object sender, EventArgs e) {
			int ERR;
			ERR = Testing.Play ();
			var result = MessageBox.Show ("Play, exit code: " + ERR.ToString (), "some sound hopefuly", 
		                              MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	
		void OnEnter(object sender, EventArgs e) {
			Console.WriteLine("Button Entered");
		}

		void OnClickPause(object sender, EventArgs e) {
			int ERR;
			ERR = Testing.Pause ();
			var result = MessageBox.Show ("Pause, exit code: " + ERR.ToString (), "no sound hopefuly", 
		                             MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		void OnClickResume(object sender, EventArgs e) {
			int ERR;
			ERR = Testing.Resume ();
			var result = MessageBox.Show ("Resume, exit code: " + ERR.ToString (), "some sound hopefuly", 
		                             MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		void OnClickRecord(object sender, EventArgs e) {
			int ERR;
			ERR = Testing.Record ();
			var result = MessageBox.Show ("Record, exit code: " + ERR.ToString (), "some sound hopefuly", 
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
