using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace UniCMS {
    public class Publisher {
		[MenuItem ("UniCMS/Generate link.xml")]
		static void GenerateLinkXML () {
			Debug.LogError("unused. this mechanism is not so effective yet. read comment of below of this Debug.LogError.");
			/*
				えーっとScriptのサイズを小さくしつつAssetBundleでのローディング最小化を目指して色々粘ってみたんですが
				link.xmlは全く無意味でしたよ。悲しい。
				http://sassembla.github.io/Public/2016:02:11%204-33-38/2016:02:11%204-33-38.html
				
				いろいろ判明するまで放置。
			*/
			// var classIds = new List<int>{23, 65, 33, 223, 213, 222};
			// ClassIdCollector.ExportLinkXMLWithUsingClassIds(classIds);
			// AssetDatabase.Refresh();
		}
		
		[MenuItem ("UniCMS/Build Contents")]
		static void BuildContentAssets () {
			var assetTargetPath = FileController.PathCombine(Directory.GetParent(Application.dataPath).ToString(), "UniCMSWeb/AssetBundles");
			FileController.RemakeDirectory(assetTargetPath);
			BuildPipeline.BuildAssetBundles(assetTargetPath, 0, BuildTarget.WebGL);
			// んで、ここで作ったAssetBundleを、リストとして出す必要がある。
		}
		
		[MenuItem ("UniCMS/Publish Site")]
		static void Publish () {
			// GenerateLinkXML();
			
			BuildContentAssets();
			
			var targetPath = FileController.PathCombine(Directory.GetParent(Application.dataPath).ToString(), "UniCMSWeb");
			BuildPipeline.BuildPlayer(new string[]{"Assets/Boot.unity"}, targetPath, BuildTarget.WebGL, 0);
		}
	}
}