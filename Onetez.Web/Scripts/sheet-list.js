(function () {
  $('[contentEditable="true"]').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('note_', '');
      const note = this.innerText;

      this.innerHTML = note.length > 0 ? note : '';
      //this.blur();

      $.ajax({
        type: "POST",
        url: "/APIv1/Sheet/ChangeNote",
        data: { id, note },
        dataType: "json",
        success: function (res) {
          showNotify(res.msg, res.status);
        }
      });
    }
  });

  $('[data-revenue]').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('revenue_', '');
      const revenue = this.value;

      $.ajax({
        type: "POST",
        url: "/APIv1/Sheet/ChangeRevenue",
        data: { id, revenue },
        dataType: "json",
        success: function (res) {
          showNotify(res.msg, res.status);
        }
      });
    }
  });

  $('[data-address]').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('address_', '');
      changeAddress(id);
    }
  });

  $('[data-location] input').keyup(function () {
    if (event.keyCode === 13) {
      const id = this.id.replace('baragay_', '').replace('province_', '').replace('city_', '');
      changeLocation(id);
    }
  });

  $('#product').keyup(function () {
    if (event.keyCode === 27) {
      this.value = '';
    }
  });

  $('#statusId').change(function () {
    if (this.value === '2' || this.value === '0')
      $('#userId').val('');
  });

  $('#list_sheet .dropdown .button').click(function () {
    const id = $(this).parents('tr').attr('id');
    $(`tr:not(#${id}) .dropdown`).removeClass('is-active');
    $(this).parents('.dropdown').toggleClass('is-active');
  });
})();

function refreshData(reload) {
  $('#btn_refresh').addClass('is-loading');
  const id = getParameterByName('shop');
  $.ajax({
    type: "GET",
    url: "/APIv1/Sheet/RefreshData",
    data: { id },
    dataType: "json",
    error: function () {
      showNotify('Đã xãy ra lội không xác định', false);
    },
    success: function (res) {
      $('#btn_refresh').removeClass('is-loading');
      if (res.status)
      {
        if (reload)
          location.reload();
        else
          showNotify(res.message, true);
      }
      else if (res.message.length > 0)
        showNotify(res.message, false);
      //else 
      //  showNotify('Không có dữ liệu mới', false);
    }
  });
}

function createOrder(id) {
  const btn = $(`#status_${id}`)
  btn.addClass('is-loading');
  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/CreateOrder",
    data: { id },
    dataType: "json",
    success: function (res) {
      btn.removeClass('is-loading');
      if (res.status === true) {
        $(`#row_${id} [data-status]`).html(`
           <span class="button is-small is-success">#${res.data.id}</span>`);
      }
      else {
        if (btn.hasClass('button')) {
          btn.addClass('is-danger');
          btn.html('Đã có lỗi');
        }
        else {
          btn.attr('title', 'Đã có lỗi');
          btn.html(`<i class="s20 feather icon-alert-triangle"></i>`);
          showNotify(res.message, false);
        }
        $(`#msg_${id}`).html(res.message);
        if (res.error.indexOf('message') !== -1) {
          const error = JSON.parse(res.error);
          $(`#msg_${id}`).html(error.message);
        }
      }
    }
  });
}

function createOrderEmpty(id) {
  const btn = $(`#empty_${id}`)
  btn.addClass('is-loading');
  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/CreateOrder",
    data: { id, createOrderEmpty: true },
    dataType: "json",
    success: function (res) {
      btn.removeClass('is-loading');
      if (res.status === true) {
        $(`#row_${id} [data-status]`).html(`<span class="button is-small is-success">#${res.data.id}</span>`);
      }
      else {
        $(`#row_${id} [data-status]`).html(`<span class="button is-small is-danger">Đã có lỗi</span>`);
        $(`#msg_${id}`).html(res.message);
        showNotify(res.message, false);
        if (res.error.indexOf('message') !== -1) {
          const error = JSON.parse(res.error);
          $(`#msg_${id}`).html(error.message);
        }
      }
    }
  });
}

function createMutiOrder(start) {
  const list = $(`#list_sheet .action-status`);
  if (start < list.length) {
    const item = list[start];
    const id = item.id.replace('status_', '');
    const btn = $(`#status_${id}`)
    btn.addClass('is-loading');
    $.ajax({
      type: "POST",
      url: "/APIv1/Sheet/CreateOrder",
      data: { id },
      dataType: "json",
      success: function (res) {
        btn.removeClass('is-loading');
        if (res.status === true) {
          $(`#row_${id} [data-status]`).html(`<span class="action-status button is-small is-success">#${res.data.id}</span>`);
        }
        else {
          if (btn.hasClass('button')) {
            btn.addClass('is-danger');
            btn.html('Đã có lỗi');
          }
          else {
            btn.attr('title', 'Đã có lỗi');
            btn.html(`<i class="s20 feather icon-alert-triangle"></i>`);
          }
          $(`#msg_${id}`).html(res.message);
          if (res.error.indexOf('message') !== -1) {
            const error = JSON.parse(res.error);
            $(`#msg_${id}`).html(error.message);
          }
        }
        createMutiOrder(start + 1);
      }
    });
  }
  else {
    console.log('CREATE ORDER FINISH', new Date());
    refreshData();
  }
}

