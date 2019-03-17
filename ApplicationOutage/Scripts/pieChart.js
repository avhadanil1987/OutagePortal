    $(document).ready(function () {
        var MSiteName = [];
        var MasterJson;
        $.ajax({
            type: "GET",
            url: "PieChart",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                successFunc(response);
            },
});

function successFunc(jsondata) {
            var data = {};
    var countryNames = [];
            jsondata.forEach(function (e) {
        countryNames.push(e.CountryName);
    data[e.CountryName] = e.CountryPopulation;
})

            var chart = c3.generate({
        bindto: '#pieChart',
                data: {
        json: [data],
                    keys: {
        value: countryNames,
},
type: 'pie'
},
                color: {
        pattern: ['#1f77b4', '#aec7e8', '#ff7f0e', '#ffbb78', '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', '#8c564b', '#c49c94', '#e377c2', '#f7b6d2', '#7f7f7f', '#c7c7c7', '#bcbd22', '#dbdb8d', '#17becf', '#9edae5']
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
});