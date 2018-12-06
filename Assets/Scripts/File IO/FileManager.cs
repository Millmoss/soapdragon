using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileManager
{
	public static LineLibrary initLines()
	{
		string data = "";

		System.IO.StreamReader inFile = new System.IO.StreamReader(@"Assets\GameData\DialogueData\DialogueLines.json");
		while (!inFile.EndOfStream)
		{
			data += inFile.ReadLine();
		}

		LineLibrary ll = LineLibrary.CreateFromJSON(data);
		return ll;
	}

	public static ExpressionLibrary initExpressions()
	{
		string data = "";

		System.IO.StreamReader inFile = new System.IO.StreamReader(@"Assets\GameData\DialogueData\DialogueExpressions.json");
		while (!inFile.EndOfStream)
		{
			data += inFile.ReadLine();
		}

		ExpressionLibrary el = ExpressionLibrary.CreateFromJSON(data);
		return el;
	}

	public static List<Person> initPersons()
	{
		List<Person> people = new List<Person>();

		string[] fileNames = System.IO.Directory.GetFiles(@"Assets\GameData\InitData\CharacterData");

		for (int fni = 0; fni < fileNames.Length; fni++)
		{
			if (fileNames[fni].Contains(".meta"))
				continue;

			List<string> data = new List<string>();

			System.IO.StreamReader inFile = new System.IO.StreamReader(fileNames[fni]);
			while (!inFile.EndOfStream)
			{
				data.Add(inFile.ReadLine());
			}
		}

		return people;
	}

	public static List<Person> loadPersons()
	{
		List<Person> people = new List<Person>();

		return people;
	}
}
