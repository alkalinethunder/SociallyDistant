﻿#nullable enable
namespace UI.Shell
{
	public interface ICommandShell
	{
		string GetVariableValue(string name);
		void SetVariableValue(string name, string newValue);
		
		string CurrentWorkingDirectory { get; }
		string CurrentHomeDirectory { get; }
	}
}