using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveContent : MonoBehaviour
{
    public RectTransform Content;
	public float tuple_size;
	
	public void MoveContentPane() {
		var pos = Content.position;
		pos.y += tuple_size;
		Content.position = pos;
	}

    // Selects object by tapping on screen while ray cast is hitting object
    public void OnPointerClick() {
		MoveContentPane();
    }
}
