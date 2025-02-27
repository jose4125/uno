﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SamplesApp.UITests.TestFramework;
using Uno.UITest.Helpers;
using Uno.UITest.Helpers.Queries;

namespace SamplesApp.UITests.Windows_UI_Xaml_Input
{
	public class EventSequence_Tests : SampleControlUITestBase
	{
		[Test]
		public void TestTap()
			=> RunSequence("Tap");

		[Test]
		public void TestClick()
			=> RunSequence("Click");

		[Test]
		public void TestHyperlink()
			=> RunSequence("Hyperlink", TapSomewhereInElement);

		[Test]
		public void TestListView()
			=> RunSequence("ListView", TapSomewhereInElement);

		private readonly Action<QueryEx> TapElement = element => element.Tap();
		private void TapSomewhereInElement(QueryEx element)
		{
			var rect = _app.WaitForElement(element).Single().Rect;
			_app.TapCoordinates(rect.X + 10, rect.Y + 10);
		}

		private void RunSequence(string testName, Action<QueryEx> tap = null)
		{
			tap = tap ?? TapElement;

			Run("UITests.Shared.Windows_UI_Input.PointersTests.EventsSequences");

			var target = _app.Marked($"Test{testName}Target");
			var reset = _app.Marked($"Test{testName}Reset");
			var validate = _app.Marked($"Test{testName}Validate");
			var result = _app.Marked($"Test{testName}Result");

			_app.WaitForElement(target);
			reset.Tap();
			tap(target);
			validate.Tap();

			TakeScreenshot("Result");

			_app.WaitForDependencyPropertyValue(result, "Text", "SUCCESS");
		}
	}
}
