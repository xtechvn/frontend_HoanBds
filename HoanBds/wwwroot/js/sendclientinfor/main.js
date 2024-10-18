var _client_infor =
{
    Send: function ()
    {
        $("#btn-send").prop("disabled", true);
        var obj =
        {
            name : $("#name").val(),
            email : $("#email").val(),
            phone : $("#phone").val(),
            content : $("#content").val()
        }
        let Form = $("#contact-form")
        Form.validate({
            rules: {
                "name": {
                    required: true,
                },
                "phone": {
                    required: true,
                    number: true
                },
                "email": {
                    required: true,
                    email: true 
                },
                "content": {
                    required: true,
                },
            },
            messages: {
                "name": {
                    required: "Vui lòng nhập tên",
                },
                "phone": {
                    required: "Vui lòng nhập số điện thoại",
                    number: "Số điện thoại không hợp lệ"
                },
                "email": {
                    required: "Vui lòng nhập email",
                    email: "Email không hợp lệ"
                },
                "content": {
                    required: "Vui lòng nhập nội dung",
                }
            },

        });


        if (Form.valid())
        {
            $.ajax({
                type: 'POST',
                url: 'Client/SendClientInfor',
                data: obj,
                success: function (data) {
                    $("#NotiModalCenter").addClass('show');
                    $("#NotiModalCenter").css('display','block');
                    $("#name").val('');
                    $("#email").val('');
                    $("#phone").val('');
                    $("#content").val('');
                    setTimeout(
                        function ()
                        {
                            $("#NotiModalCenter").removeClass('show');
                            $("#NotiModalCenter").css('display', 'none');
                            $("#btn-send").prop("disabled", false);
                        }
                        , 2000);
                },
                error: function (xhr, status, error) {
                    console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
                }
            });
        }
    }
}