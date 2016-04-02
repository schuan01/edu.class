function showMessageJS(msg) {

    if (msg != "" && msg != null) {

        var strArray = msg.split(",");

        switch (strArray[0]) {
            case "success":
                $.gritter.add({
                    title: strArray[1],
                    text: strArray[2],
                    class_name: 'with-icon check-circle success'
                });
                break;
            case "info":
                 $.gritter.add({
                     title: strArray[1],
                     text: strArray[2],
                     class_name: 'with-icon question-circle primary'
                });
                break;
            case "warning":
                 $.gritter.add({
                     title: strArray[1],
                     text: strArray[2],
                     class_name: 'with-icon exclamation-circle warning'
                 });
                break;
            case "danger":
                 $.gritter.add({
                     title: strArray[1],
                    text: strArray[2],
                    class_name: 'with-icon times-circle danger'
                });
                break;
            default:
                break;
        }
    }
}

function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}