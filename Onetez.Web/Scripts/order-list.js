
(function () {
  eventLogs();
  refreshData();
})();

const time_wait = 10000;

function dateToddMM(date) {
  const month = date.getMonth() + 1;
  const MM = month < 10 ? `0${month}` : month;
  const dd = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

  return `${dd}/${MM}`;
}

function deleteItem(id) {
  if (confirm('Bạn có chắc muốn xóa đơn này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Order/DeleteOrder",
      data: { id },
      dataType: "json",
      success: function (res) {
        if (res.status) {
          showNotify(res.msg, true);
          $(`#row_${id}`).remove();
        }
        else
          showNotify(res.msg, false);
      }
    });
  }
}

function changeStauts(id) {
  const select = $(`#status_${id}`);
  const status = select.val();
  const parent = select.parent();

  $.ajax({
    type: "POST",
    url: "/APIv1/Order/ChangeStatus",
    data: { id, status },
    dataType: "json",
    success: function (res) {
      if (res.status === true) {
        parent.attr('class', `select is-small ${res.data.color}`);
      }
      else {
        showNotify(res.msg, false);
      }
    }
  });
}

function eventLogs() {
  $('.logs_list').find('.toggler').live("click", function () {
    const parent = $(this).parent();
    parent.toggleClass('show_all');
  });

  $('.ship_log').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('ship_log_', '');
      const note = this.textContent;
      const date = new Date();

      this.innerHTML = '';
      this.blur();

      const logs = $(`#ship_logs_${id}`);
      const logStr = `<div class="item tag is-link">
                    <span class="is_date">${dateToddMM(date)}</span>
                    <span>${note}</span>
                </div>`
      logs.prepend(logStr);

      $.ajax({
        type: "POST",
        url: "/APIv1/Order/ShipLog",
        data: { id, note },
        dataType: "json",
        success: function (res) {
          if (res.status === true) {
            if (res.data === 3) {
              logs.append(`<a class="toggler">Xem tất cả (${res.data})</a>`);
            }
            else if (res.data > 3) {
              $(`#ship_logs_${id} .toggler`).html(`Xem tất cả (${res.data})`);
            }
          }
          else {
            showNotify(res.msg, false);
          }
        }
      });
    }
  });

  $('.shop_log').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('shop_log_', '');
      const note = this.textContent;
      const date = new Date();

      this.innerHTML = '';
      this.blur();

      const logs = $(`#shop_logs_${id}`);
      const logStr = `<div class="item tag is-link">
                    <span class="is_date">${dateToddMM(date)}</span>
                    <span>${note}</span>
                </div>`
      logs.prepend(logStr);

      $.ajax({
        type: "POST",
        url: "/APIv1/Order/ShopLog",
        data: { id, note },
        dataType: "json",
        success: function (res) {
          if (res.status === true) {
            if (res.data === 5) {
              logs.append(`<a class="toggler">Xem tất cả (${res.data})</a>`);
            }
            else if (res.data > 5) {
              $(`#shop_logs_${id} .toggler`).html(`Xem tất cả (${res.data})`);
            }
          }
          else {
            showNotify(res.msg, false);
          }
        }
      });
    }
  });

  $('.complain').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('complain_', '');
      const value = this.innerText;

      this.innerHTML = value.length > 0 ? value : '';
      this.blur();

      $.ajax({
        type: "POST",
        url: "/APIv1/Order/Complain",
        data: { id, value },
        dataType: "json",
        success: function (res) {
          if (res.status === false) {
            showNotify(res.msg, false);
          }
        }
      });
    }
  });
}

