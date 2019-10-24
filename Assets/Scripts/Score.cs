using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreText;
    public int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        score++;
        scoreText.text = ""+score;
    }
}
