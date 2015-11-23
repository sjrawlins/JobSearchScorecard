using System;
using System.Collections.Generic;

namespace JobSearchScorecard
{
	// One of the sub-steps in the Job Search process, an Activity belongs to a Step, has a name and a score
	// (and it might only be available once in a lifetime)
	// This data, these Activities, are used for REFERENCE (i.e. Lookup - to get the score) and are not stored on the DB

	public class Activity
	{
		public Steps Step;
		public int SubStep;
		public static int UniqueCode = 0;
		public string FullName;
		public int Score;
		public bool OneTimeOnly;

		public Activity (Steps step, string fullName, int score)
		{
			Step = step;
			SubStep = UniqueCode++;
			FullName = fullName;
			Score = score;
			OneTimeOnly = false;
		}

		public Activity (Steps step, string fullName, int score, bool oneTimeOnly)
		{
			Step = step;
			SubStep = UniqueCode++;
			FullName = fullName;
			Score = score;
			OneTimeOnly = oneTimeOnly;
		}

		public Activity ()
		{
			Step = 0;
			SubStep = 0;
			FullName = "Default Activity";
			Score = 0;
			OneTimeOnly = false;
		}
	}

	public class ActivityTable
	{
		public static Dictionary<Steps, List<Activity>> Activities = new Dictionary<Steps, List<Activity>> (10);

