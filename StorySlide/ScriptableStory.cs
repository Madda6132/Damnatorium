using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StorySteps", menuName = "Story/StoryStep", order = 0)]
public class ScriptableStory : ScriptableObject
{

    [TextArea(3, 12)][SerializeField] public string storyDescription = "New Story";
    [SerializeField] public Sprite storyPic = null;
    [SerializeField] public List<Choise> storyChoises = new List<Choise>();
    
}

[System.Serializable]
public struct Choise {

    public enum optionStyle {
        StoryChange,
        SceneChange
    }

    [TextArea(3, 6)] public string optionText;
    public optionStyle currentOptionStyle;
    //change is the ScriptableStory the choose changes to
    public ScriptableStory change;
    public int sceneIndex;
    //The choice had an impact on the story
    public storyEvent[] storyDecision;
    public storyEvent[] choiceImpact;

    /// <summary>
    /// OptionText is the text next to the button, optionStyle will run the selected one and ignore the other.
    /// </summary>
    public Choise(string optionText, optionStyle option, ScriptableStory change, int sceneIndex, 
        storyEvent[] storyDecision, storyEvent[] choiceInpact)
    {
        this.optionText = optionText;
        currentOptionStyle = option;
        this.change = change;
        this.sceneIndex = sceneIndex;
        this.storyDecision = storyDecision;
        this.choiceImpact = choiceInpact;
    }

    
}
