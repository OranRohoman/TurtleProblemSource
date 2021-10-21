using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



//singleton that provides general UI helper functions.
public class UiHelper : MonoBehaviour
{
    public GameObject PopUp;
    public GameObject Canvas;
    public GameObject TextAction;
    private static UiHelper _instance;
    private GameObject _topPanel;
    private GameObject _sidePanel;
    public static UiHelper GetInstance()
	{
		return _instance;
	}
    void Start()
    {
        if(_instance != null  && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
        _sidePanel = Canvas.transform.GetChild(1).gameObject;
        _topPanel = Canvas.transform.GetChild(0).gameObject;

    }
    public void SpawnPopUp(string text)
    {
        GameObject spawnedPopUp = Instantiate(PopUp, new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity, Canvas.transform);
        
        spawnedPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
        spawnedPopUp.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate{ destroySelf(spawnedPopUp); });
        
    }
    private void destroySelf(GameObject panel)
    {
        Destroy(panel);
    }
    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void addToStack(GraphNode node)
    {
        GameObject content = _sidePanel.transform.GetChild(1).GetChild(0).gameObject;
        GameObject text =  Instantiate(TextAction,new Vector3(0, 0, 0),Quaternion.identity,content.transform);
        text.transform.SetSiblingIndex(0);
        string description = "Move: "+node.Coordinates.ToString() + " x"+node.VisitCount;
        setTMPro(text, description);
    }
    public void setTMPro(GameObject gameObject, string text)
    {
        gameObject.transform.GetComponent<TextMeshProUGUI>().text = text;
    }
    public void initTop(){
        _sidePanel.SetActive(false);
        _topPanel.SetActive(true);
        Delete_children(Canvas.transform.GetChild(1).GetChild(1).GetChild(0).gameObject);
    }
    public void toggleValueHandler()
    {
        GameObject header = _topPanel.transform.GetChild(1).gameObject;
        bool toggle = _topPanel.transform.GetChild(3).GetComponent<Toggle>().isOn;
        if(toggle){
            setTMPro(header,"Manually enter String. ex. FFFLFFLLF");
        }
        else{
            setTMPro(header,"Enter Path to text file.");
        }
    }
    public void Delete_children(GameObject parent)
	{
		for(int i = parent.transform.childCount-1; i>=0 ; i-- )
		{
			//print("deleting: "+ parent.transform.GetChild(i).name);
			Destroy(parent.transform.GetChild(i).gameObject);

		}
	}
    public void EndProgram()
    {
        Application.Quit();
    }
}
