$(document).ready(function ()
{
    let currentUrl = window.location.href;
    let params = currentUrl.split("-").pop().split(".").shift();
    $('.tree-menu li a').each(function () {
        if ($(this).attr('href').split("-").pop().split(".").shift() == params) {
            $(this).css('font-weight', 'bold');
        }
        else
        {
            $(this).css('font-weight', 'normal');
        }
    });
    
    policy.getBodyArtical(params);
})

var policy =
{
    getBodyArtical: function (Policy_Id)
    {
        $.ajax({
            type: 'POST',
            url: '/policy/getbodypolicy',
            data: { Policy_Id: Policy_Id },
            success: function (data) {
                $('#content-policy').html(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });
    }
}