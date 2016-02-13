using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UniCMS;

public class IndexViewController : MonoBehaviour {
	
	
	public void Awake () {}
	
	public void ShowContext () {
		// 右クリックを検知できるのかな。そもそもなんかキーバインド持って行かれてるきがするな。
		Debug.LogError("right clicker.");
	}
	
	public void GoToIndex (int index) {
		switch (index) {
			default: {
				var url = "page" + (index + 1);
				SiteManager.sManager.OpenUrl(url);
				break;
			}
		}
		
	}
	
	int frame = 0; 
	
	public void OnGUI () {
		
		frame ++;
	}
	
	
    [DllImport("__Internal")] private static extern void Connect();

    
    [DllImport("__Internal")] private static extern void SendLog(string message);
		
	
	[DllImport("__Internal")] private static extern void CopyToClipboard (string text);
	
	// この発展系でDLもいけるはず。
}