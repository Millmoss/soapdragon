using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
	

	void Start ()
	{
		//initPersons();
		initLines();
	}
	
	void Update ()
	{
		
	}

	public void initLines()
	{
		string data = "";

		System.IO.StreamReader inFile = new System.IO.StreamReader(@"Assets\GameData\DialogueData\DialogueLines.json");
		while (!inFile.EndOfStream)
		{
			data += inFile.ReadLine();
		}

		LineLibrary l = LineLibrary.CreateFromJSON(data);
		
		print(l.opinionDirected[0]);
	}

	public List<Person> initPersons()
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

	public List<Person> loadPersons()
	{
		List<Person> people = new List<Person>();

		return people;
	}
}
