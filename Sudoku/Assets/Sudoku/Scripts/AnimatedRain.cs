using UnityEngine;
using System.Collections;


public class AnimatedRain : MonoBehaviour {

    
    float delay = 0.1f;
    private int textureCounter = 0;

    // Use this for initialization
    void Start () {
        InvokeRepeating("CycleTextures", delay, delay);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void CycleTextures()
    {
        textureCounter = ++textureCounter % 6;
       
        GameObject onParent = this.gameObject;
        GameObject rainChild = onParent.transform.GetChild(textureCounter).gameObject;
        onParent.GetComponent<Renderer>().material.mainTexture = rainChild.GetComponent<SpriteRenderer>().sprite.texture;
        //renderer.material.mainTexture = textures[textureCounter];
    }

}

