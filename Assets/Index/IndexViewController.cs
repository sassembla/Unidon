using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class IndexViewController : MonoBehaviour {
	
	private List<string> indexies;
	
	
	public void Awake () {
		Debug.LogError("ここでリストをResから読み出す。ビルド工程を通過してればそのまま使えるはず。");
		indexies = new List<string>();
		
		// シーンに置いたキューブはそのまま出た。
		// ただ、なんかbuttonとかtextがmissingってでるな、、ffのキャッシュではない。
		
		// create canvas. 出現を確認しよう。
		var canvas = new GameObject("Canvas", typeof(Canvas));
		
		var found = GameObject.Find("Canvas");
		Debug.LogError("canvas is alive? found:" + found);
		
		Connect();
		
		// SendLog("from C#");
	}
	
	public void GoToIndex (int index) {
		// ここでurlを中継、indexiesに入っているurlを元に、っていう。
		// まずシーン呼び出しをするタイミングでリソースは分かれているのかな？っていう。まあ全部Assetでもいいんですが。
		// そしたらビルドの意味論が変わるだけなんで。
		Debug.LogError("hannnanana");
	}
	
	[DllImport("__Internal")]
    private static extern void Connect();

    [DllImport("__Internal")]
    private static extern void SendLog(string message);
}