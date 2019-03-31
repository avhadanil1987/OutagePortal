function SelectedIndexChanged() {
    document.getElementById("btnDownload").disabled = true; 
    hideDiv();
    $("table").find("tr:not(:first)").remove(); 
    if ($('#Years').val() == "") {
        alert("Please select Year.");
    }
    var month = $('#Months').val() == "" ? 0 : $('#Months').val();
    var appId = $('#ApplicationID').val() == "" ? 0 : $('#ApplicationID').val();
    var dataObject = JSON.stringify({
        'Year': $('#Years').val(),
        'Month': month,
        'applicationId': appId,
    });
    $.ajax({
        url: 'TotalAvailability',
        type: 'POST',
        contentType: 'application/json;',
        data: dataObject,
        success: function (result) {
            if (result) {
                if (result != "undefined" || result != null) {
                    document.getElementById("btnDownload").disabled = false; 
                    showDiv();
                    FillTable(result);
                    successFunc(result);
                }
            } else {
                alert("Error")
            }
        }
    });

    function successFunc(jsondata) {
        var chart = c3.generate({
            bindto: '#Barchart',
            data: {
                json: jsondata,
                keys: {
                    value: ['GoalAvailability', 'Outage'],
                },
                columns: ['GoalAvailability', 'Outage'],
                type: 'bar'
            },
            color: {
                pattern: [' #2ca02c', 'EF0909', '#ff7f0e', '#ffbb78', '#ff9896', '#1f77b4', '#d62728', '#aec7e8', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
            },
            pie: {
                label: {
                    format: function (value, ratio, id) {
                        return value;
                    }
                }
            }
        });
    }

    function FillTable(json) {
        var tr;
        for (var i = 0; i < json.length; i++) {
            tr = $('<tr/>');
            tr.append("<td>" + json[i].Year + "</td>");
            tr.append("<td>" + json[i].MonthName + "</td>");
            tr.append("<td>" + json[i].ApplicationName + "</td>");
            tr.append("<td>" + json[i].AvailabilityInPercentage + "</td>");
            tr.append("<td>" + json[i].Outage + "</td>");
            tr.append("<td>" + json[i].GoalAvailability + "</td>");
            $('table').append(tr);
        }  
    }

    function hideDiv() {
        document.getElementById('divAvailability').style.display = "none";
        document.getElementById('Barchart').style.display = "none";
    }
    function showDiv() {
        document.getElementById('divAvailability').style.display = "";
        document.getElementById('Barchart').style.display = "";
        
    }
}