using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpressionLibrary
{
	public string[] feelingVerb;
	public string[] feelingAdjective;
	public string[] amountAdverb;
	public string[] agreementVerb;

	public static ExpressionLibrary CreateFromJSON(string jsonString)
	{
		return JsonUtility.FromJson<ExpressionLibrary>(jsonString);
	}

	public string getExpressionString(Enums.expressionTypes type, float strength, float lowerBound)
	{
		string expression = "";

		if (lowerBound == -1)
			strength = (strength + 1) / 2;

		if (strength >= 1)
			strength = 0.99f;
		else if (strength < 0)
			strength = 0;

		switch (type)
		{
				case Enums.expressionTypes.feelingVerb:
				{
					int index = Mathf.FloorToInt(strength * feelingVerb.Length);
					expression = feelingVerb[index];
					break;
				}
				case Enums.expressionTypes.feelingAdjective:
				{
					int index = Mathf.FloorToInt(strength * feelingAdjective.Length);
					expression = feelingAdjective[index];
					break;
				}
				case Enums.expressionTypes.amountAdverb:
				{
					int index = Mathf.FloorToInt(strength * amountAdverb.Length);
					expression = amountAdverb[index];
					break;
				}
				case Enums.expressionTypes.agreementVerb:
				{
					int index = Mathf.FloorToInt(strength * agreementVerb.Length);
					expression = agreementVerb[index];
					break;
				}
				default:
				{
					break;
				}
		}

		return expression;
	}
}