		public static void BuildActivitiesDictionary ()
		{

			// Initialize the Activities and corresponding scores
			// The look-up key is unique and so Dictionary will bark (runtime failure) if any duplicates - so watch out
			// Some activities are Only Once-in-a-lifetime (typically "Read Documents")

			// STEP 0 - ALL
			Activities.Add (Steps.EveryDay, new List<Activity> (8) { 
				new Activity (Steps.EveryDay, "Prayer", 10),
				new Activity (Steps.EveryDay, "Read Scripture", 10),
				new Activity (Steps.EveryDay, "Share daily plans with family", 4),
			}
			);   // subSteps 0 .. 2

			// STEP 1 - ATTITUDE
			Activities.Add (Steps.Attitude, new List<Activity> (16) {
				new Activity (Steps.Attitude, "Read Documents and list 3 most important items", 5, true),
				new Activity (Steps.Attitude, "List negative attitudes", 10),
				new Activity (Steps.Attitude, "Take Attitude Quiz", 10),
				new Activity (Steps.Attitude, "List 2 negative attitudes that must change", 8),
				new Activity (Steps.Attitude, "List 5 successes you are most proud of", 8),
				new Activity (Steps.Attitude, "Define in writing desired attitude", 8),
				new Activity (Steps.Attitude, "Volunteer/ Help someone else (non-Profit)", 7),
				new Activity (Steps.Attitude, "Join a Toastmasters Club", 10, true),
				new Activity (Steps.Attitude, "Gain new Job Skills", 15),
			}
			);  // subSteps 3 .. 11   (10 is the TM task)

			// Step 2 - ASSESSMENTS
			Activities.Add (Steps.Assessments, new List<Activity> (16) {
				new Activity (Steps.Assessments, "Read Documents", 20, true),
				new Activity (Steps.Assessments, "Document 15 best accomplishments with STAR stories", 5),
				new Activity (Steps.Assessments, "Visit Workforce Center", 5),
				new Activity (Steps.Assessments, "List findings from the Interest assessment", 5),
				new Activity (Steps.Assessments, "List findings from the Aptitude assessment", 7),
				new Activity (Steps.Assessments, "List findings from the Emotional Intelligence assessment", 7),
				new Activity (Steps.Assessments, "List findings from your \"Strengths Finder\" assessment", 10),
				new Activity (Steps.Assessments, "Define value you will bring to employer", 10),
				new Activity (Steps.Assessments, "Assess test results in \"My Job Match\"", 5),
			}
			);

			// Step 3 - MARKETING STRATEGY
			Activities.Add (Steps.MarketingStrategy, new List<Activity> (16) {
				new Activity (Steps.MarketingStrategy, "Read Documents", 8, true),
				new Activity (Steps.MarketingStrategy, "Define and document marketing strategy", 8),
				new Activity (Steps.MarketingStrategy, "Try 3 job titles in INDEED.COM", 8),
				new Activity (Steps.MarketingStrategy, "List 5 networking candidates from Yahoo list", 8),
				new Activity (Steps.MarketingStrategy, "Define primary  networking objective", 8),
				new Activity (Steps.MarketingStrategy, "Prepare Networking Handout for first contact", 10),
				new Activity (Steps.MarketingStrategy, "Complete Network Handout form", 10),
				new Activity (Steps.MarketingStrategy, "Identify appropriate search firms", 5),
				new Activity (Steps.MarketingStrategy, "Create and post a Substantive LinkedIn Profile", 8),
				new Activity (Steps.MarketingStrategy, "Hold face-to-face network meetings", 20),
				new Activity (Steps.MarketingStrategy, "Practice \"elevator speech\"", 2),
				new Activity (Steps.MarketingStrategy, "Review JTSG Yahoo postings every day", 5),
				new Activity (Steps.MarketingStrategy, "Visit J.J. Hill library to research target company", 10),
			}
			);

			// Step 4 - MARKETING MATERIALS
			Activities.Add (Steps.MarketingMaterials, new List<Activity> (16) {
				new Activity (Steps.MarketingMaterials, "Read Documents", 8, true),
				new Activity (Steps.MarketingMaterials, "Define and document your two part Job Objective",	10),
				new Activity (Steps.MarketingMaterials, "Determine how you will prove performance",	8),
				new Activity (Steps.MarketingMaterials, "Develop Marketing Letter",	10),
				new Activity (Steps.MarketingMaterials, "Develop Cover Letter",	8),
				new Activity (Steps.MarketingMaterials, "Develop Resume", 20),
				new Activity (Steps.MarketingMaterials, "Have Yellow Tagger review Resume",	5),
				new Activity (Steps.MarketingMaterials, "Get Business Cards",	8, true),
				new Activity (Steps.MarketingMaterials, "Purchase Thank You notes",	5, true),
				new Activity (Steps.MarketingMaterials, "Review e-folio concept",	3),
			}
			);

			// Step 5 - INTERVIEWING
			Activities.Add (Steps.Interviewing, new List<Activity> (32) {
				new Activity (Steps.MarketingStrategy, "Read Documents", 8, true),
				new Activity (Steps.Interviewing, "Read the \"64 Toughest Questions\"",	6),
				new Activity (Steps.Interviewing, "Document responses to \"64 Toughest Questions\" using STAR stories",	30),
				new Activity (Steps.Interviewing, "Getting Interviews - Document Strategy",	10),
				new Activity (Steps.Interviewing, "Getting Interviews - List target employers",	8),
				new Activity (Steps.Interviewing, "List 5 questions you would ask if you were the interviewer",	10),
				new Activity (Steps.Interviewing, "Read Interviewing Documents",	8),
				new Activity (Steps.Interviewing, "Develop Written Interview strategy",	10),
				new Activity (Steps.Interviewing, "Define clearly what you want the interviewer to know about you", 10),
				new Activity (Steps.Interviewing, "Develop answers to \"Behavioral Interview\" questions using STAR", 20),
				new Activity (Steps.Interviewing, "Research prospective employers", 10),
				new Activity (Steps.Interviewing, "Research prospective industries",	10),
				new Activity (Steps.Interviewing, "Research hiring manager",	10),
				new Activity (Steps.Interviewing, "Complete Interview Preparation Checklist",	15),
				new Activity (Steps.Interviewing, "Develop written 2nd interview strategy",	10),
				new Activity (Steps.Interviewing, "Practice the \"64 Toughest\" questions and answers",	10),
				new Activity (Steps.Interviewing, "Review Interviewing Basics with a yellow tag", 15),
				new Activity (Steps.Interviewing, "Review Interviewing strategy with a yellow tag", 15),
				new Activity (Steps.Interviewing, "Ask \"Rocking chair question\"", 15),
			}
			);

			// Step 6 - FOLLOW-UP
			Activities.Add (Steps.FollowUp, new List<Activity> (8) {
				new Activity (Steps.FollowUp, "Prepare draft of thank you notes", 8),
				new Activity (Steps.FollowUp, "Define to who you will send thank you notes", 8),
				new Activity (Steps.FollowUp, "Send / Deliver thank you notes within 30 minutes after interview", 15),
				new Activity (Steps.FollowUp, "Send follow-up technical job article to hiring manager", 5),
				new Activity (Steps.FollowUp, "Thank you note for \"Rejection Letter\"", 20),
			}
			);

			// Step 7 - Celebrate
			Activities.Add(Steps.Celebrate, new List<Activity> (2) {
				new Activity (Steps.Celebrate, "Bring treats to your Job Support Group", 50),
				new Activity (Steps.Celebrate, "Send thank-you notes", 50),
				new Activity (Steps.Celebrate, "Build and Maintain your network", 50),
			}
			);

		}
	}
}
