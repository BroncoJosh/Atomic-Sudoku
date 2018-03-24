using UnityEngine;
using System.Collections;

public class PlayModeScripts : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                print(hit.transform.gameObject.name);
                if (hit.transform.gameObject.name.Contains("BabyButton_"))
                {
                    //hit.transform.gameObject.GetComponent<Letter>().Increase();
                    //hit.transform.gameObject.GetComponent<Letter>().letter.color = Color.green;
                    print("Hello");
                    //Application.LoadLevel("sudoku");
                }
            }
        }
    }
    void onMouseDown ()
    {
     
    }
    public void ButtonPressed ()
    {
        
        
    }
}
