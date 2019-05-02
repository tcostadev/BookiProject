function OnSucessForm(serverResponse) {

    if (serverResponse.Redirect) {
        window.location.href = serverResponse.UrlRedirect;
    }
    if (serverResponse.Notify) {
        ShowNotification(serverResponse.Message, serverResponse.Type, serverResponse.Time);
    }
}

/**
 * @param {any} message
 * @param {any} type
 * @param {any} time
 * 
 * type {
     * alert-primary
     * alert-success
     * alert-danger
     * alert-warning
 * }
 */

function ShowNotification(message, type, time) {
    $("#notif_content").removeClass();
    $("#notif_content").addClass("alert");
    $("#notif_content").addClass("active");
    $("#notif_content").addClass(type);

    $("#notif_content").find(".content").text(message);
    
    if (time !== 0) {
        setTimeout(function () { $("#notif_content").removeClass("active"); }, time);
    }
}