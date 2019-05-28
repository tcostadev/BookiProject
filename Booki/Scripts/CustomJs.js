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
        clearTimeout();
        setTimeout(function () { $("#notif_content").removeClass("active"); }, time);
    }
}

function IsEmptyField(valField) {
    return valField === undefined || valField === null || valField === "";
}

function OnClickPesquisar(urlAction) {
    var localizacao = $("#localizacao").val();
    var dataInicio = $("#data_inicio").val();
    var dataFim = $("#data_fim").val();

    if (IsEmptyField(localizacao) || IsEmptyField(dataInicio) || IsEmptyField(dataFim)) {
        ShowNotification("Por favor confirme os dados introduzidos!", "alert-warning", 7500);
        return;
    }

    if (dataInicio > dataFim) {
        ShowNotification("A data de inicio não pode ser maior que a data de fim!", "alert-warning", 7500);
        return;
    }

    location.href = urlAction + '?dataInicio=' + dataInicio + '&dataFim=' + dataFim + '&localizacao=' + localizacao;
}