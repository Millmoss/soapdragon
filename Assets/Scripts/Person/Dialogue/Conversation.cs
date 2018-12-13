using System.Collections;
using System.Collections.Generic;

public class Conversation
{
	private LineLibrary ll;
	private ExpressionLibrary el;

	public Conversation()
	{
		ll = FileManager.initLines();   //this will be creating a new instance for each character if conversation class isn't a universal class
		el = FileManager.initExpressions();
	}

	/*
		public void speak(from, to, person, feeling, lower)
		from - person who is speaking
		to - person being spoken to
		person - person being talked about
		feeling - feeling toward person being talked about
		lower - just make this == -1 for now
	 */

	public Line speak(Person from, Person to, object about, float feeling, int lower)	//speaks about any object or string
	{
		if (about is Person)
		{
			Person person = (Person)about;
			Enums.lineTypes type = determineLine(from, to, person.name, null, "person", feeling);
			List<string> lineStrings = ll.getLineString(type);

			Expression[] eList = new Expression[lineStrings.Count];

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, person.name, feeling, lineStrings[i], type, lower);
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type);
			return l;
		}

		if (about is Thing)
		{
			NotMain.print("Thing");
		}

		if (about is Line)
		{
			Enums.lineTypes type = determineLine(from, to, null, null, "line", feeling);
			List<string> lineStrings = ll.getLineString(type);

			Expression[] eList = new Expression[lineStrings.Count];

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, null, feeling, lineStrings[i], type, lower);
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type);
			return l;
		}

		if (about is string)
		{
			string thing = (string)about;
			Enums.lineTypes type = determineLine(from, to, null, null, "string", feeling);
			List<string> lineStrings = ll.getLineString(type);

			Expression[] eList = new Expression[lineStrings.Count];

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, thing, feeling, lineStrings[i], type, lower);
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type);
			return l;
		}

		return null;
	}

	public Enums.lineTypes determineLine(Person from, Person to, string person, Thing thing, string about, float feeling)
	{
		Enums.lineTypes type = Enums.lineTypes.answerDanger;

		if (feeling < 0)	//this prevents people with high chill from threatening people cause that's not chill at all
			feeling += from.GetFeatureFloatValue("chill") / 2;

		if (about == "person")
		{
			if (to.name.Equals(person))
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
		else if (about == "string")
		{
			type = Enums.lineTypes.opinionUndirected;
		}
		else if (about == "line")
		{
			if (feeling < -.7f)
				type = Enums.lineTypes.threatDirected;
			else if (feeling < -.35f)
				type = Enums.lineTypes.insultDirected;
			else
				type = Enums.lineTypes.opinionOpinionSimple;
		}

		return type;
	}

	public Expression determineExpression(Person from, Person to, string person, float feeling, string tempExpression, Enums.lineTypes type, int lowerBound)
	{
		Expression e = new Expression("BAD");

		switch (tempExpression)
		{
				case "%feelingVerb":
				{
					string f = el.getExpressionString(Enums.expressionTypes.feelingVerb, feeling, lowerBound);
					Enums.descriptors d;
					if (feeling > .5f)
						d = Enums.descriptors.loving;
					else if (feeling > .0f)		//these are not mutually exclusive or anything, fix this later for sure
						d = Enums.descriptors.friendly;
					else if (feeling < -.66f)
						d = Enums.descriptors.hostile;
					else
						d = Enums.descriptors.safe;
					Enums.descriptors[] desc = new Enums.descriptors[1];
					desc[0] = d;
					Noun n = new Noun(person, Enums.generalTypes.person, person);
					Verb v = new Verb(f, desc, n);
					e = v;
					break;
				}
				case "%amountAdverb":		//later this should work together with other verbs/adjectives to offset and result in more dialogue variance
				{
					string f = el.getExpressionString(Enums.expressionTypes.amountAdverb, feeling, lowerBound);
					Adverb a = new Adverb(f, feeling + ((UnityEngine.Random.value - .5f) / 2), null);   //this adds a bit of variance
					e = a;
					break;
				}
				case "%feelingAdjective":
				{
					string f = el.getExpressionString(Enums.expressionTypes.feelingAdjective, feeling, lowerBound);
					Enums.descriptors d;
					if (feeling > .5f)
						d = Enums.descriptors.loving;
					else if (feeling > .0f)     //these are not mutually exclusive or anything, fix this later for sure
						d = Enums.descriptors.friendly;
					else if (feeling < -.666f)
						d = Enums.descriptors.hostile;
					else
						d = Enums.descriptors.safe;
					Enums.descriptors[] desc = new Enums.descriptors[1];
					desc[0] = d;
					Adjective a = new Adjective(f, desc);
					e = a;
					break;
				}
				case "%person%feature":
				{
					string f = "eyes";//from.GetMatchingFeature(person, feeling);
					Noun n = new Noun(f, Enums.generalTypes.feature, person);
					e = n;
					break;
				}
				case "%noun":
				{
					if (type == Enums.lineTypes.threatDirected)
					{
						string near = "rubber duck";//from.GetRandomNearbyItem().name;
						Noun n = new Noun(near, Enums.generalTypes.thing, "none");
						e = n;
					}
					else
					{
						char[] splitPercent = new char[1];
						splitPercent[0] = '%';
						string f = person.Split(splitPercent)[1];
						Noun n = new Noun(f, Enums.generalTypes.person, f);
						e = n;
					}
					break;
				}
				case "%noun%feature":
				{
					char[] splitPercent = new char[1];
					splitPercent[0] = '%';
					Noun n;
					if (person.Split(splitPercent).Length < 3)
					{
						n = new Noun("eyes", Enums.generalTypes.feature, person.Split(splitPercent)[1]);
						e = n;
						break;
					}
					string f = person.Split(splitPercent)[2];
					n = new Noun(f, Enums.generalTypes.feature, person.Split(splitPercent)[0]);
					e = n;
					break;
				}
				case "%agreementVerb":
				{
					Noun n = new Noun("line", Enums.generalTypes.line, to.name);
					Enums.descriptors d;
					Main.print(feeling + ", ");
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
					NotMain.print(tempExpression);
					break;
				}
		}

		return e;
	}
}
