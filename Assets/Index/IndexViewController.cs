using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using UniCMS;

public class IndexViewController : MonoBehaviour {
	
	
	public void Awake () {
		// シーンに置いたキューブはそのまま出た。
		// ただ、なんかbuttonとかtextがmissingってでるな、、ffのキャッシュではない。
		
		// create canvas. ここまでは成立してる。エラーもなし。
		// var canvas = new GameObject("Canvas", typeof(Canvas));
		
		// // eventSystemも作らないといけない。
		// var eventSystem = new GameObject("EventSystem", typeof(EventSystem));
		
		// で。これと同じことを、AssetBundleを元に行う。
		// ・既存のものを読み出すと、missingが出る。なんで？？
		
		
		// Connect();
		
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