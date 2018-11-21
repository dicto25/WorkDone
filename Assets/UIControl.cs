using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour {

    private GameObject letterText;
    private UnityEngine.UI.Text textCompo;
    private GameObject clonedLetter;

    private int lettersCount = 0;

    bool onDisplay = false;
    private readonly float timeToWait = 0.7f;
    private float deltaTime =0;

	// Use this for initialization
	void Start () {
        letterText = GameObject.Find("LetterText");
        letterText.SetActive(false);
        textCompo = letterText.GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update () {
        this.deltaTime += Time.deltaTime;

        if (this.deltaTime >= timeToWait && onDisplay)
        {
            moveLetter();
            deltaTime = 0;
        }
	}

    //UI Commands
    public void DisplayLetterOnCenter(string letter)
    {
        letterText.SetActive(true);
        textCompo.text = "" + letter;
        lettersCount++;
        onDisplay = true;
    }

    public void ResetText()
    {
        lettersCount = 0;
    }

    private void moveLetter()
    {
        clonedLetter = Instantiate(letterText, letterText.transform.parent);
        clonedLetter.tag = "Respawn";
        letterText.SetActive(false);
        clonedLetter.GetComponent<Animator>().SetInteger("LetterCount", lettersCount);
        clonedLetter.GetComponent<Animator>().SetTrigger("StartMoving");
        onDisplay = false;
    }
}
