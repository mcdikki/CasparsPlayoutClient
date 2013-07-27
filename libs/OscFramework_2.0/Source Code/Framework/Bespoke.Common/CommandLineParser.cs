using System;
using System.Text;
using System.Collections.Generic;

namespace Bespoke.Common
{
	/// <summary>
	/// Command-line parsing class.
	/// </summary>
    /// <remarks>Looks for command-line arguments in the format: name1=value1 name2=value2 ...</remarks>
	public class CommandLineParser
	{
		/// <summary>
		/// Gets a argument associated with the specified key.
		/// </summary>
		/// <param name="key">The name of the argument to retrieve.</param>
        /// <returns> The value associated with the specified key. If the specified key is not
        /// found, a <see cref="System.Collections.Generic.KeyNotFoundException"/> is thrown.
        /// </returns>
		public string this[string key]
		{
			get
			{
				return mArguments[key.ToUpper()];
			}
		}

		/// <summary>
        /// Gets the number of arguments present on the command-line.
		/// </summary>
        /// <remarks>Does not include the name of the invoked application.</remarks>
		public int Count
		{
			get
			{
				return mArguments.Count;
			}
		}

		/// <summary>
		/// Gets the list of argument names.
		/// </summary>
		public string[] Keys
		{
			get
			{
				string[] keys = new string[mArguments.Keys.Count];
				mArguments.Keys.CopyTo(keys, 0);
				return keys;
			}
		}

		/// <summary>
        /// Gets the list of argument values.
		/// </summary>
		public string[] Values
		{
			get
			{
				string[] values = new string[mArguments.Values.Count];
				mArguments.Values.CopyTo(values, 0);
				return values;
			}
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
		/// </summary>
		/// <param name="args">The arguments provided from the command-line (typically passed into Main).</param>
		public CommandLineParser(string[] args)
		{
            mArguments = new SortedDictionary<string, string>();

            string collapsedArguments = CommandLineParser.CollapseArguments(args);
            if (collapsedArguments.IndexOf(SpecialValueSeparator) != -1)
            {
                ParseSpecialCommandLine(collapsedArguments);
            }
            else
            {
                ParseWindowsCommandLine(args);
            }
		}

        /// <summary>
        /// Collapse the set of arguments into a single, space-delimted string.
        /// </summary>
        /// <param name="args">The set of command-line arguments.</param>
        /// <returns>A single, space-delimted string representation of the arguments.</returns>
        public static string CollapseArguments(string[] args)
        {
            StringBuilder collapseArguments = new StringBuilder();

            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    collapseArguments.Append(arg);
                    collapseArguments.Append(SpaceCharacter);
                }

                // Trim last space
                collapseArguments.Remove(collapseArguments.Length - 1, 1);
            }

            return collapseArguments.ToString();
        }

		/// <summary>
		/// Determine if the specified argument exists.
		/// </summary>
		/// <param name="key">The name of the argument to find.</param>
		/// <returns>true if the argument exists; otherwise, false.</returns>
		public bool ContainsKey(string key)
		{
			return mArguments.ContainsKey(key.ToUpper());
        }

        #region Private Methods

        /// <summary>
        /// Parse the standard windows command-line.
        /// </summary>
        /// <param name="args"></param>
        private void ParseWindowsCommandLine(string[] args)
        {
            string name = String.Empty;
            string value = String.Empty;

            int delimiterIndex = 0;
            foreach (string arg in args)
            {
                // Find a value delimiter in the current name=value pair
                delimiterIndex = arg.IndexOf(ValueDelimiter);
                if (delimiterIndex == -1)
                {
                    name = arg;
                    value = String.Empty;
                }
                else
                {
                    // Parse out the name and value from the pair
                    name = arg.Substring(0, delimiterIndex).Trim().ToUpper();
                    value = arg.Substring(delimiterIndex + 1).Trim().ToUpper();
                }

                mArguments.Add(name, value);
            }
        }

        /// <summary>
        /// Parse a command-line containing (`) as the value delimiter.
        /// </summary>
        /// <param name="collapsedArgs">The collapsed arguments string.</param>
        private void ParseSpecialCommandLine(string collapsedArgs)
        {
            // Reconstruct a Windows-style command-line
            List<string> args = new List<string>();
            
            string arg = string.Empty;
            bool inQuotedArgument = false;
            foreach (char c in collapsedArgs)
            {
                if (c == SpecialValueSeparator)
                {
                    if (inQuotedArgument)
                    {
                        // End quoted argument
                        if (arg != String.Empty)
                        {
                            args.Add(arg);
                            arg = String.Empty;
                        }
                        inQuotedArgument = false;
                    }
                    else
                    {
                        // Begin quoted argument
                        inQuotedArgument = true;
                    }
                }
                else if ((c == SpaceCharacter) && (inQuotedArgument == false))
                {
                    if (arg != String.Empty)
                    {
                        args.Add(arg);
                        arg = String.Empty;
                    }
                }
                else
                {
                    arg += c;
                }
            }

            if (arg != String.Empty)
            {
                args.Add(arg);
            }

            ParseWindowsCommandLine(args.ToArray());
        }

        #endregion

        private static readonly char SpaceCharacter = ' ';
        private static readonly char SpecialValueSeparator = '`';
		private static readonly string ValueDelimiter = "=";

		private static SortedDictionary<string, string> mArguments;
	}
}
