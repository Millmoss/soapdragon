using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
	

	void Start ()
	{
		initPersons();
	}
	
	void Update ()
	{
		
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

			List<System.Object> personInfo = (List<System.Object>) deserializeBlade(data);
		}

		return people;
	}

	public List<Person> loadPersons()
	{
		List<Person> people = new List<Person>();

		return people;
	}
}
