//$(document).ready(function () {
//    //$("#dateOfBirth").datepicker({

//    //    dateFormat: "dd-mm-yy",
//    //    changeMonth: true,
//    //    changeYear: true

//    //});

//    function checkuserName() {
//    $('#userName').keyup(function () {
//        var userName = $(this).val();
//        if (userName.length >= 5) {
//            $.ajax({
//                url: 'Register/GetAvailableUserName',
//                datatype: 'json',
//                data: {userName:userName},
//                contentType: 'application/json; charset=utf-8',
//                method: 'post',
//                success: function (data) {
//                    var divElement = $('#divElement');
//                    var spanElement = $('#spanElement');
//                    var linkElement = $('#linkElement');
//                    if (data.userNameInUse) {
//                        divElement.text(userName + 'already in use');
//                    } else {
//                        divElement.text(data.userName + 'available');
//                    }
//                }
//            });
//        }
//    });
//    }
//});