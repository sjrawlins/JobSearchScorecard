﻿using Android.Speech.Tts;
using Xamarin.Forms;
using System.Collections.Generic;
using Java.Lang;
using JobSearchScorecard;

[assembly: Dependency (typeof(TextToSpeech_Android))]

namespace JobSearchScorecard
{
	public class TextToSpeech_Android : Object, ITextToSpeech, TextToSpeech.IOnInitListener
	{
		TextToSpeech speaker;
		string toSpeak;

		public TextToSpeech_Android ()
		{
		}

		public void Speak (string text)
		{
			var c = Forms.Context; 
			toSpeak = text;
			if (speaker == null) {
				speaker = new TextToSpeech (c, this);
			}
			var p = new Dictionary<string,string> ();
			speaker.Speak (toSpeak, QueueMode.Flush, p);

		}

		#region IOnInitListener implementation

		public void OnInit (OperationResult status)
		{
			if (status == OperationResult.Error) {
				System.Diagnostics.Debug.WriteLine ("IOnInit OperationResult.Error");
			} else if (status.Equals (OperationResult.Success)) {
				System.Diagnostics.Debug.WriteLine ("spoke");
				var p = new Dictionary<string,string> ();
				speaker.Speak (toSpeak, QueueMode.Flush, p);
			} else
				System.Diagnostics.Debug.WriteLine ("was quiet");
		}

		#endregion
	}
}