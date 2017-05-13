
var showButton1 = false;
var showButton2 = false;

window.onload = function () {
    $("#FacultyId").hide();
    $("#FacultyLabel").hide();
    $("#DepId").hide();
    $("#DepartamentLabel").hide();
    $("#CourseId").hide();
    $("#CourseLabel").hide();
    $("#CreateButton").hide();
    $("#StudentLabel").hide();
    $("#StudentId").hide();
};

$(document)
    .ready(function () {
        $("#UniversityId")
            .change(
                function () {
                    var id = $("#UniversityId").val();
                    var url = "/UsersProfile/GetFaculty?universityId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { universityId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelectFaculty($("#UniversityId").closest("form").find("#FacultyId"), result, false);
                            $("#FacultyId").show();
                            $("#FacultyLabel").show();
                        },
                        error: function (xhr, status, error) {
                            $("#FacultyId").hide();
                            $("#FacultyLabel").hide();
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });


function fillSelectFaculty(select, data, hasEmpty, defaultValue) {
    if (!defaultValue)
        defaultValue = select.data("sel-item");

    var selID = select.find("option:selected").val();

    if ((typeof (hasEmpty) != "undefined" && !hasEmpty))
        select.find("option").remove();
    else
        select.find("option:gt(0)").remove();
    select.append($("<option></option>").text(" ").val(" "));
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
                            fillSelectDepartment($("#FacultyId").closest("form").find("#DepId"), result, false);
                            $("#DepId").show();
                            $("#DepartamentLabel").show();
                        },
                        error: function (xhr, status, error) {
                            $("#DepId").hide();
                            $("#DepartamentLabel").hide();
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });

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
        $("#DepId")
            .change(
                function () {
                    var id = $("#DepId").val();
                    var url = "/ClassBooks/GetCourse?depId=" + id;
                    var url2 = "/ClassBooks/GetStudents?depId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { depId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelectCourse($("#DepId").closest("form").find("#CourseId"), result, false);
                            $("#CourseId").show();
                            $("#CourseLabel").show();
                            if (result.length === 0) {
                                $("#CourseId").hide();
                                $("#CourseLabel").hide();
                                $("#StudentId").hide();
                            } else {
                                if ($("#StudentId").is(":visible")) {
                                    $("#CreateButton").show();
                                } else {
                                    $("#CreateButton").hide();
                                }
                            }
                        },
                        error: function (xhr, status, error) {
                            $("#CourseId").hide();
                            $("#CourseLabel").hide();
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                    $.ajax({
                        type: "GET",
                        url: url2,
                        data: { facultyId: id },
                        success: function (result) {
                            console.log(result); // show response from the script.                 
                            fillSelectStudents($("#DepId").closest("form").find("#StudentId"), result, false);
                            $("#StudentLabel").show();
                            $("#StudentId").show();
                            if (result.length === 0) {
                                $("#StudentLabel").hide();
                                $("#StudentId").hide();
                            } else {
                                if ($("#CourseId").is(":visible")) {
                                    $("#CreateButton").show();
                                } else {
                                    $("#CreateButton").hide();
                                }
                            }

                        },
                        error: function (xhr, status, error) {
                            $("#StudentLabel").hide();
                            $("#StudentId").hide();
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });

    });



/////
//function play() {
//    var id = $("#DepId").val();
//    var url = "/ClassBooks/GetStudents?depId=" + id;
//    $.ajax({
//        type: "GET",
//        url: url,
//        data: { facultyId: id },
//        success: function (result) {
//            console.log(result); // show response from the script.                 
//            fillSelectStudents($("#DepId").closest("form").find("#StudentId"), result, false);
//            $("#CreateButton").show();
//            $("#StudentLabel").show();
//            $("#StudentId").show();
//            if (result.length === 0) {
//                $("#CreateButton").hide();
//                $("#StudentLabel").hide();
//                $("#StudentId").hide();
//            }

//        },
//        error: function (xhr, status, error) {
//            $("#CreateButton").hide();
//            $("#StudentLabel").hide();
//            $("#StudentId").hide();
//            var err = eval("(" + xhr.responseText + ")");
//            console.log(error);
//        }
//    });
//}
////



////
function fillSelectCourse(select, data, hasEmpty, defaultValue) {
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
            select.append($("<option></option>").text(this.Name).val(this.CourseId));
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

function fillSelectStudents(select, data, hasEmpty, defaultValue) {
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