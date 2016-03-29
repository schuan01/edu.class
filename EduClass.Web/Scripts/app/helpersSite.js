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