using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translatefunc : MonoBehaviour {

	public RectTransform panel;
	public RectTransform[] players;
	public RectTransform selectedposition;

	public int selectedIndex;
	public bool dragging = false;

	void Start () {
	}

	void Update () {

		if (!dragging) { //如果目前没有在滑动
			lerpEleToCenter(selectedposition.position.x-players[selectedIndex].position.x); //LerpEleToCenter作用是自然地滑到目标距离,参数是应该向正方向移动的距离
		}

	}

	void lerpEleToCenter(float x)
	{
		if (Mathf.Abs (x) < 3)
			return;
		float newx = Mathf.Lerp(0,x,Time.deltaTime* 20);

		panel.position = new Vector3(panel.position.x+newx,panel.position.y,panel.position.z);
	}

	public void StartDrag()
	{
		dragging = true;
	}
	public void EndDrag()
	{
		dragging = false;

		float minDistance = Mathf.Abs (players [0].position.x - selectedposition.position.x);
		selectedIndex = 0;
		float tempDistance;
		for (int i = 1; i < players.Length; i++) {
			tempDistance = Mathf.Abs (players [i].position.x - selectedposition.position.x);
			if (minDistance > tempDistance) {
				minDistance = tempDistance;
				selectedIndex = i;
			}
		}
	}

	public void addSelectIndex()
	{
		selectedIndex += 1;
		selectedIndex = Mathf.Clamp (selectedIndex, 0, players.Length-1);
	}

	public void reduceSelectIndex()
	{
		selectedIndex -= 1;
		selectedIndex = Mathf.Clamp (selectedIndex, 0, players.Length - 1);
	}
}

