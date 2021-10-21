using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Starter class
public class FileSelector : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject TopPanel;
    public GameObject SidePanel;
    public GameObject Agent;
    private string _path;
    private Queue<State> _states = new Queue<State>(); 
    private bool _toggled;
    private FsmAgent _agent;
    void Start()
    {
        _path = Application.dataPath;
        TopPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text = _path;
        _toggled = TopPanel.transform.GetChild(3).GetComponent<Toggle>().isOn;
    }
    // activates when begin is pressed
    public void selectFileAtPath()
    {
        _toggled = TopPanel.transform.GetChild(3).GetComponent<Toggle>().isOn;
        if(!_toggled)
        {
            _path = TopPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text;
            if(File.Exists(_path))
            {   
                string text = System.IO.File.ReadAllText(_path);
                TranscribeText(text);
            }
            else
            {
                string text = "File not found at location: "+ _path;
                UiHelper.GetInstance().SpawnPopUp(text);
            }
        }
        else
        {
            _path = TopPanel.transform.GetChild(0).GetComponent<TMP_InputField>().text;
            TranscribeText(_path);
        }
    }

    // function that transcribes the text recived from the file, or input box, into a Queue of States.
    public void TranscribeText(string text)
    {   
        // if redoing a session destroy old FSMAgent
        if(_agent != null){_agent.destroySelf();}

        

        for(int i = 0 ; i < text.Length ; i++)
        {
            string value = text[i].ToString();
            State new_state = new State();
            switch(value)
            {
                case "F":
                    new_state = new F();
                    
                    break;
                case "R":
                    new_state = new R();
                    break;
                case "L":
                    new_state = new L();
                    break;
                default:
                    UiHelper.GetInstance().SpawnPopUp("Invalid input text.");
                    _states = new Queue<State>();
                    return;
                    

            }
            _states.Enqueue(new_state);
        }

        // UI helper functions
        UiHelper.GetInstance().HidePanel(TopPanel);
        UiHelper.GetInstance().ShowPanel(SidePanel);

        //Starts the main code for the agent to traverse the world based on instructions.
        GameObject SpawnedAgent = Instantiate(Agent,new Vector3(0, 0, -1),Quaternion.identity);
        _agent = SpawnedAgent.transform.GetComponent<FsmAgent>();
        _agent.begin(_states);


    }
  
}
