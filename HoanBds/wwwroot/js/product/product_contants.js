var product_constant =
{
    Type: [
        { id: 58, path: 'nha-pho', name : 'Nhà phố'},
        { id: 60, path: 'dat' , name : 'Đất'},
        { id: 59, path: 'ccmn', name : 'Chung cư mini' },
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
                                        <label for="price-1"></label>
                                        <span>dưới 10 tỉ</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="2" id="price-2" name="price" class="checkbox">
                                        <label for="price-2"></label>
                                        <span>10-20 tỷ</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="3" id="price-3" name="price" class="checkbox">
                                        <label for="price-3"></label>
                                        <span>20-30 tỷ</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="4" id="price-4" name="price" class="checkbox">
                                        <label for="price-4"></label>
                                        <span>trên 30 tỷ</span>
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
                                        <input type="checkbox" value="87" id="district-1" name="district" class="checkbox">
                                        <label for="district-1"></label>
                                        <span>Ba Đình</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="83" id="district-2" name="district" class="checkbox">
                                        <label for="district-2"></label>
                                        <span>Hoàn Kiếm</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="88" id="district-2" name="district" class="checkbox">
                                        <label for="district-2"></label>
                                        <span>Cầu Giấy</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="84" id="district-3" name="district" class="checkbox">
                                        <label for="district-3"></label>
                                        <span>Đống Đa</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="85" id="district-4" name="district" class="checkbox">
                                        <label for="district-4"></label>
                                        <span>Thanh Xuân</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="86" id="district-5" name="district" class="checkbox">
                                        <label for="district-5"></label>
                                        <span>Từ Liêm</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="-1" id="district-6" name="district" class="checkbox">
                                        <label for="district-6"></label>
                                        <span>Khác</span>
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
                                        <label for="type-1"></label>
                                        <span>Đất thổ cư</span>
                                    </div>
                                </li>
                                <li>
                                    <div class="box-checkbox">
                                        <input type="checkbox" value="69" id="type-2" name="type" class="checkbox">
                                        <label for="type-2"></label>
                                        <span>Đất dự án</span>
                                    </div>
                                </li>
                            </ul>
                        </div>
    `,
}