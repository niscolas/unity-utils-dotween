using System;

namespace Plugins.ScriptableTween.Utilities
{
	public static class FloatExtensions
	{
		public static float Randomize(this float number, float randomizationRate)
		{
			return RandomizeUsing(number, randomizationRate, new Random());
		}
		
		public static float RandomizeUsing(this float number, float randomizationRate, Random random)
		{
			float baseRandom = number * randomizationRate;
			float result = random.NextFloat(number - baseRandom, number + baseRandom);
			return result;
		}
	}
}