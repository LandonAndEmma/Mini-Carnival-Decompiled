using System.Text.RegularExpressions;

namespace MiscToolKits
{
	public static class MiscStaticTools
	{
		public static bool SF_IsPureNumber(string tar)
		{
			Regex regex = new Regex("[^0-9]");
			Match match = regex.Match(tar);
			return !match.Success;
		}
	}
}
