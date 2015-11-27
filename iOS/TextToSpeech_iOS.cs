using System;
using AVFoundation;
using Xamarin.Forms;
using JobSearchScorecard;

[assembly: Dependency (typeof (TextToSpeech_iOS))]

namespace JobSearchScorecard
{
	public class TextToSpeech_iOS : ITextToSpeech
	{
		public TextToSpeech_iOS ()
		{
		}

		public void Speak (string text)
		{
			if (String.IsNullOrEmpty (text))
				return;
			var speechSynthesizer = new AVSpeechSynthesizer ();

			var speechUtterance = new AVSpeechUtterance (text) {
				Rate = AVSpeechUtterance.MaximumSpeechRate/4,
				Voice = AVSpeechSynthesisVoice.FromLanguage ("en-US"),
				Volume = 0.5f,
				PitchMultiplier = 1.0f
			};

			speechSynthesizer.SpeakUtterance (speechUtterance);
		}
	}
}


