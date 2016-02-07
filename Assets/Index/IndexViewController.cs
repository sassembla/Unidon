using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndexViewController : MonoBehaviour {
	
	private List<string> indexies;
	
	
	public void Awake () {
		Debug.LogError("ここでリストをResから読み出す。ビルド工程を通過してればそのまま使えるはず。");
		indexies = new List<string>();
	}
	
	public void GoToIndex (int index) {
		// ここでurlを中継、indexiesに入っているurlを元に、っていう。
		// まずシーン呼び出しをするタイミングでリソースは分かれているのかな？っていう。まあ全部Assetでもいいんですが。
		// そしたらビルドの意味論が変わるだけなんで。
		
	}
}