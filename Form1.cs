using System;
using System.IO;
using System.Windows.Forms;

namespace Timer
{ 
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer dayTimer;
        private string timerFilePath = "timerData.txt";

        public Form1()
        {
            InitializeComponent();
            InitializeDayTimer();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeDayTimer()
        {
            dayTimer = new System.Windows.Forms.Timer { Enabled = false };  // Initialize without starting

            // Try to read the next elapse time from the file
            if (File.Exists(timerFilePath))
            {
                string nextElapseStr = File.ReadAllText(timerFilePath);
                if (DateTime.TryParse(nextElapseStr, out DateTime nextElapse))
                {
                    if (DateTime.Now >= nextElapse)
                    {
                        // Previous elapse time has passed. Perform the actions and reset timer.
                        TimesUp();
                    }
                    else
                    {
                        // There is still time until the next elapse. Set the timer interval accordingly.
                        TimeSpan timeUntilNextElapse = nextElapse - DateTime.Now;
                        dayTimer.Interval = (int)timeUntilNextElapse.TotalMilliseconds;
                        dayTimer.Enabled = true;  // Start only after setting the interval.
                    }
                }
                else
                {
                    // If parsing failed, reset the timer for the next day.
                    TimesUp();
                }
            }
            else
            {
                // If there is no saved time, reset the timer for the next day.
                TimesUp();
            }

            dayTimer.Tick += DayTimer_Tick;
        }

        private void DayTimer_Tick(object sender, EventArgs e)
        {
            TimesUp();
        }

        private void TimesUp()
        {
            MessageBox.Show("The timer has elapsed! Resetting for another day.", "Timer Elapsed");

            ResetTimerForNextDay();
        }

        private void ResetTimerForNextDay()
        {
            // Set the timer to elapse again in 24 hours
            dayTimer.Interval = 15000; // 24 hours in milliseconds
            dayTimer.Enabled = true; // Start the timer

            // Calculate the next elapse time and save it to the file
            DateTime nextElapse = DateTime.Now.AddMilliseconds(dayTimer.Interval);
            File.WriteAllText(timerFilePath, nextElapse.ToString());
        }
    }
}