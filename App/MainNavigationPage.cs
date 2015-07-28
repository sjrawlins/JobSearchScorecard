using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace JobScorecard
{
	public class StartPage : ContentPage
	{
		public Dictionary<int, Activity> Activities;
		public List<Task> currentTasks;

		public StartPage ()
		{
			currentTasks = MockUpTasks ();
			InitActivities ();

			// test array bounds in C#
			float[] c = new float[1:10];
			for (int i = 0;i<=10;i++)
			{
				c [i] = (float)i * 10.5f + 0.1f;
			}

			Padding = new Thickness (20);
			var listView = new ListView {
				RowHeight = 40
			};
			listView.ItemsSource = new string [] {
				"ATTITUDE",
				"ASSESSMENTS",
				"MARKETING STRATEGY",
				"MARKETING MATERIALS",
				"INTERVIEWING",
				"FOLLOW-UP",
			};
			listView.ItemSelected += async (sender, e) => {
				//System.Diagnostics.Debug.WriteLine("Tapped!", e.SelectedItem + " was tapped.");
				await DisplayAlert ("Tapped!", e.SelectedItem + " was tapped.", "OK");
			};

			var scoreLabel = new Label () {
				//TextColor = Color.Green,
				FontAttributes = FontAttributes.Bold,
				FontSize = 25,
				Text = Convert.ToString (CalculateScore (currentTasks)),
				XAlign = TextAlignment.Center,
			};

			DateTime dateRangeFrom = new DateTime (2015, 6, 10);
			DateTime dateRangeTo = DateTime.Now;
			var dateRange = "period from: " + dateRangeFrom.ToString () + " to " + dateRangeTo.ToString ();
			var dateRangeLabel = new Label () {
				FontSize = 20,
				Text = dateRange,
				XAlign = TextAlignment.Center,
			};
				
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.StartAndExpand,
				Children = {

					new Label {
						XAlign = TextAlignment.Center,
						Text = "Job Search Scorecard",
						//TextColor = Color.Blue,
						FontSize = 22,
					},
					listView,
					dateRangeLabel,
					scoreLabel,
				}
			};
		}

		public List<Task> MockUpTasks ()
		{
			var fakeList = new List<Task> () {
				new Task (1000, 1, DateTime.Now, "some notes"),
				new Task (1001, 2, DateTime.Now.AddMinutes (-5), "other notes"),
				new Task (1002, 3, DateTime.Now.AddDays (-2), "stillmore"),
				new Task (1002, 200, DateTime.Now.AddDays (-2), "stillmore"),
				new Task (1002, 200, DateTime.Now.AddDays (-2), "stillmore"),
			};
			return fakeList;
		}

		public int CalculateScore (List<Task> tasksAccomplished)
		{
			int tempScore = 0;
			foreach (var item in tasksAccomplished) {
				tempScore += Activities [item.Code].Score;
			}
			;
			return tempScore;		
		}

		private void InitActivities ()
		{
			int n = -1;

			// Initialize the Activities and corresponding scores
			// The look-up key is unique and so Dictionary will bark (runtime failure) if any duplicates - so watch out!

			// Some activities are Once Only (typically "Read Documents")

			Activities = new Dictionary<int, Activity> ();
			Activities.Add (1, new Activity (Steps.All, "Prayer", 10));
			Activities.Add (2, new Activity (Steps.All, "Read Scripture", 10));
			Activities.Add (3, new Activity (Steps.All, "Share daily plans with family", 4));

			// STEP 1 - ATTITUDE
			n = (int)Steps.Attitude * 100;
			Activities.Add (n++, new Activity (Steps.Attitude, "Read Documents and list 3 most important items", 5, true));
			Activities.Add (n++, new Activity (Steps.Attitude, "List negative attitudes", 10));
			Activities.Add (n++, new Activity (Steps.Attitude, "Take Attitude Quiz", 10));
			Activities.Add (n++, new Activity (Steps.Attitude, "List 2 negative attitudes that must change", 8));
			Activities.Add (n++, new Activity (Steps.Attitude, "List 5 successes you are most proud of", 8));
			Activities.Add (n++, new Activity (Steps.Attitude, "Define in writing desired attitude", 8));
			Activities.Add (n++, new Activity (Steps.Attitude, "Volunteer/ Help someone else (non-Profit)", 7));
			Activities.Add (n++, new Activity (Steps.Attitude, "Join a Toastmasters Club", 10));
			Activities.Add (n++, new Activity (Steps.Attitude, "Gain new Job Skills", 15));

			// Step 2 - ASSESSMENTS
			n = (int)Steps.Assessments * 100;
			Activities.Add (n++, new Activity (Steps.Assessments, "Read Documents", 20, true));
			Activities.Add (n++, new Activity (Steps.Assessments, "Document 15 best accomplishments with STAR stories", 5));
			Activities.Add (n++, new Activity (Steps.Assessments, "Visit Workforce Center", 5));
			Activities.Add (n++, new Activity (Steps.Assessments, "List findings from the Interest assessment", 5));
			Activities.Add (n++, new Activity (Steps.Assessments, "List findings from the Aptitude assessment", 7));
			Activities.Add (n++, new Activity (Steps.Assessments, "List findings from the Emotional Intelligence assessment", 7));
			Activities.Add (n++, new Activity (Steps.Assessments, "List findings from your \"Strengths Finder\" assessment", 10));
			Activities.Add (n++, new Activity (Steps.Assessments, "Define value you will bring to employer", 10));
			Activities.Add (n++, new Activity (Steps.Assessments, "Assess test results in \"My Job Match\"", 5));

			// Step 3 - MARKETING STRATEGY
			n = (int)Steps.MarketingStrategy * 100;
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Read Documents", 8, true));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Define and document marketing strategy", 8));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Try 3 job titles in INDEED.COM", 8));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "List 5 networking candidates from Yahoo list", 8));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Define primary  networking objective", 8));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Prepare Networking Handout for first contact", 10));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Complete Network Handout form", 10));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Identify appropriate search firms", 5));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Create and post a Substantive LinkedIn Profile", 8));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Hold face-to-face network meetings", 20));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Practice \"elevator speech\"", 2));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Review JTSG Yahoo postings every day", 5));
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Visit J.J. Hill library for target company information", 10));

			// Step 4 - MARKETING MATERIALS
			n = (int)Steps.MarketingMaterials * 100;
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Read Documents", 8, true));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Define and document your two part Job Objective",	10));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Determine how you will prove performance",	8));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Develop Marketing Letter",	10));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Develop Cover Letter",	8));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Develop Resume", 20));
			/*		Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Have Yellow Tagger review Resume",	5));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Get Business Cards",	8, true));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Purchase Thank You notes",	5, true));
			Activities.Add (n++, new Activity (Steps.MarketingMaterials, "Review e-folio concept",	3));
			*/

			// Step 5 - INTERVIEWING
			n = (int)Steps.Interviewing * 100;
			Activities.Add (n++, new Activity (Steps.MarketingStrategy, "Read Documents", 8, true));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Read the \"64 Toughest Questions\"",	6));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Document responses to \"64 Toughest Questions\" using STAR stories",	30));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Getting Interviews - Document Strategy",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Getting Interviews - List target employers",	8));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - List 5 questions you would ask if you were the interviewer",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Read Interviewing Documents",	8));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Develop Written Interview strategy",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Define clearly what you want the interviewer to know about you", 10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Develop answers to \"Behavioral Interview\" questions using STAR", 20));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Research prospective employers", 10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Research prospective industries",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Research hiring manager",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Complete Interview Preparation Checklist",	15));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Develop written 2nd interview strategy",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Preparing for the Interview - Practice the \"64 Toughest\" questions and answers",	10));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Interviewing - Review Interviewing Basics with a yellow tag", 15));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Interviewing - Review Interviewing strategy with a yellow tag", 15));
			Activities.Add (n++, new Activity (Steps.Interviewing, "Interviewing - Ask \"Rocking chair question\"", 15));

			// Step 6 - FOLLOW-UP
			n = 600;
			Activities.Add (n++, new Activity (Steps.FollowUp, "Prepare draft of thank you notes", 8));
			Activities.Add (n++, new Activity (Steps.FollowUp, "Define to who you will send thank you notes", 8));
			Activities.Add (n++, new Activity (Steps.FollowUp, "Send / Deliver thank you notes within 30 minutes after interview", 15));
			Activities.Add (n++, new Activity (Steps.FollowUp, "Send follow-up technical job article to hiring manager", 5));
			Activities.Add (n++, new Activity (Steps.FollowUp, "Thank you note for \"Rejection Letter\"", 20));

		}
	}
}

