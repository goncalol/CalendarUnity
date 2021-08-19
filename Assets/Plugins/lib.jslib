mergeInto(LibraryManager.library, {
 
  GetCSRFToken: function () {
  
    var at = getCSRFTokenJS();
	
	var bufferSize = lengthBytesUTF8(at) + 1; 
	
    var buffer = _malloc(bufferSize);
	
    stringToUTF8(at, buffer, bufferSize);
	
    return buffer;
  },

  SendData: function (str) {
    sendDataToServer(UTF8ToString(str))
  }

});