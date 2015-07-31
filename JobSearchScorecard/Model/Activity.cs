using System;

namespace JobSearchScorecard
{
	public class Activity
	{
		public Steps Step;
		public string FullName;
		public int Score;
		public bool OneTimeOnly;

		public Activity(Steps step, string fullName, int score)
		{
			Step = step;
			FullName = fullName;
			Score = score;
			OneTimeOnly = false;
		}
		public Activity(Steps step, string fullName, int score, bool oneTimeOnly)
		{
			Step = step;
			FullName = fullName;
			Score = score;
			OneTimeOnly = oneTimeOnly;
		}

		public Activity ()
		{
			Step = 0;
			FullName = "Default Activity";
			Score = 5;
			OneTimeOnly = false;
		}
	}
}