function refreshData() {
  const today = new Date();
  const shop = $('#ddlShopId').val();
  $.ajax({
    type: "GET",
    url: "/APIv1/Order/GetList",
    data: { shop },
    dataType: "json",
    success: function (res) {
      $.each(res, function (i, item) {
        const rowOld = $(`#row_${item.Id}`);
        if (rowOld.length > 0) {
          const update = rowOld.data('update').toString();
          // Kiểm tra lần cập nhật cuối cùng
          if (update !== item.LastUpdate) {
            console.log(`Update row_${item.Id}`);

            // Cập thời gian xử lý
            rowOld.data('update', item.LastUpdate);

            // Tồn kho
            $(`#in_stock_${item.Id}`).html(item.ShipInStock);

            // Ship xử lý
            let shipLogs = '';
            for (let j = 0; j < item.ShipLogs.length; j++) {
              const log = item.ShipLogs[j];
              const date = convertToDate(log.updated_at);
              const is_today = dateToddMM(date) === dateToddMM(today);
              const is_manual = log.status_code === '000';
              let color = '';
              if (is_today && !is_manual)
                color = 'is-info';
              else if (is_today && is_manual)
                color = 'is-link';
              else if (is_manual)
                color = 'is-link is-light';

              const logNote = log.note.toLowerCase();
              $.each(color_list, function (i, tag) {
                if (logNote.indexOf(tag.name) !== -1)
                  color = tag.color;
              });

              shipLogs += `<div class="item tag ${color}" title="${log.status}">
                                    <span class="is_date">${dateToddMM(date)}</span>
                                    <span>${log.note}</span>
                                </div>`;
            }
            if (item.ShipLogs.length > 2) {
              shipLogs += `<a class="toggler">Xem tất cả (${item.ShipLogs.length})</a>`;
            }
            $(`#ship_logs_${item.Id}`).html(shipLogs);

            // Shop xử lý
            let shopLogs = '';
            for (let j = 0; j < item.ShopLogs.length; j++) {
              const log = item.ShopLogs[j];
              const date = convertToDate(log.date);
              const is_today = dateToddMM(date) === dateToddMM(today);
              let color = is_today ? 'is-link' : '';
              const logNote = log.note.toLowerCase();
              $.each(color_list, function (i, tag) {
                if (logNote.indexOf(tag.name) !== -1)
                  color = tag.color;
              });

              shopLogs += `<div class="item tag ${color}" title="Nhân viên: ${log.user}">
                                    <span class="is_date">${dateToddMM(date)}</span>
                                    <span>${log.note}</span>
                                </div>`;
            }
            if (item.ShopLogs.length > 4) {
              shopLogs += `<a class="toggler">Xem tất cả (${item.ShopLogs.length})</a>`;
            }
            $(`#shop_logs_${item.Id}`).html(shopLogs);

            // Trạng thái
            const select = $(`#status_${item.Id}`);
            select.val(item.Status.id);
            select.parent().attr('class', `select is-small ${item.Status.color}`);

            // Khiếu nại
            $(`#complain_${item.Id}`).html(item.Complain);
          }
        }
      });

      setTimeout(function () {
        console.log('REFRESH DATA', formatToFullDate(new Date()));
        refreshData();
      }, time_wait);
    }
  });
}

function selectOrder() {
  const cbList = $(`.js_cb_list[data-group="order"]`);
  let orderList = '';
  for (let i = 0; i < cbList.length; i++) {
    var item = cbList[i];
    if (item.checked) {
      const id = item.dataset.id;
      orderList += orderList.length === 0 ? id : ', ' + id;
    }
  }

  $('#copy_data').val(orderList);
  console.log('ORDERS: ' + $('#copy_data').val());
}

function xoaDonTrungLap() {
  if (confirm('Bạn có chắc muốn xóa các đơn hàng trùng lặp ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Order/XoaDonTrungLap",
      data: {},
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, true);
      }
    });
  }
}

function xoaDonThanhCong() {
  if (confirm('Bạn có chắc muốn xóa tất cả đơn đã thành công hay không ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Order/XoaDonThanhCong",
      data: {},
      dataType: "json",
      success: function (res) {
        if (res.status)
          showNotify(res.msg, true);
        else
          showNotify(res.msg, false);
      }
    });
  }
}