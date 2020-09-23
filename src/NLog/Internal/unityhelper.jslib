mergeInto(LibraryManager.library, {

    LoadFile: function (url) {

        var urlString = Pointer_stringify(url);
        console.log("LoadFile " + urlString);

        var request = new XMLHttpRequest();
        request.open('GET', urlString, false);  // `false` makes the request synchronous
        request.send(null);

        if (request.status === 200) {
            console.log(request.responseText);
        }

        var returnStr = request.responseText;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
   }

});