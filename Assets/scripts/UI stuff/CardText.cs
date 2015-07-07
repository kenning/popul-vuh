using UnityEngine;
using System.Collections;

public class CardText : MonoBehaviour {
    
    //this is really horrible but i can't figure out why it acts differently than in 4.6 so who cares
    bool initializedAlready = false;
    int initializePosition = 0;
    public void Initialize() { Initialize(initializePosition); }
    //end horrible crap

	public void Initialize (int position) {
        if (initializePosition == 0) initializePosition = position;
        MeshRenderer mRend = gameObject.GetComponent<MeshRenderer>();
		mRend.sortingLayerName = "Card";
		mRend.sortingOrder = position;
//        if (!initializedAlready)
//        {
//            Invoke("Initialize", .4f);
//            initializedAlready = true;
//        }
    }
}
