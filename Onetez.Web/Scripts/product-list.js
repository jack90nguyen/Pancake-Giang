
$('#SheetCode').focus();

function deleteItem(id) {
  if (confirm('Bạn có chắc muốn xóa sản phẩm này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Product/Delete",
      data: { id },
      dataType: "json",
      success: function (res) {
        if (res.status === true) {
          $(`#row_${id}`).remove();
          $(`.child_${id}`).remove();
        }
        else {
          showNotify(res.msg, false);
        }
      }
    });
  }
}

function duplicateItem(id) {
  if (confirm('Bạn có chắc muốn nhân bản sản phẩm này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Product/Duplicate",
      data: { id },
      dataType: "json",
      success: function (res) {
        if (res.status === true) {
          location.reload();
        }
        else {
          showNotify(res.msg, false);
        }
      }
    });
  }
}

function popupCombo(id) {
  $.ajax({
    type: "GET",
    url: "/APIv1/Product/GetCombo",
    data: { id },
    dataType: "json",
    success: function (res) {
      $('#popup_combo').addClass('is-active');
      $('#combo_id').val(res.id);
      $('#combo_sheetcode').val(res.name);
      $('#combo_discount').val(res.price);
      let strItems = '';
      res.childs.forEach(item => {
        strItems += renderItemCombo(item);
      })
      $('#popup_combo tbody').html(strItems);
    }
  });
}

function updateCombo() {
  $('#popup_combo .btn_save').addClass('is-loading');
  var listSave = $('#popup_combo tbody .icon_save');
  for (var i = 0; i < listSave.length; i++) {
    var item = listSave[i];
    item.click();
  }
  setTimeout(function () {
    const id = $('#combo_id').val();
    const sheetcode = $('#combo_sheetcode').val();
    const discount = $('#combo_discount').val();
    const shop_id = $('#ddlShopId').val();
    $.ajax({
      type: "POST",
      url: "/APIv1/Product/UpdateCombo",
      data: { id, sheetcode, discount, shop_id },
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, res.status);
        if (res.status)
          location.reload();
      }
    });
  }, 1000);
}

$('#combo_search').keyup(function () {
  const key = $('#combo_search').val();
  if (key.length > 0) {
    if (event.keyCode === 38) {
      ResultSelectUp();
    }
    else if (event.keyCode === 40) {
      ResultSelectDown();
    }
    else if (event.keyCode === 13) {
      ResultSelectThis();
    }
    else {
      let strItems = '';
      list_product.forEach(item => {
        if (item.key.indexOf(key) !== -1) {
          strItems += `<li class="rsitem" onclick="selectProduct('${item.id}', '${item.name}')">
                     <div class="columns is-mobile is-gapless">
                        <span class="column">${item.name}</span>
                        <span class="column is-2 has-text-grey">MSP: ${item.msp}</span>
                        <span class="column is-2 has-text-grey">MMM: ${item.mmm}</span>
                        <span class="column is-1 has-text-danger has-text-right">${item.price}</span>
                     </div>
                  </li>`;
        }
      });
      $('#combo_suggest').html(strItems);
      $('#combo_suggest').show();
    }
  }
  else {
    $('#combo_suggest').hide();
    $('#combo_suggest').html('');
  }
});

function selectProduct(id, name) {
  $('#combo_suggest').hide();
  $('#combo_suggest').html('');
  $('#combo_product').val(id);
  $('#combo_search').val(name);
}

function addToCombo() {
  const parent_id = $('#combo_id').val();
  const product_id = $('#combo_product').val();
  $.ajax({
    type: "POST",
    url: "/APIv1/Product/AddToCombo",
    data: { parent_id, product_id },
    dataType: "json",
    success: function (res) {
      if (res.status) {
        $('#popup_combo tbody').prepend(renderItemCombo(res.data));
        $('#combo_product').val('');
        $('#combo_search').val('');
      }
      else {
        showNotify(res.msg, false);
      }
    }
  });
}

function updateItemCombo(id) {
  const price = $(`#pro_${id} .is_price`).val();
  const quantity = $(`#pro_${id} .is_quantity`).val();
  $.ajax({
    type: "POST",
    url: "/APIv1/Product/UpdateItemCombo",
    data: { id, price, quantity },
    dataType: "json",
    success: function (res) {
      showNotify(res.msg, res.status);
    }
  });
}

function deleteItemCombo(id) {
  if (confirm('Bạn có chắc muốn xóa sản phẩm này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Product/Delete",
      data: { id },
      dataType: "json",
      success: function (res) {
        if (res.status === true) {
          $(`#pro_${id}`).remove();
        }
        else {
          showNotify(res.msg, false);
        }
      }
    });
  }
}

