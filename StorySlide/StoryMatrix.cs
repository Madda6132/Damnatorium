using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryMatrix : MonoBehaviour
{
    public static StoryMatrix instance = null;
    [SerializeField] GameObject slideStarter = null;
    [SerializeField] List<storyEvent> storyEvents = new List<storyEvent>();
    //Poor choice. The script counts every time this script is created
    //to keep track of the players progression in the game.
    [SerializeField] static int counter = 0;
    private void Awake()
    {
        counter++;
        Debug.Log(counter);
        if (instance == null)
        {
            instance = this;
        } else if (counter >= 3)
        {
            Destroy(instance.gameObject);
            instance = this;
            counter = 0;
            
        } else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    public bool GetValidation(storyEvent key)
    {
        
        foreach (var item in storyEvents)
        {
            if (item.GetName() == key.GetName()) {

                return item.GetValue() == key.GetValue();
            }
        }
        return true;
    }


    public void SetValue(storyEvent key)
    {
        for (int i = 0; i < storyEvents.Count; i++)
        {
            if (storyEvents[i].GetName() == key.GetName()) {
                storyEvents[i] = key;
                break;
            }
                
        }
    }
}

[System.Serializable]
public struct storyEvent
{
    storyEvent(string name, bool value)
    {
        this.name = name;
        this.value = value;
    }

    [SerializeField] string name;
    [SerializeField] bool value;

    public string GetName()
    {
        return name;
    }

    public bool GetValue()
    {
        return value;
    }

    public void SetValue(bool setBool)
    {
        value = setBool;
    }
}
