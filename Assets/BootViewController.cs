using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniCMS {
	public class UniCMS {
		public const string BOOT_HTML_NAME = "index.html";
	}
	
	public class BootViewController : MonoBehaviour {
		private const bool IS_DEBUG = true;
		
		public static List<string> indexies;
		
		private static SiteManager siteManager;
		
		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeMethodLoad () {
			var dataPath = Application.dataPath;
			var url = string.Empty;
			
			// set url for resources.
			if (string.IsNullOrEmpty(Application.absoluteURL)) {
				var localAssetPathBase = Directory.GetParent(dataPath).ToString();
				var localAssetPath = Path.Combine(localAssetPathBase, "UniCMSWeb/" + UniCMS.BOOT_HTML_NAME);
				
				url = "file://" + localAssetPath;
			} else {
				url = Application.absoluteURL;
			}
			
			if (url.StartsWith("file://")) {
				Debug.Log("running in local. attaching websocket debugguer. browser's log window is suck.");
				var webSocketConsole = new WebSocketConsole();
				Application.logMessageReceived += webSocketConsole.SendLog;
			} else {
				Debug.Log("running in production.");
			}
			
			var usingMemory = Profiler.GetTotalAllocatedMemory();
			var reservedMemory = Profiler.GetTotalReservedMemory();
			
			Debug.Log("load done, show everything. url:" + url + " dataPath:" + dataPath + " usingMemory:" + usingMemory + " reservedMemory:" + reservedMemory);
			
			// url:file:///Users/tomggg/Desktop/UniCMS/UniCMSWeb/index.html 
			// dataPath:file:///Users/tomggg/Desktop/UniCMS/UniCMSWeb 
			// usingMemory:		1,143,025 
			// reservedMemory:	1,585,093
			
			// ここから別のSceneをBuildTargetに入れてビルドして、どうなるのかみてみる。
			// usingMemory:		1,768,850 
			// reservedMemory:	2,223,982
			
			// ということで、Sceneリストに書くだけでビルドに巻き込まれるので、書かずに完全に別にビルドするのが得策っぽい。
			
			// Q.使用してないリソースがこのプロジェクトに増えた場合ってどうなるの？画像とか。
			// A.当然、容量は増えないので平和っぽい。
			
			if (SceneManager.GetActiveScene().name != "Boot") {
				Debug.LogError("this is not Boot scene, current scene is:" + SceneManager.GetActiveScene().name);	
				SceneManager.LoadSceneAsync("Boot", LoadSceneMode.Additive);
				return;
			}
			
			// loaded from Boot scene. start loading index scene.
			var siteManagerObj = GameObject.Find("SiteManagerObject");
			
			siteManager = new SiteManager(siteManagerObj, url);
			
			Debug.Log("urlによって呼び出すものを変えればいい。index.htmlからきてるなら、index。それ以外ならそれ以外のコンテンツを読み出す。 url:" + url);
			siteManager.LoadIndexView();
		}
		
		public void Start () {
			if (siteManager != null) return;
			
			var url = Application.absoluteURL;
			
			var siteManagerObj = GameObject.Find("SiteManagerObject");
			siteManager = new SiteManager(siteManagerObj, url);
		}
	}
	
	public class WebSocketConsole {
		// connect to editor.
		public WebSocketConsole () {
			Debug.LogError("not yet implemented. EditorとWSで通信したいっすね。ログをどっかに吐きたい");
		}
		
		
		public void SendLog (string condition, string stackTrace, LogType type) {
			
		}
		
		
	}
}