using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person {

    //Range for GetRandomNearbyItem
    public static int random_radius = 6; 

	public string name { get; private set; }
    public int eyesight { get; private set; }
    private Vector2Int position;
    private Enums.rotations rotation;
	public Memory memory { get; private set; }
	public Place current_place { get; private set; }
    public Conversation c;
    public bool in_conversation { get; private set; }

    Dictionary<string, float> needs = new Dictionary<string, float>()
    {
        {"hunger", 0 },
        {"social", 0 },
        {"thrist",0 },
        {"tiredness",0 },
        {"stress", 0f },
        {"libido", 0f }
    };

    Dictionary<string,float> features_float = new Dictionary<string, float>() {
        //emotions
        {"happiness", 0.5f },
        //mental
        {"wit",  0.5f },
        {"chill", 0.5f },
        {"introversion",0.5f },
        //physical
        {"muscle",0.5f },
        {"fat",0.5f },
        {"beauty",0.5f },
        //weight is in meteres, weight in pounds.
        {"height",3 },
        {"weight", 125 },   
    };

    Dictionary<string, string> features_string = new Dictionary<string, string>()
    {
        {"gender","male" },
        {"hair", "red" },
        {"eyes", "blue" }
    };
    //Feeling; -1 for hate, 1 for love.
    private List<Preference_Things> likes_things;
    private List<Preference_People> likes_people;

    //Pair values are Type -> Value -> how much is liked. Prob 2? One fo rstrings and one for floats 
    //Unless we can think of something not dumb.
    //EX: KeyValuePair<"gender","male">, 0.1
    private Dictionary<string, float> likes_traits;

	//general preference of a person, place, thing, feature, etc
	//get and set using structure of "person, name, feature", "person, hair, blue", "thing, apple, flavor%sweet"
	private Preferences preferences;
	private char[] splitPercent;
	private char[] splitColon;

	//These are for pathfinding / AI interaction stuff.
	private Thing wanted_object;
	private Vector2Int moveto_position;
	private given_action current_action;
    private List<Vector2Int> path_to_obj;

    private static Thing null_object = new Thing("NOTHING", Vector2Int.zero, 0, 0, 0, 0, 0, null, null, null);



    public Person(string _name, Enums.gender _gender, Vector2Int _pos, Place _current_room, 
        Dictionary<string, float> _likes_traits,
		Dictionary<string, string> feat_str)
    {
        name = _name;
        eyesight = 5;

        likes_people = new List<Preference_People>();

        position = _pos;

        memory = new Memory();
        current_place = _current_room;

        rotation = Enums.rotations.W;

        wanted_object = null;
        likes_traits = _likes_traits;
		features_string = feat_str;


		splitPercent = new char[1];
		splitPercent[0] = '%';
		splitColon = new char[1];
		splitColon[0] = ':';
	}

	public Person(PersonData pd, Place _current_room)
	{
		position = new Vector2Int(pd.xPosition, pd.yPosition);
		rotation = (Enums.rotations)pd.rotation;
		name = pd.name;

		eyesight = pd.eyesight;

		features_float = new Dictionary<string, float>();
		features_string = new Dictionary<string, string>();

		splitPercent = new char[1];
		splitPercent[0] = '%';
		splitColon = new char[1];
		splitColon[0] = ':';

		features_float["muscle"] = pd.muscle;
		features_float["fat"] = pd.fat;
		features_float["height"] = pd.height;
		features_float["weight"] = pd.weight;
		features_float["beauty"] = pd.beauty;
		features_float["wit"] = pd.wit;
		features_float["chill"] = pd.chill;
		features_float["introversion"] = pd.introversion;
		features_float["happiness"] = pd.happiness;
		features_float["sadness"] = pd.sadness;
		features_float["anger"] = pd.anger;
		features_float["fear"] = pd.fear;
		features_float["disgust"] = pd.disgust;
		needs["hunger"] = pd.hunger;
		needs["thirst"] = pd.thirst;
		needs["tiredness"] = pd.tiredness;
		needs["social"] = pd.social;
		needs["stress"] = pd.stress;
		needs["libido"] = pd.libido;

		for (int i = 0; i < pd.features.Length; i++)
		{
			string[] f = pd.features[i].Split(splitPercent);
			features_string[f[0]] = f[1];
		}

		preferences = new Preferences(pd.preferences);
        memory = new Memory();
        current_place = _current_room;
	}

	public Dictionary<string, string> GetFeaturesString()
	{
		return new Dictionary<string, string>(features_string);
	}

	public Dictionary<string, float> GetFeaturesFloat()
	{
		return new Dictionary<string, float>(features_float);
	}

	public void setChill(float c)
	{
		features_float["chill"] = c;
	}

    //Returns true if the person has the given trait.
    public bool HasFeature(string feature)
	{
		string[] f = feature.Split(splitPercent);
		if (f[0] == "person" && features_string.ContainsKey(f[1]))
        {
            if (features_string[f[1]] == f[2])
                return true;
        }
        return false; 
    }

    //Returns true if a person's feature has the given value.
    public bool HasFeature(string feature, float value)
    {
        if (features_float.ContainsKey(feature))
        {
            //Has a flex area of 0.1f.
            if (0.1f >= Mathf.Abs(features_float[feature]- value))
                return true;
        }
        return false;
    }

    //Return closest trait thisperson has feeling towards person prsn towards w float feeling.
    //Always returns a value. 
    public string GetMatchingFeature(Person prsn, float feeling)
    {
        float dif = float.MaxValue;
        string feature = "";

		feature = preferences.getClosestMatchingPerson(prsn, feeling);

        return feature;
    }

    //returns null if theres no items.
    public Thing GetRandomNearbyItem()
    {
        List<Thing> items = memory.GetThings(current_place.name);
        Thing ret = null;
        if (items.Count > 0)
            ret = items[UnityEngine.Random.Range(0, items.Count)];
        return ret;
    }

    //When this person is spoken to.
    public void AddLine(Person p, Line l)
    {
        memory.AddLine(p, l);
        memory.AddPerson(current_place.name, p.Position, p);
        in_conversation = true;
        //Manipulate preferences based on the given line.
    }

	//Each step this function is called; live "updates"
	public void Update(Dictionary<Vector2Int, List<Person>> ppl, Dictionary<Vector2Int, List<Thing>> thngs)
	{
		Debug.Log(rotation);

		Vector2Int checkPos = new Vector2Int();
		//Check everything in eyesight range.
		int comp = 1;
		if (rotation == Enums.rotations.S)
			comp *= -1;
		for (int y = 0; y < eyesight; y++)
		{
			if (rotation == Enums.rotations.N || rotation == Enums.rotations.S)
				checkPos.y = y + 1;
			else
				checkPos.x = y + 1;
			checkPos.y *= comp;
			for (int x = -y; x <= y; x++)
			{
				if (rotation == Enums.rotations.N || rotation == Enums.rotations.S)
					checkPos.x = x;
				else
					checkPos.y = x;
				//First we remove all memory of thigns @ the chcked positions.
				memory.RemoveAllThings(current_place.name, checkPos);
				//We do the same with the people as well.
				memory.RemoveAllPeople(current_place.name, checkPos);

				//Now we go through and readd everything to to the memory using the new knowledge we have.
				if (thngs.ContainsKey(checkPos))
				{
					foreach (Thing obj in thngs[checkPos])
					{
						memory.AddThing(current_place.name, checkPos, obj);
					}
				}
				if (ppl.ContainsKey(checkPos))
				{
					Debug.Log("WOW!");
					foreach (Person prsn in ppl[checkPos])
					{
						memory.AddPerson(current_place.name, checkPos, prsn);
					}
				}
			}
		}




		//Tick hunger / social stat.
		if (needs["hunger"] < 1)
		{
			needs["hunger"] += 0.1f;
		}

		if (needs["social"] < 1 && !in_conversation)
		{
			needs["social"] += (1.5f - features_float["introversion"]) * 0.0245f;
		}

		if (features_float["fear"] > .1f)
			features_float["fear"] -= .05f;

		needs["social"] = Mathf.Clamp01(needs["social"]);
    }

    //Perform actions based on memory.
    public string Action()
    {
        string ret = "";
        ret += PerformAction();
        string ps = PerformSpeaking();
        if (ps != "")
        {
            ret += "\n";
            ret += ps;
        }
        return ret;

    }

    //Returns the value of the given need.
    public float GetNeedValue(string x)
    {
        return needs[x];
    }

    //Returns the value of the given need.
    public float GetFeatureFloatValue(string x)
    {
        return features_float[x];
    }

    //Returns the value of the given need.
    public string GetFeatureStringValue(string x)
    {
        return features_string[x];
    }

    private string PerformSpeaking()
    {
        string ret = "";
		bool exitConv = false;

        //He wants to talk.
        if ((needs["social"] > 0.45f || in_conversation) && features_float["fear"] < .3f)
        {
			float lineFeeling = 0;
            Person x = memory.GetRandomPerson(current_place);
			if (x == null || (x.in_conversation == true && !in_conversation))	//HERE
			{
				return "There is no one around to talk to for " + name + ".";
			}
			in_conversation = true;
			x.in_conversation = true;
            Line l;
            float personFeeling = 0;
			float randomMod = UnityEngine.Random.value;
			if (preferences.has("person"))
				personFeeling = preferences.get("person", x.name) - features_float["anger"] / 5;
			else
			{
				preferences.set("person", x.name, "character", 0);
				personFeeling = preferences.get("person", x.name) - features_float["anger"] / 5;
			}
			personFeeling = Mathf.Clamp(personFeeling, -1, 1);
			Line lastLine = memory.GetLine(x);
			if (lastLine == null)	//starts new conversation
			{
				if (personFeeling > -.5f)
				{
					l = c.speak(this, x, "greeting", personFeeling, -1);
				}
				else
				{
					l = c.speak(this, x, x, personFeeling, -1);
				}
			}
			else	//continues current conversation
			{
				float agg = lastLine.aggregateLine();
				lineFeeling = -.05f * (features_float["introversion"] + 1);

				string response = memory.DetermineAppropriateLine();

				//determining feeling toward last line
				if (lastLine.about != null && lastLine.about.expression == name && Mathf.Abs(agg) > .3f)
				{
					if (agg > 0)
					{
						lineFeeling = -.01f * (features_float["introversion"] + 1);
						preferences.mod("person", x.name, "character", agg / 100);

						if (preferences.get("person", "gender", x.GetFeaturesString()["gender"]) > .5f)
							needs["libido"] += agg / 10;
					}
					else
					{
						lineFeeling = -.07f * (features_float["introversion"] + 1);
						preferences.mod("person", x.name, "character", (agg / 50) * (1 - features_float["chill"]));
						needs["stress"] -= agg / 20;
						features_float["fear"] -= agg / 10;
					}

					features_float["sadness"] -= agg / 10;
					features_float["happiness"] += agg / 10;
					features_float["anger"] -= agg / 5;
					features_float["disgust"] -= agg / 15;
				}
				else if (lastLine.about != null)
				{
					float f = .25f - Mathf.Abs(preferences.get(lastLine.type.ToString(), lastLine.about.expression) - agg);
					preferences.mod("person", x.name, "character", (f / 40) * (1 - features_float["chill"]));

					features_float["anger"] -= f / 5;
					features_float["disgust"] -= f / 5;
					features_float["happiness"] += f / 10;
				}

				//responding to last line
				if (lastLine.type == Enums.lineTypes.threatDirected)	//last line was a threat
				{
					lineFeeling = -.2f * (features_float["introversion"] + 2);
					features_float["fear"] += .3f;
					if (features_float["fear"] > .5f)
						exitConv = true;
					l = c.speak(this, x, x, personFeeling, -1);
				}
				else if (personFeeling < -.5f || agg < -.7f)			//last line was very negative or feeling toward person is very negative
				{
					lineFeeling = -.05f * (features_float["introversion"] + 1);
					l = c.speak(this, x, x, personFeeling, -1);
					preferences.mod("person", x.name, "character", -(features_float["anger"] + needs["stress"] + features_float["disgust"]) / 80);
				}
				else if (response == "greeting")						//last line was a greeting
				{
					l = c.speak(this, x, "greeting", personFeeling, -1);
					float sm = needs["social"] - .45f;
					preferences.mod("person", x.name, "character", sm / 10 + features_float["chill"] / 40 - (features_float["anger"] + needs["stress"] + features_float["disgust"]) / 75);
				}
				else
				{
					if (Mathf.Abs(personFeeling) <= .1f)				//feeling toward person is very neutral
					{
						string closest = preferences.getClosestMatchingAny(randomMod, x.name);

						string[] fs = closest.Split(splitPercent);
						l = c.speak(this, x, closest, preferences.get(fs[0], fs[1], fs[2]), -1);
					}
					else if (personFeeling > .3f)						//feeling toward person is reasonably positive
					{
						if (UnityEngine.Random.value > .7f)
							l = c.speak(this, x, x, personFeeling, -1);
						else
						{
							string closest = preferences.getClosestMatchingAny(randomMod * 2 - 1, x.name);
							string[] fs = closest.Split(splitPercent);
							l = c.speak(this, x, closest, preferences.get(fs[0], fs[1], fs[2]), -1);
						}
					}
					else												//feeling toward person is either barely positive or is negative
					{
						lineFeeling = -.05f * (features_float["introversion"] + 1);
						if (UnityEngine.Random.value > .5f)
							l = c.speak(this, x, x, personFeeling, -1);
						else
						{
							string closest = preferences.getClosestMatchingAny(randomMod - 1, x.name);
							string[] fs = closest.Split(splitPercent);
							l = c.speak(this, x, closest, preferences.get(fs[0], fs[1], fs[2]), -1);
						}
					}
				}
			}
			
			if (needs["social"] < .2f && x.needs["social"] < .3f)
			{
				exitConv = true;
			}

			if (exitConv)
			{
				in_conversation = false;
				x.in_conversation = false;
				memory.WipeLines();
				x.memory.WipeLines();
				if (features_float["fear"] > .5f)
					return name + " is too spooked to talk right now.";
				else
					return name + " is finished talking.";
			}

			memory.AddLine(this, l);
            x.AddLine(this,l);
			ret += name + " speaks to " + x.name + " : ";
            ret += l.getLineString();
			needs["social"] += lineFeeling;

			features_float["anger"] = Mathf.Clamp01(features_float["anger"]);
			features_float["sadness"] = Mathf.Clamp01(features_float["sadness"]);
			features_float["happiness"] = Mathf.Clamp01(features_float["happiness"]);
			features_float["disgust"] = Mathf.Clamp01(features_float["disgust"]);
			features_float["fear"] = Mathf.Clamp01(features_float["fear"]);
        }

        return ret;
    }

    private string PerformAction()
    {
        given_action next_action = Blackboard.GetNextMove(this);
        if (current_action.action != next_action.action)
        {
            current_action = next_action;

            return name + " realized he needed to satisfy " + current_action.action + " with a value of " + next_action.value;
        }
        else
        {
            current_action.value = next_action.value;
        }
        //Basic movement algorithm
        if (wanted_object == null)
        {
            bool found = false;
            //If we remmeber there is a item @ position, we set that as the item we want to get to to interact with.
            if(memory.remember_items.ContainsKey(current_place.name))
            {
                foreach (memory_thing item in memory.remember_items[current_place.name])
                {
                    if (Enums.IsInDictionary(item.thing.uses, current_action.action))
                    {
                        found = true;
                        wanted_object = item.thing;
                        moveto_position = item.pos_at_place;
                        path_to_obj = Pathfinding.GetPath(position, moveto_position,
                            new List<Vector2Int>(), new Vector2Int(999, 999));
                        break;
                    }
                }
            }
            if (!found)
            {
                wanted_object = null_object;
            }
        }

        if (wanted_object != null_object)
        {

            //IF we're at the position, do the stuff.
            if (position == moveto_position)
            {
                //How long does it take to do the stuff? Need this functionality.
                //current_room.RemoveItem(moveto_position);

                //REDO: TODO: What happens when bar is full?

                //Big switch case odds are? This is EAT
                if (wanted_object.durability > 0)
                {
                    if (current_action.action == Enums.actions.eat)
                    {
                        if (needs["hunger"] < 1)
                        {
                            //TODO: Add handling when duravility is 0.
                            wanted_object.Damage();
                            needs["hunger"] = Mathf.Clamp(needs["hunger"] + wanted_object.nutrition, 0, 1);

                            return name + " is doing action " + next_action.action + " with original intention of " + next_action.value + " onto " + wanted_object.name +
                                " and his new hunger value is " + needs["hunger"] + ".";
                        }
                        else
                        {
                            return FinishedAction();
                        }
                    }
                }
                else
                {
                    return name + "'s item is out of durabililty.";
                }

            }
        }
        else
        {
            return name + " has no idea what to do!";
        }

        //Move towards item; use actual pathfinding at some point.
        position = path_to_obj[0];
        path_to_obj.RemoveAt(0);


        return name + " is moving towards the " + wanted_object.name + " at position " + moveto_position +
            " with action " + current_action.action + " with intention " + current_action.value +
            "\nCurrently " + name + " is at now at position " + position;


    }

    //finished action    
    private string FinishedAction()
    {
        //Finished doing the stuff, apply changes.
        string ret = "";
        ret = name + " is finished with action " + current_action.action + 
            " and is ready to do something else, with a hunger value of " + needs["hunger"] + ".";

        current_action.action = Enums.actions.nothing;

        wanted_object = null;

        return ret;

    }

    //Is there a stored like value?
    private bool LikesItem(string x)
    {
        for (int i = 0; i < likes_things.Count; i++)
        {
            if (likes_things[i].thing.name == x)
                return true;
        }
        return false;
    }

    //Adds items rn, prob add other thing later.
    public void AddLikeItem(Thing itm)
    {
        if (!LikesItem(itm.name))
        {
            Preference_Things p = new Preference_Things();
            p.thing = itm;

            //Write algorithm based on values for this. How do you define things like this?
            p.like_value = 0.75f;

            likes_things.Add(p);
        }
    }

	public Vector2Int Position
	{
		get { return new Vector2Int(position.x, position.y); }
	}

	public List<Preference_Things> Likes
	{
		get { return new List<Preference_Things>(likes_things); }
	}
}
