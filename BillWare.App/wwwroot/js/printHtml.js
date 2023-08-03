window.printHtml = function (html, args) {
    var windowReference = window.open('', '_blank');
    windowReference.document.write('<html><head <link rel="stylesheet" type="text/css" href="css/fac.css"> <title></title> </head><body>');
    windowReference.document.write(html);
    windowReference.document.write('</body></html>');
    windowReference.document.close();
    windowReference.print();
};