function renderItemCombo(item) {
  return `<tr id="pro_${item.id}">
      <td>${item.name}</td>
      <td>${item.msp}</td>
      <td>${item.mmm}</td>
      <td><input class="input is-small is_price" type="number" style="width: 100px;" value="${item.price}"></td>
      <td><input class="input is-small is_quantity" type="number" style="width: 60px;" value="${item.quantity}"></td>
      <td align="center">
         <a class="icon icon_save" onclick="updateItemCombo('${item.id}')">
            <i class="feather icon-save"></i>
         </a>
         <a class="icon has-text-danger" onclick="deleteItemCombo('${item.id}')">
            <i class="feather icon-x"></i>
         </a>
      </td>
   </tr>`;
}

function editItem(id) {
  if (id === '') {
    $('#popup_product').addClass('is-active');
    $('#product_id').val('');
    $('#popup_product .input').val('');
    $('#popup_product .input[type="number"]').val('0');
  }
  else {
    $.ajax({
      type: "GET",
      url: "/APIv1/Product/GetProduct",
      data: { id },
      dataType: "json",
      success: function (res) {
        if (res !== null) {
          $('#popup_product').addClass('is-active');
          $('#product_id').val(res.id);
          $('#popup_product [data-code]').val(res.code);
          $('#popup_product [data-name]').val(res.name);
          $('#popup_product [data-product]').val(res.product);
          $('#popup_product [data-display]').val(res.display);
          $('#popup_product [data-size]').val(res.size);
          $('#popup_product [data-color]').val(res.color);
          $('#popup_product [data-price]').val(res.price);
          $('#popup_product [data-discount]').val(res.discount);
          $('#popup_product [data-quantity]').val(res.quantity);
          $('#popup_product [data-weight]').val(res.weight);
        }
        else
          showNotify('Sản phẩm không tồn tại !', false);
      }
    });
  }
}

function updateProduct() {
  const id = $('#product_id').val();
  const shop = $('#product_shop').val();
  const code = $('#popup_product [data-code]').val();
  const name = $('#popup_product [data-name]').val();
  const product = $('#popup_product [data-product]').val();
  const display = $('#popup_product [data-display]').val();
  const size = $('#popup_product [data-size]').val();
  const color = $('#popup_product [data-color]').val();
  const price = $('#popup_product [data-price]').val();
  const discount = $('#popup_product [data-discount]').val();
  const quantity = $('#popup_product [data-quantity]').val();
  const weight = $('#popup_product [data-weight]').val();
  $.ajax({
    type: "POST",
    url: "/APIv1/Product/UpdateProduct",
    data: { id, code, name, product, display, size, color, price, discount, quantity, weight, shop },
    dataType: "json",
    success: function (res) {
      $('#popup_product').removeClass('is-active');
      const strItem = `
        <td>${code}</td>
        <td>
          <a onclick="editItem('${res.id}')">${name}</a>
        </td>
        <td>${product}</td>
        <td>${display}</td>
        <td>${size}</td>
        <td>${color}</td>
        <td><span class="has-text-danger">${price}</span></td>
        <td><span class="has-text-danger">${discount}</span></td>
        <td>${quantity}</td>
        <td>${weight}</td>
        <td class="has-text-centered">
          <div class="dropdown is-hoverable is-right is-menu">
            <div class="dropdown-trigger">
              <span class="icon">
                <i class="s18 feather icon-more-vertical"></i>
              </span>
            </div>
            <div class="dropdown-menu">
              <div class="dropdown-content">
                  <a class="dropdown-item has-icon" onclick="editItem('${res.id}')">
                    <span class="icon"><i class="feather icon-edit"></i></span>
                    <span>Chỉnh sửa</span>
                  </a>
                <a class="dropdown-item has-icon" onclick="duplicateItem('${res.id}')">
                  <span class="icon"><i class="feather icon-copy"></i></span>
                  <span>Nhân bản</span>
                </a>
                <hr class="dropdown-divider">
                <a class="dropdown-item has-text-danger has-icon" onclick="deleteItem('${res.id}')">
                  <span class="icon"><i class="feather icon-trash-2"></i></span>
                  <span>Xóa</span>
                </a>
              </div>
            </div>
          </div>
        </td>`;
      if (id === '')
        $('#list_sheet tbody').prepend(`<tr id="row_${res.id}">${strItem}</tr>`);
      else
        $(`#row_${res.id}`).html(strItem);
      processProduct(shop);
    }
  });
}

function processProduct(shop) {
  $.ajax({
    type: "GET",
    url: "/apiv1/sheet/ProcessProduct",
    data: { id: shop },
    dataType: "json",
    success: function (res) {
      console.log('total: ' + res.total, 'known: ' + res.known, 'unknown: ' + res.unknown);
    }
  });
}