function tryOrderError(start) {
  const list = $(`#list_sheet .action-status`);
  if (start < list.length) {
    const item = list[start];
    const id = item.id.replace('status_', '');
    const btn = $(`#status_${id}`)
    btn.addClass('is-loading');
    $.ajax({
      type: "POST",
      url: "/APIv1/Sheet/CreateOrder",
      data: { id },
      dataType: "json",
      success: function (res) {
        btn.removeClass('is-loading');
        if (res.status === true) {
          $(`#row_${id} [data-status]`).html(`<span class="action-status button is-small is-success">#${res.data.id}</span>`);
        }
        else {
          $(`#row_${id} [data-status]`).html(`<span class="action-status button is-small is-danger">Đã có lỗi</span>`);
          $(`#msg_${id}`).html(res.message);
          if (res.error.indexOf('message') !== -1) {
            const error = JSON.parse(res.error);
            $(`#msg_${id}`).html(error.message);
          }
        }
        tryOrderError(start + 1);
      }
    });
  }
}

function updateOrder(id) {
  const btn = $(`#status_${id}`)
  btn.addClass('is-loading');
  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/UpdateOrder",
    data: { id },
    dataType: "json",
    success: function (res) {
      btn.removeClass('is-loading');
      console.log(res);
    }
  });
}

function deleteItem(id) {
  if (confirm('Bạn có chắc muốn hủy data này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Sheet/Delete",
      data: { id },
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, res.status);
        if (res.status) {
          $(`#row_${id}`).remove();
        }
      }
    });
  }
}

function removeItem(id) {
  if (confirm('Bạn có chắc muốn xóa data này khỏi phần mềm ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Sheet/Remove",
      data: { id },
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, res.status);
        if (res.status) {
          $(`#row_${id}`).remove();
        }
      }
    });
  }
}

function restoreItem(id) {
  if (confirm('Bạn có chắc muốn xóa khôi phục data này ?')) {
    $.ajax({
      type: "POST",
      url: "/APIv1/Sheet/Restore",
      data: { id },
      dataType: "json",
      success: function (res) {
        showNotify(res.msg, res.status);
        if (res.status) {
          $(`#row_${id}`).remove();
        }
      }
    });
  }
}

function searchSheet() {
  const tab = getParameterByName('tab');
  const shop = getParameterByName('shop');
  const phone = $('#filterPhone').val().replace(/ /g, '-');
  const product = $('#filterProduct').val().replace(/ /g, '-');

  location.href = `/sheet/list?tab=${tab}&shop=${shop}&phone=${phone}&product=${product}`;
}

function changeUserProcess() {
  let users = [];
  const listUser = $('input[data-group="user"]');
  for (let i = 0; i < listUser.length; i++) {
    const item = listUser[i];
    if (item.checked)
      users.push(item.dataset.id);
  }

  let sheets = [];
  const listSheet = $('.js_cb_list');
  for (let i = 0; i < listSheet.length; i++) {
    const item = listSheet[i];
    if (item.checked)
      sheets.push(item.dataset.id);
  }

  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/UserProcess",
    data: { users, sheets },
    dataType: "json",
    success: function (res) {
      showNotify(res.msg, res.status);
      if (res.status) {
        for (var i = 0; i < sheets.length; i++)
          $(`#row_${sheets[i]}`).remove();
      }
    }
  });
}

function changeProcess(id) {
  const process = parseInt($(`#process_${id}`).val());
  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/ChangeProcess",
    data: { id, process },
    dataType: "json",
    success: function (res) {
      if (res.status) {
        $(`#logs_${id}`).html(`<div class="dropdown-item is-size-7 px-2">${res.msg}</div>`);
        if (process === 2) {
          $(`#delete_${id}`).addClass('is-hidden');
          $(`#status_${id}`).removeClass('is-hidden');
        }
        else if (process === 3 ||process === 4 || process > 20) {
          $(`#delete_${id}`).removeClass('is-hidden');
          $(`#status_${id}`).addClass('is-hidden');
        }
        else {
          $(`#delete_${id}`).addClass('is-hidden');
          $(`#status_${id}`).addClass('is-hidden');
        }
      }
      else {
        showNotify(res.msg, false);
      }
    }
  });
}

function changeCategory(id, index) {
  const category = $(`#category${index}_${id}`).val();
  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/ChangeCategory",
    data: { id, category, index },
    dataType: "json",
    success: function (res) {
      if (!res.status)
        showNotify(res.msg, false);
    }
  });
}

function changeLocation(id) {
  const state = $(`#state_${id}`).val();
  const city = $(`#city_${id}`).val();
  const locality = $(`#locality_${id}`).val();
  const address = $(`#address_${id}`).val();
  const pin = $(`#pin_${id}`).val();

  $.ajax({
    type: "POST",
    url: "/APIv1/Sheet/ChangeLocation",
    data: { id, state, city, locality, address, pin },
    dataType: "json",
    success: function (res) {
      showNotify(res.msg, res.status);
    }
  });
}