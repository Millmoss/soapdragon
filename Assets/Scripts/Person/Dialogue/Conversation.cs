using System.Collections;
using System.Collections.Generic;

public class Conversation
{
	private LineLibrary ll;
	private ExpressionLibrary el;
	private char[] splitPercent;

	public Conversation()
	{
		ll = FileManager.initLines();   //this will be creating a new instance for each character if conversation class isn't a universal class
		el = FileManager.initExpressions();
		splitPercent = new char[1];
		splitPercent[0] = '%';
	}

	/*
		public void speak(from, to, about, feeling, lower)
		from - person who is speaking
		to - person being spoken to
		about - person or thing being talked about
		feeling - feeling toward person or thing being talked about
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

			Noun aboutNoun = null;

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, person.name, feeling, lineStrings[i], type, lower);
					if (e is Noun)
						aboutNoun = (Noun)e;
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type, aboutNoun);
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

			Noun aboutNoun = null;

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, null, feeling, lineStrings[i], type, lower);
					if (e is Noun)
						aboutNoun = (Noun)e;
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type, aboutNoun);
			return l;
		}

		if (about is string)
		{
			string thing = (string)about;
			Enums.lineTypes type = determineLine(from, to, thing, null, "string", feeling);
			List<string> lineStrings = ll.getLineString(type);

			Expression[] eList = new Expression[lineStrings.Count];

			Noun aboutNoun = null;

			for (int i = 0; i < lineStrings.Count; i++)
			{
				if (lineStrings[i][0] == '%')
				{
					Expression e = determineExpression(from, to, thing, feeling, lineStrings[i], type, lower);
					if (e is Noun)
						aboutNoun = (Noun)e;
					eList[i] = e;
				}
				else
				{
					eList[i] = new Expression(lineStrings[i]);
				}
			}

			Line l = new Line(eList, type, aboutNoun);
			return l;
		}

		return null;
	}

	public Enums.lineTypes determineLine(Person from, Person to, string about, Thing thing, string aboutType, float feeling)
	{
		Enums.lineTypes type = Enums.lineTypes.answerDanger;

		if (aboutType == "person")
		{
			if (to.name.Equals(about))
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
		else if (aboutType == "string")
		{
			if (about == "greeting")
				type = Enums.lineTypes.greeting;
			else
				type = Enums.lineTypes.opinionUndirected;
		}
		else if (aboutType == "line")
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

	public Expression determineExpression(Person from, Person to, string about, float feeling, string tempExpression, Enums.lineTypes type, int lowerBound)
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
					Noun n = new Noun(about, Enums.generalTypes.person, about);
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
					string f = from.GetMatchingFeature(to, feeling);
					Noun n = new Noun(f, Enums.generalTypes.feature, about);
					e = n;
					break;
				}
				case "%noun":
				{
					if (type == Enums.lineTypes.threatDirected)
					{
						string near = from.GetRandomNearbyItem().name;
						Noun n = new Noun(near, Enums.generalTypes.thing, "none");
						e = n;
					}
					else
					{
						string[] ns = about.Split(splitPercent);
						Noun n = new Noun(ns[1], Enums.generalTypes.thing, ns[1]);
						e = n;
					}
					break;
				}
				case "%noun%feature":
				{
					char[] splitPercent = new char[1];
					splitPercent[0] = '%';
					Noun n;
					string f = about.Split(splitPercent)[2];
					n = new Noun(f, Enums.generalTypes.feature, about.Split(splitPercent)[1]);
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
