using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour {
	public string format;
	public string formatDown;
	public string formatDownDisabled;
	TMPro.TMP_Text textMesh;
	void Awake(){
		textMesh = GetComponent<TMPro.TMP_Text>();
	}

	public void UpdateScore(float baseScore, float activeScore, int multiplier, bool disabled = false)
    {
		textMesh.text = string.Format(disabled ? formatDownDisabled : formatDown, baseScore, activeScore, multiplier);
    }
	public void UpdateScore(float baseScore)
    {
		textMesh.text = string.Format(format, baseScore);
    }

	public void SumUpAnimation(){
		GetComponent<Animator>().SetTrigger("SumUp");
		print("SumUp");
	}
}
