function fillSelectTeacher(select, data, hasEmpty, defaultValue) {
    if (!defaultValue)
        defaultValue = select.data("sel-item");

    var selID = select.find("option:selected").val();

    if ((typeof (hasEmpty) != "undefined" && !hasEmpty))
        select.find("option").remove();
    else
        select.find("option:gt(0)").remove();
    select.append($("<option></option>").text("").val(""));
    $.each(data,
        function () {
            select.append($("<option></option>").text(this.fullname).val(this.UserId));
        });
    if (selID)
        select.find("option[value='" + selID + "']").prop("selected", true);
    else if (typeof defaultValue != "undefined") {
        if (select.find("option[value='" + defaultValue + "']").length > 0)
            select.find("option[value='" + defaultValue + "']").prop("selected", true);
        else
            select.find("option:contains('" + defaultValue + "')").prop("selected", true);
    }

    select.trigger("chosen:updated");
}


function fillSelectDepartment(select, data, hasEmpty, defaultValue) {
    if (!defaultValue)
        defaultValue = select.data("sel-item");

    var selID = select.find("option:selected").val();

    if ((typeof (hasEmpty) != "undefined" && !hasEmpty))
        select.find("option").remove();
    else
        select.find("option:gt(0)").remove();
    select.append($("<option></option>").text("").val(""));
    $.each(data,
        function () {
            select.append($("<option></option>").text(this.Name).val(this.DepId));
        });
    if (selID)
        select.find("option[value='" + selID + "']").prop("selected", true);
    else if (typeof defaultValue != "undefined") {
        if (select.find("option[value='" + defaultValue + "']").length > 0)
            select.find("option[value='" + defaultValue + "']").prop("selected", true);
        else
            select.find("option:contains('" + defaultValue + "')").prop("selected", true);
    }

    select.trigger("chosen:updated");
}

$(document)
    .ready(function () {
        $("#DepartamentId")
            .change(
                function () {
                    var id = $("#DepartamentId").val();
                    var url = "/Courses/GetTeacher?depId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { depId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelectTeacher($("#DepartamentId").closest("form").find("#TeacherId"), result, false);
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });


$(function () {
    $('#dd').datepicker({
        dateFormat: 'yy-mm-dd',
        onSelect: function (datetext) {

            var d = new Date(); // for now

            var h = d.getHours();
            h = (h < 10) ? ("0" + h) : h;

            var m = d.getMinutes();
            m = (m < 10) ? ("0" + m) : m;

            var s = d.getSeconds();
            s = (s < 10) ? ("0" + s) : s;

            datetext = datetext + " " + h + ":" + m + ":" + s;

            $('#dd').val(datetext);
        }
    });
});

$(document)
    .ready(function () {
        $("#FacultyId")
            .change(
                function () {
                    var id = $("#FacultyId").val();
                    var url = "/Courses/GetDepartment?facultyId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { facultyId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelectDepartment($("#FacultyId").closest("form").find("#DepartamentId"), result, false);
                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });



///////

function fillSelect(select, data, hasEmpty, defaultValue) {
    if (!defaultValue)
        defaultValue = select.data("sel-item");

    var selID = select.find("option:selected").val();

    if ((typeof (hasEmpty) != "undefined" && !hasEmpty))
        select.find("option").remove();
    else
        select.find("option:gt(0)").remove();
    select.append($("<option></option>").text("").val(""));
    $.each(data,
        function () {
            select.append($("<option></option>").text(this.Name).val(this.FacultyId));
        });
    if (selID)
        select.find("option[value='" + selID + "']").prop("selected", true);
    else if (typeof defaultValue != "undefined") {
        if (select.find("option[value='" + defaultValue + "']").length > 0)
            select.find("option[value='" + defaultValue + "']").prop("selected", true);
        else
            select.find("option:contains('" + defaultValue + "')").prop("selected", true);
    }

    select.trigger("chosen:updated");
}

$(document)
    .ready(function () {
        $("#UniversityId")
            .change(
                function () {
                    var id = $("#UniversityId").val();
                    var url = "/Departaments/GetFaculty?universityId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { universityId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelect($("#UniversityId").closest("form").find("#FacultyId"), result, false);

                        },
                        error: function (xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });



$(document)
    .ready(function () {
        var id = $("#UniversityId").val();
        var url = "/Departaments/GetFaculty?universityId=" + id;
        $.ajax({
            type: "GET",
            url: url,
            data: { universityId: id },
            success: function (result) {
                console.log(result); // show response from the script.                 
                fillSelect($("#UniversityId").closest("form").find("#FacultyId"), result, false);

            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                console.log(error);
            }
        });
    });


$(document)
    .ready(function () {
        var id = $("#FacultyId").val();
        var url = "/Courses/GetDepartment?facultyId=" + id;
        $.ajax({
            type: "GET",
            url: url,
            data: { facultyId: id },
            success: function (result) {
                console.log(result); // show response from the script.                 
                fillSelectDepartment($("#FacultyId").closest("form").find("#DepartamentId"), result, false);
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                console.log(error);
            }
        });
    });





$(document)
    .ready(function () {
        var id = $("#DepartamentId").val();
        var url = "/Courses/GetTeacher?depId=" + id;
        $.ajax({
            type: "GET",
            url: url,
            data: { depId: id },
            success: function (result) {
                console.log(result); // show response from the script.                 
                fillSelectTeacher($("#DepartamentId").closest("form").find("#TeacherId"), result, false);
            },
            error: function (xhr, status, error) {
                var err = eval("(" + xhr.responseText + ")");
                console.log(error);
            }
        });
    });