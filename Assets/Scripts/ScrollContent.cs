using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollContent : MonoBehaviour {
	public GameObject ScrollViewGameObject;
	public GameObject ItemGameObject;
	
    // Start is called before the first frame update
    void Start() {
        //Cards is an array of data
		for (int i = 0; i < 2; i++) {
			//ItemGameObject is my prefab pointer that i previous made a public property  
			//and  assigned a prefab to it
			GameObject card = Instantiate(ItemGameObject) as GameObject;
	
			//scroll = GameObject.Find("CardScroll");
			if (ScrollViewGameObject != null) {
				//ScrollViewGameObject container object
				card.transform.SetParent(ScrollViewGameObject.transform,false);
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
