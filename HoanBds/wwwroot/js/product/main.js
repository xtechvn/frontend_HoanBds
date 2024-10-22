
var id;
var priceCode;
$(document).ready(function () {

    let currentUrl = window.location.href;
    let params = currentUrl.split("?")[0].split("/").pop();
    if (/^\d+$/.test(params.split("-").pop())) {
        id = params.split("-").pop();
        product_constant.Type.forEach(item => {
            if (id == item.id) {
                $(".nameCate").html(item.name);
            }
        })
    }
    else {
        product_constant.Type.forEach(item => {
            if (params == item.path) {
                id = item.id;
                $(".nameCate").html(item.name);
            }
        })

    }

    const urlParams = new URLSearchParams(window.location.search);
    const pricecode = urlParams.get('pricecode');
    const districtcode = urlParams.get('district');
    const typecode = urlParams.get('type');
    var pageIndex = urlParams.get('page') == null ? 1 : urlParams.get('page');
    var pageSize = pageIndex == 1 ? 12 : pageIndex * 12;

    _product.LoadProduct(id, pricecode, pageIndex, districtcode, typecode, pageSize);

    if (id)
    {
        _product.LoadFilter(id);
        if (pricecode != null) {
            const priceElement = $('input[name="price"]' + "[value=" + pricecode + "]");
            priceElement.attr('checked', 'checked');
        }
        if (districtcode != null) {
            const districtElement = $('input[name="district"]' + "[value=" + districtcode + "]");
            districtElement.attr('checked', 'checked');
        }
        if (typecode != null) {
            const typeElement = $('input[name="type"]' + "[value=" + typecode + "]");
            typeElement.attr('checked', 'checked');
        }
    }


    $('input[name="price"]').on('change', function () {
        const selectedValue = $(this).val();

        // Thêm search param
        const newUrl = new URL(currentUrl);
        newUrl.searchParams.delete('page');
        // Kiểm tra xem pricecode đã tồn tại trong URL chưa
        const existingPricecode = newUrl.searchParams.get('pricecode');

        if ($(this).is(':checked')) {
            if (existingPricecode) {
                // Nếu đã tồn tại, cập nhật giá trị
                newUrl.searchParams.set('pricecode', selectedValue);
            } else {
                // Nếu chưa tồn tại, thêm mới
                newUrl.searchParams.append('pricecode', selectedValue);
            }
        }
        else
        {
            $(this).prop('checked', false);
            newUrl.searchParams.delete('pricecode');
        }

        // Chuyển hướng đến URL mới
        window.location.href = newUrl.toString();
    });
    $('input[name="type"]').on('change', function () {
        const selectedValue = $(this).val();

        // Thêm search param
        const newUrl = new URL(currentUrl);
        newUrl.searchParams.delete('page');
        // Kiểm tra xem type đã tồn tại trong URL chưa
        const existingPricecode = newUrl.searchParams.get('type');
        if ($(this).is(':checked')) {
            if (existingPricecode) {
                // Nếu đã tồn tại, cập nhật giá trị
                newUrl.searchParams.set('type', selectedValue);
            } else {
                // Nếu chưa tồn tại, thêm mới
                newUrl.searchParams.append('type', selectedValue);
            }
        }
        else
        {
            $(this).prop('checked', false);
            newUrl.searchParams.delete('type');
        }
        

        // Chuyển hướng đến URL mới
        window.location.href = newUrl.toString();
    });
    $('input[name="district"]').on('change', function () {
        const selectedValue = $(this).val();

        // Thêm search param
        const newUrl = new URL(currentUrl);
        newUrl.searchParams.delete('page');

        const existingPricecode = newUrl.searchParams.get('district');

        if ($(this).is(':checked')) {
            if (existingPricecode) {
                // Nếu đã tồn tại, cập nhật giá trị
                newUrl.searchParams.set('district', selectedValue);
            } else {
                // Nếu chưa tồn tại, thêm mới
                newUrl.searchParams.append('district', selectedValue);
            }
        }
        else
        {
            $(this).prop('checked', false);
            newUrl.searchParams.delete('district');
        }
        

        // Chuyển hướng đến URL mới
        window.location.href = newUrl.toString();
    });

    $(document.body).on('click', '.expand-products', function (e) {
        var total_item = $(".total_items").val();
        var total_page = Math.ceil(total_item / 12);

        const newUrl = new URL(currentUrl);
        const query_string = window.location.search;
        // Khởi tạo URLSearchParams để xử lý query string
        const url_params = new URLSearchParams(query_string);
        // Lấy giá trị của tham số 'page'
        var pageIndex = url_params.get('page') == null ? 1 : url_params.get('page');

        if (pageIndex == total_page) {
            $(".expand-products").remove();
        } else {
            const existingPage = newUrl.searchParams.get('page');

            if (existingPage) {
                // Nếu đã tồn tại, cập nhật giá trị
                newUrl.searchParams.set('page', parseInt(pageIndex) + 1);
            } else {
                // Nếu chưa tồn tại, thêm mới
                newUrl.searchParams.append('page', parseInt(pageIndex) + 1);
            }
            // Chuyển hướng đến URL mới
            window.location.href = newUrl.toString();
        }
    });
})


var _product =
{
    LoadFilter: function (id)
    {
        $("#product_filter").html('');
        if (id == 57) // Nhà phố
        {
            $("#product_filter").append(filter_html.price);
        }
        else if (id == 58) // CCMN
        {
            $("#product_filter").append(filter_html.price);
            $("#product_filter").append(filter_html.district);
        }
        else if (id == 59)
        {
            $("#product_filter").append(filter_html.type);
        }
    },
    LoadProduct: function (group_product_id, pricecode, pageIndex,districtcode,typecode, take)
    {
        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/san-pham/search',
            data: { _group_product_id: group_product_id, pricecode: pricecode, _page_index: pageIndex, districtcode: districtcode, typecode: typecode, _page_size: take, view_name: "~/Views/Shared/Components/Product/ProductSearchListViewComponent.cshtml" },
            success: function (data) {
                $("#list-product-search").html(data);
                var total_item = $(".total_items").val();
                var total_page = Math.ceil(total_item / 12);
                if (pageIndex == total_page) {
                    $(".expand-products").remove();
                }
                if (pageIndex >= 2) {
                    var active_index = (pageIndex - 1) * 12;
                    var active_item = $('.product_item:eq(' + active_index + ')');
                    if (active_item.length > 0) {
                        const offset = active_item.offset().top;

                        // Cuộn đến vị trí đó
                        $('html, body').animate({
                            scrollTop: offset
                        }, 500);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });
    }
}