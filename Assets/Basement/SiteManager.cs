using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniCMS
{
    public class SiteManager {
		private readonly GameObject siteManagerObj;
		private BootViewController cont;
		
		private readonly string basePath;
		private readonly string targetAssetPathBase;
		
		public static SiteManager sManager;
		
		private List<string> loadingUrls = new List<string>();
		
		private Dictionary<string, AssetBundle> assetCache = new Dictionary<string, AssetBundle>();
		
		public SiteManager (GameObject siteManagerObj, string indexUrl) {
			this.siteManagerObj = siteManagerObj;
			this.cont = siteManagerObj.GetComponent<BootViewController>() as BootViewController;
			this.basePath = indexUrl.Replace(UniCMS.BOOT_HTML_NAME, string.Empty);
			this.targetAssetPathBase = Path.Combine(basePath, "AssetBundles");
			
			Caching.CleanCache();
			sManager = this;
		}
		
		public void BackToIndex () {
			LoadIndexView();
		}
		
		public void LoadIndexView () {
			var sceneName = "index";
			var targetSceneAssetPath = Path.Combine(targetAssetPathBase, sceneName);
			OpenSceneFromURL(sceneName, targetSceneAssetPath);
		}
		
		public void OpenUrl (string url) {
			if (url.StartsWith("http")) {
				Debug.Log("外部サイトへ、別タブで開くとか。");
				return;
			}
			
			if (url.StartsWith(UniCMS.PATH_DELIMITER)) {
				Debug.LogError("/Aとかで来てる。どうすっかな〜〜 url:" + url);
				return;
			}
			
			// もし、preloadされてたAssetBundleをロードしたいなら、ここで。できるようにはなったけどうーんっていう。
			
			var sceneName = url;
			var targetSceneAssetPath = Path.Combine(targetAssetPathBase, sceneName);
			OpenSceneFromURL(sceneName, targetSceneAssetPath);
		}
		
		public void OpenSceneFromURL (string sceneName, string url) {
			// already cached, load from cache.
			if (Caching.IsVersionCached(url, 0)) {
				var contentsPaths = new List<string>();
				var sceneResources = assetCache[url + "resources"];
				var sceneAsset = assetCache[url];
				OpenSceneAsset(sceneAsset.GetAllScenePaths()[0], sceneResources, contentsPaths);
				return;
			}
			
			/*
				start loading page data from AssetBundle.
				
				2step(experimental).
				1.load scene resources(additional assetbundles) automatically.
				2.load scene itself then add 1's resources.
			*/
			cont.StartCoroutine(
				// load resource first.(no mean yet..).
				CacheReadyThenOpenScene(
					url + "resources", 
					(AssetBundle sceneResources) => {
						Debug.Log("succeeded to load asset of scene:" + url);
						
						// open contentsPaths.
						var contentsPaths = new List<string> {
							// "Assets/Index/Camera.prefab",
							// "Assets/Index/Canvas.prefab",
							// "Assets/Index/EventSystem.prefab"
						};
						
						cont.StartCoroutine(
							CacheReadyThenOpenScene(
								url, 
								(AssetBundle asset) => {
									OpenSceneAsset(asset.GetAllScenePaths()[0], sceneResources, contentsPaths);
								}
							)
						);
					}
				)
			);
		}
		
		private IEnumerator CacheReadyThenOpenScene (string sceneUrl, Action<AssetBundle> Finally) {
			while (!Caching.ready) {
				yield return null;
			}
			
			if (loadingUrls.Contains(sceneUrl)) {
				Debug.Log("url:" + sceneUrl + " is now loading.");
				yield break;
			}
			
			Debug.Log("ロード開始、 sceneUrl:" + sceneUrl);
			
			loadingUrls.Add(sceneUrl);
			
			cont.StartCoroutine(
				OpenScene(
					sceneUrl,
					(AssetBundle asset) => {
						assetCache[sceneUrl] = asset;
						loadingUrls.Remove(sceneUrl);
						Finally(asset);
					}
				)
			);
		}
		
		private IEnumerator OpenScene (string sceneUrl, Action<AssetBundle> Finally) {
			Debug.Log("start loading asset from:" + sceneUrl);
			
			AssetBundle asset;
			using (var www = WWW.LoadFromCacheOrDownload(sceneUrl, 0)) {
				while (!www.isDone) {
					yield return null;
				}
				
				if (string.IsNullOrEmpty(www.error)) {
					// no error.
				} else {
					Debug.LogError("failed to download asset of url:" + sceneUrl + " error:" + www.error);
					yield break;
				}
				
				asset = www.assetBundle;
			}
			
			Finally(asset);
		}
		
		private void OpenSceneAsset (string sceneNameSourceStr, AssetBundle sceneResources, List<string> contentsPaths) {
			var bundledSceneName = ToSceneName(sceneNameSourceStr);
			
			// load scene syncronously.
			SceneManager.LoadScene(bundledSceneName, LoadSceneMode.Single);
			
			var destinationScene = SceneManager.GetSceneByName(bundledSceneName);
			
			// can load assets automatically. works, but experimental.
			foreach (var contentsPath in contentsPaths) {
				var obj = sceneResources.LoadAsset(contentsPath);
				var instantiated = GameObject.Instantiate(obj) as GameObject;
				UnityEngine.Object.DontDestroyOnLoad(instantiated);
				
				SceneManager.MoveGameObjectToScene(instantiated, destinationScene);
			}
		}
		
		private string ToSceneName(string path) {
			string[] parts = path.Split('/');
			string scene = parts[parts.Length - 1];

			return scene.Remove(scene.Length - 6);
		}
		
	}
}