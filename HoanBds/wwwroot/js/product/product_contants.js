var product_constant =
{
    Type: [
        { id: 58, path: 'nha-pho', name : 'Nhà phố'},
        { id: 69, path: 'dat' , name : 'Đất'},
        { id: 57, path: 'ccmn', name : 'Chung cư mini' },
    ],
}

var filter_html =
{
    price: `
    <div>
                            <div class="sidebarblog-title title_block">
                                <h2>
                                    Giá
                                </h2>
                            </div>
                            <ul class="sidebar-filter">
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="1" id="price-1" name="price" class="checkbox">
                                        <label for="price-1">
                                           <span>dưới 10 tỉ</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="2" id="price-2" name="price" class="checkbox">
                                        <label for="price-2">
                                           <span>10-20 tỷ</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="3" id="price-3" name="price" class="checkbox">
                                        <label for="price-3">
                                           <span>20-30 tỷ</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="4" id="price-4" name="price" class="checkbox">
                                        <label for="price-4">
                                           <span>trên 30 tỷ</span>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
    `,
    district: `
    <div>
                            <div class="sidebarblog-title title_block">
                                <h2>
                                    Quận
                                </h2>
                            </div>
                            <ul class="sidebar-filter">
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="67" id="district-1" name="district" class="checkbox">
                                        <label for="district-1">
                                           <span>Ba Đình</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="63" id="district-2" name="district" class="checkbox">
                                        <label for="district-2">
                                           <span>Hoàn Kiếm</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="68" id="district-3" name="district" class="checkbox">
                                        <label for="district-3">
                                           <span>Cầu Giấy</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="64" id="district-4" name="district" class="checkbox">
                                        <label for="district-4">
                                           <span>Đống Đa</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="65" id="district-5" name="district" class="checkbox">
                                        <label for="district-5">
                                           <span>Thanh Xuân</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="66" id="district-6" name="district" class="checkbox">
                                        <label for="district-6">
                                           <span>Từ Liêm</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="-1" id="district-7" name="district" class="checkbox">
                                        <label for="district-7">
                                           <span>Khác</span>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
    `,
    type: `
    <div>
                            <div class="sidebarblog-title title_block">
                                <h2>
                                    Phân loại
                                </h2>
                            </div>
                            <ul class="sidebar-filter">
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="70" id="type-1" name="type" class="checkbox">
                                        <label for="type-1">
                                           <span>Đất thổ cư</span>
                                        </label>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="69" id="type-2" name="type" class="checkbox">
                                        <label for="type-2">
                                           <span>Đất dự án</span>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
    `,
}