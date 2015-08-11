using UnityEngine;
using System.Collections;

public class CardText : MonoBehaviour {
    
    int initializePosition = 0;
    public void Initialize() { Initialize(initializePosition); }

	public void Initialize (int position) {
        if (initializePosition == 0) initializePosition = position;
        MeshRenderer mRend = gameObject.GetComponent<MeshRenderer>();
		mRend.sortingLayerName = "Card";
		mRend.sortingOrder = position;
    }
}
