using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotMain : MonoBehaviour
{
	//this is not main! use this for testing garbage, never actually use
	Conversation c;

	void Start ()
	{
		c = new Conversation();

		/*
		Person p1 = new Person("waldo", Enums.gender.Male, new Vector2Int(0, 0), null);
		Person p2 = new Person("odlaw", Enums.gender.Nonbinary, new Vector2Int(0, 0), null);

		Line l1 = c.speak(p1, p2, p2, .7f, -1);
		print(l1.getLineString());
		Line l2 = c.speak(p2, p1, l1, l1.aggregateLine() - 1.3f, -1);
		print(l2.getLineString());
		Line l3 = c.speak(p1, p2, l2, l2.aggregateLine(), -1);
		print(l3.getLineString());
		Line l4 = c.speak(p2, p1, l3, l3.aggregateLine(), -1);
		print(l4.getLineString());
		*/

		/*Place r = new Place(new Rectangle(new Vector2Int(0, 0), 3, 3, 0), "Kitchen");


		List<Thing> lt = new List<Thing>();
		lt.Add(new Thing("Dog", new Vector2Int(0, 2), 0, 0, 0, 0, 0, null, null, null));
		r.AddThings(new Vector2Int(0, 2), lt);

		Dictionary<string, string> bifeat = new Dictionary<string, string>();
		bifeat.Add("hair", "blue");
		bifeat.Add("eyes", "red");
		bifeat.Add("personality", "nice");
		bifeat.Add("gender", "Male");
		bifeat.Add("voice", "normal");
		Dictionary<KeyValuePair<string, string>, float> bilikes = new Dictionary<KeyValuePair<string, string>, float>();
		bilikes.Add(new KeyValuePair<string, string>("hair", "blue"), .9f);
		bilikes.Add(new KeyValuePair<string, string>("hair", "black"), .4f);
		bilikes.Add(new KeyValuePair<string, string>("personality", "banana"), -1f);
		bilikes.Add(new KeyValuePair<string, string>("hair", "banana"), -1f);
		bilikes.Add(new KeyValuePair<string, string>("eyes", "banana"), -1f);

		Person billiam = new Person("Billiam", Enums.gender.male, new Vector2Int(0, 0), r, bilikes, bifeat);
		billiam.setChill(.1f);

		Dictionary<string, string> jafeat = new Dictionary<string, string>();
		jafeat.Add("hair", "black");
		jafeat.Add("eyes", "black");
		jafeat.Add("personality", "negative");
		jafeat.Add("gender", "Female");
		jafeat.Add("voice", "loud");
		Dictionary<KeyValuePair<string, string>, float> jalikes = new Dictionary<KeyValuePair<string, string>, float>();
		jalikes.Add(new KeyValuePair<string, string>("hair", "blue"), .3f);
		jalikes.Add(new KeyValuePair<string, string>("hair", "black"), -1f);
		jalikes.Add(new KeyValuePair<string, string>("gender", "Nonbinary"), -.3f);
		jalikes.Add(new KeyValuePair<string, string>("hair", "banana"), -.3f);
		jalikes.Add(new KeyValuePair<string, string>("eyes", "red"), -.3f);

		Person janine = new Person("Janine", Enums.gender.female, new Vector2Int(0, 0), r, jalikes, jafeat);
		janine.setChill(.9f);

		Dictionary<string, string> bafeat = new Dictionary<string, string>();
		bafeat.Add("hair", "banana");
		bafeat.Add("eyes", "banana");
		bafeat.Add("personality", "banana");
		bafeat.Add("gender", "Nonbinary");
		bafeat.Add("voice", "banana");
		Dictionary<KeyValuePair<string, string>, float> balikes = new Dictionary<KeyValuePair<string, string>, float>();
		balikes.Add(new KeyValuePair<string, string>("hair", "blue"), 1f);
		balikes.Add(new KeyValuePair<string, string>("hair", "black"), 1f);
		balikes.Add(new KeyValuePair<string, string>("gender", "Male"), 1f);
		balikes.Add(new KeyValuePair<string, string>("personality", "nice"), 1f);
		balikes.Add(new KeyValuePair<string, string>("eyes", "black"), -1f);

		Person bananaMan = new Person("The Banana Man", Enums.gender.nonbinary, new Vector2Int(0, 0), r, balikes, bafeat);
		bananaMan.setChill(.5f);

		List<Person> lp = new List<Person>();
		lp.Add(billiam);
		lp.Add(janine);
		lp.Add(bananaMan);
		r.AddPeople(new Vector2Int(0, 0), lp);
		r.PeopleUpdate();

		Line l1 = c.speak(billiam, janine, bananaMan, -.9f, -1);
		print("Billiam to Janine: " + l1.getLineString());
		Line l2 = c.speak(janine, billiam, l1, l1.aggregateLine(), -1);
		print("Janine to Billiam: " + l2.getLineString());
		Line l3 = c.speak(janine, billiam, billiam, -.1f, -1);
		print("Janine to Billiam: " + l3.getLineString());
		Line l4 = c.speak(billiam, janine, janine, l3.aggregateLine(), -1);
		print("Billiam to Janine: " + l4.getLineString());
		Line l5 = c.speak(bananaMan, billiam, billiam, .9f, -1);
		print("The Banana Man to Billiam: " + l5.getLineString());
		Line l6 = c.speak(billiam, bananaMan, bananaMan, Mathf.Clamp(-.9f + l5.aggregateLine(), -1, 1), -1);
		print("Billiam to The Banana Man: " + l6.getLineString());
		Line l7 = c.speak(bananaMan, billiam, l6, l6.aggregateLine(), -1);
		print("The Banana Man to Billiam: " + l7.getLineString());*/

		List<Person> lp = FileManager.initPersons(null);
		print(lp[0].GetFeatureFloatValue("weight"));
		print(lp[0].GetFeatureStringValue("hair"));
	}
	
	void Update ()
	{
		
	}
}
