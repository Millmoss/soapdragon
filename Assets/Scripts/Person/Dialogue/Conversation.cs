using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation
{
	private LineLibrary ll;
	private ExpressionLibrary el;

	public Conversation()
	{
		ll = FileManager.initLines();   //this will be creating a new instance for each character if conversation class isn't a universal class
		el = FileManager.initExpressions();
	}

	public void speak(Person from, Person to, Person person, float feeling, int lower) //speaks of person
	{
		Enums.lineTypes type = determineLine(from, to, person, null, "person", feeling);
		List<string> lineStrings = ll.getLineString(type);

		for (int i = 0; i < lineStrings.Count; i++)
		{
			if (lineStrings[i][0] == '.' && lineStrings[i].Length > 1)
			{
				Expression e = determineExpression(from, to, person, feeling, lineStrings[i], type, lower);
			}
		}
	}

	public void speak(Person from, Person to, Thing thing, float feeling, int lower)    //speaks of thing
	{
		int i = 0;
	}

	public void speak(Person from, Person to, Line line, float feeling, int lower)    //speaks of line, this is a response
	{
		int i = 0;
	}

	public Enums.lineTypes determineLine(Person from, Person to, Person person, Thing thing, string about, float feeling)
	{
		Enums.lineTypes type = Enums.lineTypes.answerDanger;

		if (feeling < 0)	//this prevents people with high chill from threatening people cause that's not chill at all
			feeling += from.chill / 2;

		if (about == "person")
		{
			if (to.name.Equals(person.name))
			{
				if (feeling < -.8f)
					type = Enums.lineTypes.threatDirected;
				else if (feeling < -.45f)
					type = Enums.lineTypes.insultDirected;
				else
					type = Enums.lineTypes.opinionDirected;
			}
			else
			{
				type = Enums.lineTypes.opinionUndirected;
			}
		}
		else if (about == "line")
		{
			if (feeling < -.7f)
				type = Enums.lineTypes.threatDirected;
			else if (feeling < -.35f)
				type = Enums.lineTypes.insultDirected;
			else if (feeling < .4f)
				type = Enums.lineTypes.opinionOpinionSimple;
		}

		return type;
	}

	public Expression determineExpression(Person from, Person to, Person person, float feeling, string tempExpression, Enums.lineTypes type, int lowerBound)
	{
		Expression e = new Expression();

		switch (tempExpression)
		{
				case ".feelingVerb":
				{
					break;
				}
				case ".amountAdverb":
				{
					break;
				}
				case ".feelingAdjective":
				{
					break;
				}
				case ".person.feature":
				{
					break;
				}
				case ".noun":
				{
					if (type == Enums.lineTypes.insultDirected)
					{
						string near = "rubber duck";//from.getRandomNearbyItem();
						Noun n = new Noun(near, Enums.generalTypes.thing, "none");
						e = n;
					}
					else
					{
						Noun n = new Noun(person.name, Enums.generalTypes.person, person.name);
						e = n;
					}
					break;
				}
				case ".noun.feature":
				{
					string f = "butt";//person.getMatchingFeature(feeling);
					Noun n = new Noun(f, Enums.generalTypes.feature, person.name);
					e = n;
					break;
				}
				case ".agreementVerb":
				{
					Noun n = new Noun("line", Enums.generalTypes.line, to.name);
					Enums.descriptors d;
					if (feeling > .33f)
						d = Enums.descriptors.agreeable;
					else if (feeling < -.33f)
						d = Enums.descriptors.disagreeable;
					else
						d = Enums.descriptors.safe;
					Enums.descriptors[] desc = new Enums.descriptors[1];
					desc[0] = d;
					Verb v = new Verb(el.getExpressionString(Enums.expressionTypes.agreementVerb, feeling, lowerBound), desc, n);
					e = v;
					break;
				}
				default:
				{
					break;
				}
		}

		return e;
	}
}
