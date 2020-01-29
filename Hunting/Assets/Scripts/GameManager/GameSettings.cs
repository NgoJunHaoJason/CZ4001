using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using TMPro;

public class GameSettings : MonoBehaviour
{
    public TextMeshProUGUI message;
    public bool showArrowTrail;
    public float messageDuration;

    public Color badMessage;
    public Color goodMessage;

    public List<AnimalBehaviour.AnimalType> animalsInSeason = new List<AnimalBehaviour.AnimalType>();
    public int score = 0;

    private bool hasMessageToShow = false;
    private float messageDurationTimer = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (!hasMessageToShow)
            return;

        if (messageDurationTimer > 0)
            messageDurationTimer -= Time.deltaTime;
        else
        {
            message.text = "";
            hasMessageToShow = false;
        }
    }

    public void ToggleArrowTrail()
    {
        showArrowTrail = !showArrowTrail;
        if (showArrowTrail)
        {
            message.text = "Arrow Trail On";
        }
        else
        {
            message.text = "Arrow Trail Off";
        }
        message.color = goodMessage;
        hasMessageToShow = true;
        messageDurationTimer = messageDuration;
    }

    public void SubmitHuntedAnimal(AnimalBehaviour.AnimalType type)
    {
        if (animalsInSeason.Contains(type))
        {
            score++;
        }
        else
        {
            score--;
            message.text = "Please Hunt Responsibly";
            message.color = badMessage;
            hasMessageToShow = true;
            messageDurationTimer = messageDuration;
        }
    }
}
