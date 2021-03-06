﻿using System;
using LoadBalancerKata;
using NHamcrest;
using NHamcrest.Core;

namespace LoadBalancerKataTests.Matchers
{
	public class CurrentLoadPercentageMatcher : Matcher<Server>
	{
		private const double Epsilon = 0.001d;
		private const double Full = 100.0;
		private readonly double _expectedLoadPercentage;
		private readonly bool _fullMatch;

		private CurrentLoadPercentageMatcher(double expectedLoadPercentage) =>
			_expectedLoadPercentage = expectedLoadPercentage;

		private CurrentLoadPercentageMatcher(bool full) => _fullMatch = full;

		public override void DescribeTo(IDescription description) =>
			description.AppendText("This server has a load percentage that equals: ")
			   .AppendValue(_expectedLoadPercentage);

		public override void DescribeMismatch(Server item, IDescription mismatchDescription) =>
			mismatchDescription.AppendText("This server has a load percentage that equals: ")
			   .AppendValue(item.CurrentLoadPercentage);

		public override bool Matches(Server server) =>
			_fullMatch
				? CompareDoubles(server.CurrentLoadPercentage, Full)
				: CompareDoubles(server.CurrentLoadPercentage, _expectedLoadPercentage);

		private static bool CompareDoubles(double first, double second) => Math.Abs(first - second) < Epsilon;

		public static Matcher<Server> HasLoadPercentageOf(double value) => new CurrentLoadPercentageMatcher(value);

		public static IMatcher<Server> StaysFull() => new CurrentLoadPercentageMatcher(true);
	}
}