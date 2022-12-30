using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour
{
    //First startup story
    [SerializeField] ScriptableStory firstStory;
    [SerializeField] Image imageDisplay;
    [SerializeField] Text storyDescription;
    [SerializeField] Transform optionContainer;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider loadingBar;
    //Story matrix keeps track of the players choices
    [SerializeField] GameObject StoryMatrixPrefab;
    StoryMatrix storyMatrix;
    private void Start()
    {
        SlideShowStarter persistentStory = FindObjectOfType<SlideShowStarter>();
        GameObject tempStoryMatrix = Instantiate(StoryMatrixPrefab);

        storyMatrix = FindObjectOfType<StoryMatrix>();
        if(storyMatrix == null)
        { 
            
            storyMatrix = tempStoryMatrix.GetComponent<StoryMatrix>();
        }


        if (persistentStory == null) {
            if(firstStory != null) StartStory(firstStory);
        }
        else
        {
            StartStory(persistentStory.story);
            Destroy(persistentStory.gameObject);
        }
        

    }

    public void StartStory(ScriptableStory story)
    {
        imageDisplay.sprite = story.storyPic;
        storyDescription.text = story.storyDescription;

        
        //Remove buttons
        while (optionContainer.childCount > 0)
        {
            DestroyImmediate(optionContainer.GetChild(0).gameObject);
        }
        
        foreach (var choise in story.storyChoises)
        {

            if (choise.storyDecision != null) { 
                bool fillsTheRequirements = true;
                foreach (var storyEvent in choise.storyDecision)
                {
                    if (!storyMatrix.GetValidation(storyEvent)) {
                        fillsTheRequirements = false;
                            break;
                    } 
                }

                if (!fillsTheRequirements) continue;
            }

            //Add Option
            GameObject button = Instantiate(buttonPrefab, optionContainer);
            button.GetComponentInChildren<Text>().text = choise.optionText;
            //Onclick changes to another story of that choice

            switch (choise.currentOptionStyle)
            {
                case Choise.optionStyle.StoryChange:
                    button.GetComponentInChildren<Button>().onClick.AddListener(delegate { PlayerChoice(choise); });
                    break;
                case Choise.optionStyle.SceneChange:
                    button.GetComponentInChildren<Button>().onClick.AddListener(delegate { StartScene(choise.sceneIndex); });
                    break;
                default:
                    break;
            }
        }
    }

   
    private void PlayerChoice(Choise choise)
    {
        if (choise.choiceImpact != null)
        {
            foreach (var story in choise.choiceImpact) {

                storyMatrix.SetValue(story); 
            }
        }
        
        StartStory(choise.change);
    }

    void StartScene(int scene) {
        StartCoroutine(LoadSceneAsynchronously(scene));

    }

    IEnumerator LoadSceneAsynchronously(int index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            yield return null;
        }
    }
}
