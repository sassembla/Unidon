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
		
		
		public SiteManager (GameObject siteManagerObj, string indexUrl) {
			this.siteManagerObj = siteManagerObj;
			this.cont = siteManagerObj.GetComponent<BootViewController>() as BootViewController;
			this.basePath = indexUrl.Replace(UniCMS.BOOT_HTML_NAME, string.Empty);
			this.targetAssetPathBase = Path.Combine(basePath, "AssetBundles");
			
			Caching.CleanCache();
			sManager = this;
		}
		
		public void LoadIndexView () {
			var sceneName = "index";
			var targetSceneAssetPath = Path.Combine(targetAssetPathBase, sceneName);
			OpenSceneFromURL(sceneName, targetSceneAssetPath);
		}
		
		public void OpenSceneFromURL (string sceneName, string url) {
			cont.StartCoroutine(CacheReadyThenOpenScene(url + "resources", (AssetBundle sceneResources) => {
				Debug.Log("succeeded to load asset of scene:" + url);
				
				// open contentsPaths.
				var contentsPaths = new List<string> {
					// "Assets/Index/Camera.prefab",
					// "Assets/Index/Canvas.prefab",
					// "Assets/Index/EventSystem.prefab"
				};
				
				cont.StartCoroutine(CacheReadyThenOpenScene(url, (AssetBundle asset) => {
					OpenSceneAsset(asset.GetAllScenePaths()[0], sceneResources, contentsPaths);
				}));
			}));
		}
		
		public IEnumerator CacheReadyThenOpenScene (string sceneUrl, Action<AssetBundle> Finally) {
			while (!Caching.ready) {
				yield return null;
			}
			
			if (loadingUrls.Contains(sceneUrl)) {
				Debug.Log("url:" + sceneUrl + " is now loading.");
				yield break;
			} 
			
			loadingUrls.Add(sceneUrl);
			
			cont.StartCoroutine(
				OpenScene(
					sceneUrl,
					(AssetBundle asset) => {
						Finally(asset);
						loadingUrls.Remove(sceneUrl);
					}
				)
			);
		}
		
		private IEnumerator OpenScene (string sceneUrl, Action<AssetBundle> Finally) {
			Debug.Log("start loading asset from:" + sceneUrl);
			
			if (Caching.IsVersionCached(sceneUrl, 0)) {
				Debug.Log("asset is already cached, but not managed yet. sceneUrl:" + sceneUrl);
			} else {
				Debug.Log("not cached yet. sceneUrl:" + sceneUrl);
			}
			
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
			
			foreach (var contentsPath in contentsPaths) {
				var obj = sceneResources.LoadAsset(contentsPath);
				var instantiated = GameObject.Instantiate(obj) as GameObject;
				UnityEngine.Object.DontDestroyOnLoad(instantiated);
				
				var destinationScene = SceneManager.GetSceneByName(bundledSceneName);
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