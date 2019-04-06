$("#EndDate").focusout(function () {
    if ($("#StartDate").val() != "" && $("#EndDate").val() != "") {
        if ($(this).val() < $("#StartDate").val()) {
            $("#btnOutage").prop("disabled", true);
            $("#errormess").html('End Date value cannot be lower then Start Date Value');
        }
        else {
            $("#btnOutage").prop("disabled", false);
            $("#errormess").html('');
        }
    }
});

$("#StartDate").focusout(function () {
    if ($("#StartDate").val() != "" && $("#EndDate").val() != "") {
        if ($(this).val() > $("#EndDate").val()) {
            $("#btnOutage").prop("disabled", true);
            $("#errormess").html('End Date value cannot be lower then Start Date Value');
        }
        else {
            $("#btnOutage").prop("disabled", false);
            $("#errormess").html('');
        }
    }
});

$("#IncidentNumber").focusout(function () {
    $("#errorIncident").html('');
    if ($("#IncidentNumber").val() != "" && $("#ID").val() == undefined) {
        $.ajax({
            url: 'GetIncidents',
            type: 'GET',
            data: { "IncidentNumber": $("#IncidentNumber").val() },
            success: function (result) {
                if (result=='True') {
                        $("#errorIncident").html("This Incident already assinged to another outage.")
                        $("#btnOutage").prop("disabled", true);
                } else
                {
                    $("#errorIncident").html('');
                    $("#btnOutage").prop("disabled", false);
                }
            }
        });
    }
});