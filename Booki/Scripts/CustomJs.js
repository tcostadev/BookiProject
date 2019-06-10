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

    if (new Date(dataInicio) < new Date()) {
        ShowNotification("A data de inicio tem que ser maior que o dia atual!", "alert-warning", 7500);
        return;
    }

    location.href = urlAction + '?dataInicio=' + dataInicio + '&dataFim=' + dataFim + '&localizacao=' + localizacao;
}

function OnClickReservarDestino(idTarifa) {
    var objTarifa = JSON.parse($("tr#" + idTarifa).attr("data-object"));

    var nNoites = new Date(new Date($('#data_fim').val()) - new Date($('#data_inicio').val())) / 1000 / 60 / 60 / 24;
    var precoUni = objTarifa.PrecoUnidade;

    $("#TarifaJson").val(JSON.stringify(objTarifa));

    $("#C_data_inicio").val($("#data_inicio").val());
    $("#C_data_fim").val($("#data_fim").val());
    $("#C_n_noites").val(nNoites);
    $("#C_TipoQuarto").val(objTarifa.DesignacaoTipoQuarto);
    $("#C_total").val(precoUni * nNoites + "€");
    $("#C_total_inicial").val(precoUni * nNoites);

    $('#reserva_destino').modal('show');
}

function OnClickConfirmarReserva(url) {
    var objTarifa = JSON.parse($("#TarifaJson").val());

    if ($("#NHospedes").val() === "") {
        ShowNotification("Número de hospedes obrigatório", "alert-warning", 7500);
        return;
    }

    if (objTarifa.Capacidade < parseInt($("#NHospedes").val())) {
        ShowNotification("A capacidade do quarto escolhido é de: " + objTarifa.Capacidade + " hóspede", "alert-warning", 7500);
        return;
    }

    var listaServicos = [];

    var servicosSelected = $("#multi_pedidos").find("input[type=checkbox]:checked");
    if (servicosSelected !== undefined && servicosSelected.length > 0) {
        $.each(servicosSelected, function (index, value) {
            listaServicos.push($(value).attr("id_artigo"));
        });
    }

    $.post({
        url: url,
        data: {
            jsonTarifa: $("#TarifaJson").val(),
            nHospedes: $("#NHospedes").val(),
            dataInicio: $("#C_data_inicio").val(),
            dataFim: $("#C_data_fim").val(),
            precoTotal: $("#C_total").val(),
            listServicos: JSON.stringify(listaServicos)
        },
        success: function (result) {
            ShowNotification("A reserva foi efetuada com sucesso", "alert-success", 7500);
            $('#reserva_destino').modal('hide');
            setTimeout(location.reload(), 300);
        }
    });
}

function OnClickCancelarReserva(idReserva, url) {
    var response = confirm("Tem a certeza que pretende cancelar esta reserva?");
    if (response == true) {
        $.post({
            url: url,
            data: {
                idReserva: idReserva
            },
            success: function (result) {
                ShowNotification("A reserva foi cancelada!", "alert-success", 7500);
                setTimeout(location.reload(), 300);
            }
        });
    } 
}