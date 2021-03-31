using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.ScriptableTween.Utilities
{
	public static class EnumerableExtensions
	{
		internal static T RandomElement<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.RandomElementUsing(new Random());
		}

		internal static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
		{
			int index = rand.Next(0, enumerable.Count());
			return enumerable.ElementAt(index);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
		{
			return enumerable == null || !enumerable.Any();
		}
	}
}