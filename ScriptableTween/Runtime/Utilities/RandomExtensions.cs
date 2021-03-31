using System;

namespace Plugins.ScriptableTween.Utilities
{
	public static class RandomExtensions
	{
		internal static float NextFloat(this Random randomGen)
		{
			return (float) randomGen.NextDouble();
		}

		internal static float NextFloat(this Random randomGen, float min, float max)
		{
			return randomGen.NextFloat() * (max - min) + min;
		}
	}
}