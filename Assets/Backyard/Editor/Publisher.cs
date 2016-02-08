using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace UniCMS {
    public class Publisher {
		[MenuItem ("UniCMS/Generate link.xml")]
		static void GenerateLinkXML () {
			var classIds = new List<int>{23, 65, 33, 223, 213, 222};
			ClassIdCollector.ExportLinkXMLWithUsingClassIds(classIds);
			AssetDatabase.Refresh();
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
			GenerateLinkXML();
			
			BuildContentAssets();
			
			var targetPath = FileController.PathCombine(Directory.GetParent(Application.dataPath).ToString(), "UniCMSWeb");
			BuildPipeline.BuildPlayer(new string[]{"Assets/Boot.unity"}, targetPath, BuildTarget.WebGL, 0);
		}
	}
}