function fillSelect(select, data, hasEmpty, defaultValue) {
    if (!defaultValue)
        defaultValue = select.data("sel-item");

    var selID = select.find("option:selected").val();

    if ((typeof (hasEmpty) != "undefined" && !hasEmpty))
        select.find("option").remove();
    else
        select.find("option:gt(0)").remove();

    $.each(data,
        function() {
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
    .ready(function() {
        $("#UniversityId")
            .change(
                function() {
                    var id = $("#UniversityId").val();
                    var url = "/UsersProfile/GetFaculty?universityId=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { universityId: id },
                        success: function(result) {
                            console.log(result); // show response from the script.                 
                            fillSelect($("#UniversityId").closest("form").find("#FacultyId"), result, false);

                        },
                        error: function(xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });


window.onload = function() {
    var id = $("#UniversityId").val();
    var url = "/UsersProfile/GetFaculty?universityId=" + id;
    $.ajax({
        type: "GET",
        url: url,
        data: { universityId: id },
        success: function(result) {
            console.log(result); // show response from the script.                 
            fillSelect($("#UniversityId").closest("form").find("#FacultyId"), result, false);
        },
        error: function(xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            console.log(error);
        }
    });
};


window.onload = function() {
    var id = $("#UserId").val();
    var url = "/UsersProfile/GetUserNameRole?id=" + id;
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        success: function(result) {
            console.log(result); // show response from the script.                 
            if (result === true) {
                $("#RoleId").hide();
                $("#RoleHide").hide();
            } else {
                $("#RoleId").show();
                $("#RoleHide").show();
            }
        },
        error: function(xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            console.log(error);
        }
    });
};


$(document)
    .ready(function() {
        $("#UserId")
            .change(
                function() {
                    var id = $("#UserId").val();
                    var url = "/UsersProfile/GetUserNameRole?id=" + id;
                    $.ajax({
                        type: "GET",
                        url: url,
                        data: { id: id },
                        success: function(result) {
                            console.log(result); // show response from the script.
                            if (result === true) {
                                $("#RoleId").hide();
                                $("#RoleHide").hide();
                            } else {
                                $("#RoleId").show();
                                $("#RoleHide").show();
                            }

                        },
                        error: function(xhr, status, error) {
                            var err = eval("(" + xhr.responseText + ")");
                            console.log(error);
                        }
                    });
                });
    });