
(function () {
  $('#ads_product').keyup(function () {
    if (event.keyCode === 27) {
      this.value = '';
    }
  });
})();

function changeDay() {
  const shop = $('#ads_shop').val();
  const day = $('#ads_day').val();
  $.ajax({
    type: "GET",
    url: "/Ads/SheetProducts",
    data: { shop, day },
    dataType: "json",
    success: function (res) {
      let strItem = '';
      res.forEach(item => {
        if (item === '')
          strItem += `<option value="Sản phẩm không xác định" />`;
        else
          strItem += `<option value="${item}" />`;
      });
      $('#products').html(strItem);
    }
  });
}

function updateItem() {
  const id = 0;
  const day = $('#ads_day').val();
  const product = $('#ads_product').val();
  const cost = $('#ads_cost').val();
  const rate = $('#ads_rate').val();
  const shop = $('#ads_shop').val();

  if (checkRequired('form_edit')) {
    $.ajax({
      type: "POST",
      url: "/Ads/UpdateItem",
      data: { id, day, product, cost, rate, shop },
      dataType: "json",
      success: function (res) {
        if (res.status) {
          $('#ads_product').val('');
          $('#ads_cost').val('');
          $('#data_list').prepend(`
            <tr id="row_${res.id}">
              <td>${res.day}</td>
              <td>${res.product}</td>
              <td>${res.cost}</td>
              <td>${res.rate}</td>
              <td>${res.money}</td>
              <td align="center">
                <a onclick="deleteItem('${res.id}')" class="delete"></a>
              </td>
            </tr>`);
        }
        else
          showNotify(res.msg, false);
      }
    });
  }

}

function deleteItem(id) {
  if (confirm('Bạn có chắc muốn xóa mục này ?')) {
    $.ajax({
      type: "POST",
      url: "/Ads/Delete",
      data: { id },
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, res.status);
        if (res.status)
          $(`#row_${id}`).remove();
      }
    });
  }
}