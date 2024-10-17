var total_product = 4; // Tổng số sản phẩm của 1 box
var group_id_product_NP = 54;// id nhóm sản phẩm box NP
var group_id_product_CCMN = 54;// id nhóm sản phẩm box CCMN

$(document).ready(function () {
    // Page load render data by group product id
    var view_name = "~/Views/Shared/Components/Product/ProductListViewComponent.cshtml";
    var skip = 0;
    var take = total_product;
    /*    var div_location_render_data = ".component-product-list";*/
    var location_type = "CATEGORY";
    const currentUrl = window.location.href;
    const hasParams = currentUrl.indexOf('?') !== -1;
    if (!hasParams)
    {
        group_product.render_product_list_Home(group_id_product_NP, "#list_sp_ccmn_home", view_name, skip, take, location_type);
        group_product.render_product_list_Home(group_id_product_CCMN, "#list_sp_np_home", view_name, skip, take, location_type);
    }
    
})

/*$(document.body).on('click', '.ajax_action_page', function (e) {

    var page_index = (parseInt($(this).data("page")) - 1) * total_product;
    //// Sau khi bắn link, lấy giá trị group_id
    debugger;
    var groupId = lib.getUrlParameter('group_id');
    var group_product_id = groupId == null ? -1 : parseInt(groupId);
    var view_name = "~/Views/Shared/Components/Product/ProductListViewComponent.cshtml";
    var skip = page_index;
    var take = total_product;
    var div_location_render_data = ".component-product-list";
    var location_type = "HOME";
    group_product.render_product_list_Home(group_product_id, div_location_render_data, view_name, skip, take, location_type);
});*/

var lib = {
    // Hàm lấy tham số từ URL
    getUrlParameter: function (param) {
        var pageUrl = window.location.search.substring(1); // Lấy phần query string từ URL
        var urlVariables = pageUrl.split('&'); // Tách các tham số dựa trên ký tự &

        for (var i = 0; i < urlVariables.length; i++) {
            var parameter = urlVariables[i].split('='); // Tách tên và giá trị tham số

            if (parameter[0] === param) {
                return parameter[1]; // Trả về giá trị của tham số
            }
        }
        return null; // Trả về null nếu không tìm thấy tham số
    }
}
var group_product = {

    render_product_list_Home: function (group_product_id, div_location_render_data, view_name, skip, take, location_type) { //render data sản phẩm theo ngành hàng call ajax
        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/home/loadProductTopComponent',
            data: { group_product_id: group_product_id, page_index: skip, page_size: take, view_name: view_name },
            success: function (data) {
                $(div_location_render_data).html(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });
    },
}