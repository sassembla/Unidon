#Unidon

1 push to generate website.  
Unidon is static WebSite generater using Unity WebGL.

##Automatic page generation
Unidon makes these ops automatically.

* Scene -> Page convert
* make AssetBundle for each scenes.
* Load Index page when browse.

Only push **Menu > Unidon > Publish Site**

Your websites are located at YOUR_PROJECT/UnidonWeb folder. let's locate it to your site. e.g. Dropbox.


##Lightweight as possible
Unidon basically takes 4.4MB js resources. it contains uGUI and controller code resources.

not featherweight yet.　ああ、、鳥とかになりたい。


##About rule of Contents
Unity's standard uGUI features are supported.  
You can easily make your assets to web things.

but 2 rules exists.

1. Your contents should be in Assets/Contents folder.
2. contents should keep naming rule. Assets/Contents/MY_CONTENT/MY_CONTENT.unity
3. Index/Index.unity is also editable, but should not rename it.


##How to load page by your script
do in your script.

```C#
// back to index.
SiteManager.sManager.BackToIndex();

// go to other scene.
var sceneName = "page1";// lower case of your scene name. without extension.
SiteManager.sManager.OpenScene(sceneName);

```

##Motivation
no more javascript. --just kidding.  

I like compilable-web. Unity + WebGL is just for doing it.

* Keep loading first. start page should be shown asap.
* Keep sites small. it's desired but not enough yet.
* Can control like HTML sites. 


##Caution
It's not production level yet.

##License
MIT.