using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
       
    }


    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject g = hit.collider.gameObject;
                string name = g.name;

                if (name == "Gear")
                {
                    GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                    
                    if (goParent.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                           goParent.transform.GetChild(9).gameObject.SetActive(false);
                    }
                    else
                    {
                        goParent.transform.GetChild(9).gameObject.SetActive(true);
                    }
                    System.Threading.Thread.Sleep(100);
                }

                if (name == "MusicOff")
                {
                    GameObject settingsactive = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsactive.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        GameObject temp = GameObject.Find("Slider");
                        if (temp != null)
                        {
                            Slider volumeSlider;
                            volumeSlider = temp.GetComponent<Slider>();

                            if (volumeSlider != null)
                            {
                                volumeSlider.normalizedValue = 0.5f;
                            }
                        }
                    }
                    GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                    goParent.transform.GetChild(8).gameObject.SetActive(false);
                    System.Threading.Thread.Sleep(100);

                }
                if (name == "MusicOn")
                {
                    GameObject settingsactive = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsactive.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        GameObject temp = GameObject.Find("Slider");
                        if (temp != null)
                        {
                            Slider volumeSlider;
                            volumeSlider = temp.GetComponent<Slider>();

                            if (volumeSlider != null)
                            {
                                volumeSlider.normalizedValue = 0;
                            }
                        }
                    }


                    GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                    goParent.transform.GetChild(8).gameObject.SetActive(true);
                    System.Threading.Thread.Sleep(100);
                }

                if (name == "BabyButton")
                {
                    GameObject settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }
                    PlayerPrefs.SetInt("Difficulty", 1);
                    Application.LoadLevel("sudoku");
                }

                if (name == "BeginnerButton")
                {
                    GameObject settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }
                    PlayerPrefs.SetInt("Difficulty", 2);
                    Application.LoadLevel("sudoku");
                }

                if (name == "NoviceButton")
                {
                    GameObject settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }
                    PlayerPrefs.SetInt("Difficulty", 3);
                    Application.LoadLevel("sudoku");
                }

                if (name == "ExpertButton")
                {
                    GameObject settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }
                    PlayerPrefs.SetInt("Difficulty", 4);
                    Application.LoadLevel("sudoku");
                }

                if (name == "InsaneButton")
                {
                    GameObject settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }
                    PlayerPrefs.SetInt("Difficulty", 5);
                    Application.LoadLevel("sudoku");
                }

                if (name == "StartButton")
                {
                            GameObject settingsact;
                            settingsact = hit.collider.gameObject.transform.parent.gameObject;
                    if (settingsact.transform.GetChild(9).gameObject.activeSelf == true)
                    {
                        return;
                    }

                    hit.collider.gameObject.SetActive(false);
                    GameObject goParent = hit.collider.gameObject.transform.parent.gameObject;
                    for (int i = 1; i < 7; i++)
                    {
                        goParent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    System.Threading.Thread.Sleep(100);
                    return;
                }
            }
           
        }


    }
   
}