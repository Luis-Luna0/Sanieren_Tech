// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function injectJS() {
    //const element = document.getElementById("myPdf");
    //element.getElementsByTagName("body")[0].setAttribute("oncontextmenu", "return false;");
    var iframevar = document.getElementById('myPdf');
    var elmnt = iframevar.contentWindow.document.getElementByTagName("modal-body");
    elmnt.setAttribute("oncontextmenu", "return false;");
}

//function disableContextMenu(var name) {
//    window.frames[name].document.oncontextmenu=fun
//}