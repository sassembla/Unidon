var CopyUtil = {
	// unused.
	CopyToClipboard : function (textSource) {
		var messageStr = Pointer_stringify(textSource);
		// it's only IE!!!! fuck!!!!!
		// Copied = messageStr.createTextRange();
		// Copied.execCommand("Copy");
	},
	
	ShowCodeWindow : function (identity, x, y, w, h, text) {
		// targetとなるdomにいろいろ足す。これでOK
		// targetは事前に用意しておけばいいや。
		var elemLi = document.createElement('li');
		document.getElementById(identity).appendChild(elemLi);
		// <div id="wrapper">
		// 	<div style="position: absolute; left: 100px; top: 10px; width:200px; height:100px; background-color: white; overflow: scroll;">
		// 		abcd
		// asdasd
		// ad
		// dsadadadsdaasd
		// dsadasdsdas
		// dsdadasda
		// sdaddsada
		// dsdsd
		// dsdadsad
		// sdsdsadd
		// sddsdads
		// 	</div>
		// </div>
	}
};

mergeInto(LibraryManager.library, CopyUtil);