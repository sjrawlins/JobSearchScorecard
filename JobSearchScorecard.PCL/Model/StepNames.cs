using System;
using System.Collections.Generic;

namespace JobSearchScorecard
{
	// Top-level Steps in the Job Search process (note: taking no chances on the underlying enum integer values)
	public enum Steps
	{
		Daily = 0,
		Attitude = 1,
		Assessments = 2,
		MarketingStrategy = 3,
		MarketingMaterials = 4,
		Interviewing = 5,
		FollowUp = 6,
		Celebrate = 7,
		//Custom = xx,   // maybe later... allow user to create their own STEP
	}

	public class StepNames
	{
		public static Dictionary<Steps, string> StepDictionary;
		public static List<string> StepList;

	    static StepNames ()  // Static constructor is automatically called once, and only once, at class-init time
		{
			int enumIndex = 0;
			StepDictionary = new Dictionary<Steps, string> ();
			StepList = new List<string> { "Daily", "Attitude", "Assessments", "Marketing Strategy", "Marketing Materials", "Interviewing",
				"Follow-up", "Celebrate",
			};
			foreach (var s in StepList) {
				StepDictionary.Add ((Steps)enumIndex, s);
				enumIndex++;
			}
		}
		public static Steps LookUpStepCodeGivenName (string stepName)
		{
			return (Steps) StepList.FindIndex (x => x.Equals(stepName));
		}
		public static string LookUpStepNameGivenCode (Steps stepCode)
		{
			if (!StepDictionary.ContainsKey (stepCode)) {
				return "Unknown";
			} else {
				return StepDictionary [stepCode];
			}
		}
	}
}