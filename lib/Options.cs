using System;
using System.ComponentModel;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace ssl_checker
{
	public sealed class Options : CommandLineOptionsBase
	{
		#region Standard Option Attribute
//        [Option("v", null, HelpText = "Print details during execution.")]
//        public bool Verbose { get; set; }

		#endregion

        #region Specialized Option Attribute
		//Each value not captured by an option can be included in a collection of strings
		[ValueList(typeof(List<string>), MaximumElements = 10)]
		public IList<string> Urls { get; set; }

        #endregion

        #region Help Screen
		[HelpOption]
		public string GetUsage ()
		{
			return HelpText.AutoBuild (this, (HelpText current) => HelpText.DefaultParsingErrorsHandler (this, current));
		}
        #endregion
	}
}

