function setCookie(key, value) {
  var expires = new Date();
  expires.setTime(expires.getTime() + (1 * 24 * 60 * 60 * 1000));
  document.cookie = key + '=' + value + ';expires=' + expires.toUTCString() + ';path=/';
}

function getCookie(key) {
  var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
  return keyValue ? keyValue[2] : null;
}

function getParameterByName(name, url) {
  if (!url) url = window.location.href;
  name = name.replace(/[\[\]]/g, "\\$&");
  var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
    results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, " "));
}

const isMobile = window.innerWidth < 768 ? true : false;

function hideNotify(id) {
  if (!id) {
    $('#notify').html('');
  }
  else {
    $('#notify #' + id).remove();
  }
}

function showNotify(content, isSuccess) {
  const id = new Date().valueOf();
  const status = isSuccess === true ? 'is-success' : 'is-danger';

  $('#notify').html(`
    <div id="${id}" class="notification is-light ${status}">
      <a class="delete" onclick="hideNotify(${id})"></a> ${content}
    </div>`);

  setTimeout(function () {
    hideNotify(id);
  }, 10000);
}

function changeUrlAfterNotify() {
  var url_full = location.href;
  var index = url_full.length;
  if (url_full.indexOf('msgg') !== -1)
    index = url_full.indexOf('msgg') - 1;
  else if (url_full.indexOf('msgr') !== -1)
    index = url_full.indexOf('msgr') - 1;
  var url_new = url_full.substr(0, index);
  history.pushState(null, 0, url_new);
}

function ajaxUpload(fileId, folder, preview) {
  var data = new FormData();
  var files = $(`#${fileId}`).get(0).files;
  if (files.length > 0) {
    data.append("AjaxUploaded", files[0]);

    $.ajax({
      type: "POST",
      url: `/Upload/Index?folder=${folder}`,
      contentType: false,
      processData: false,
      data: data,
      success: function (res) {
        if (res.length > 0) {
          const item = res[0];
          $(`#${fileId}`).val(fileUrl);
          if (preview === true) {
            const view = $(`#preview_${fileId}`);
            view.attr('href', item.link);
            view.css('background-image', "url('" + item.link + "')");
          }
        }
        else {
          alert("Can't upload file :(");
        }
      }
    });
  }
}

function ajaxUploadMuti(fileId, folder) {
  var data = new FormData();
  var files = $(`#${fileId}`).get(0).files;
  if (files.length > 0) {
    for (let i = 0; i < files.length; i++) {
      data.append("AjaxUploaded" + i, files[i]);
    }

    $.ajax({
      type: "POST",
      url: `/Upload/Index?folder=${folder}`,
      contentType: false,
      processData: false,
      data: data,
      success: function (res) {
        ajaxUploadMutiCallback(res)
      }
    });
  }
}

function ajaxUploadMutiCallback(images) {
  //Viết hàm khác để ghi đè hàm này
  $.each(images, function (i, item) {
    console.log(item.id, item.name, item.link);
  });
}

function getFileFormat(file_name) {
  const index_dot = file_name.lastIndexOf('.');
  if (index_dot > 0)
    return file_name.substring(index_dot + 1);
  else
    return '?';
}

function formatToMoney(number) {
  if (number !== null)
    return number.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
  else
    return '';
}

function formatToNumber(str) {
  str = str.toString().replace(/,/g, '').replace('VND', '');
  let num = parseFloat(str);
  if (isNaN(num))
    return 0;
  else
    return num;
}

function formatToISODay(date) {
  const month = date.getMonth() + 1;
  const yyyy = date.getFullYear();
  const MM = month < 10 ? `0${month}` : month;
  const dd = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();

  return `${yyyy}-${MM}-${dd}`;
}

function formatToISOTime(date) {
  const HH = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours();
  const mm = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();
  const ss = date.getSeconds() < 10 ? `0${date.getSeconds()}` : date.getSeconds();

  return `${HH}:${mm}:${ss}`;
}

function formatToFullDate(date) {
  return `${formatToISODay(date)}, ${formatToISOTime(date)}`;
}

function formatTimestampToDate(timestamp) {
  return formatToFullDate(convertToDate(timestamp));
}

function convertToDate(timestamp) {
  var time = parseInt(timestamp.toString().replace('/Date(', '').replace(')/', ''));
  return new Date(time);
}

function DaysBetween(date1, date2) {
  var ONE_DAY = 1000 * 60 * 60 * 24;
  // Convert both dates to milliseconds
  var date1_ms = date1.getTime();
  var date2_ms = date2.getTime();
  // Calculate the difference in milliseconds
  var difference_ms = Math.abs(date1_ms - date2_ms);
  // Convert back to days and return
  return Math.round(difference_ms / ONE_DAY);
}

function HideMsg() {
  $('#msg').html("");
}

function MsgRed(msg) {
  $('#msg').html("<div class='msg-red'>" + msg + "</div>");
  setTimeout(function () {
    HideMsg();
  }, 5000);
  window.addEventListener("keydown", function (e) {
    if (e.keyCode === 27) {
      HideMsg();
    }
  });
}

function MsgGreen(msg) {
  $('#msg').html("<div class='msg-green'>" + msg + "</div>");
  setTimeout(function () {
    HideMsg();
  }, 5000);
  window.addEventListener("keydown", function (e) {
    if (e.keyCode === 27) {
      HideMsg();
    }
  });
}

function MsgLoading() {
  $('body').prepend('<div id="loading"><span>Đang tải...</span></div>');
}

function HideLoading() {
  $('#loading').remove();
}

function ReloadPage() {
  MsgLoading();
  setTimeout(function () {
    location.reload();
  }, 500);
}

function disableEnterSubmit(formid) {
  $('#' + formid).on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
      if (e.srcElement.className.indexOf("disEnter") !== -1) {
        e.preventDefault();
        return false;
      }
    }
  });
}

function MoveHtml(idA, idB) {
  var itemA = $('#row_' + idA);
  var itemB = $('#row_' + idB);
  var itemC = itemA.html();
  itemA.addClass('move');
  itemA.html(itemB.html());
  itemB.html(itemC);
  itemA.attr('id', 'row_' + idB);
  itemB.attr('id', 'row_' + idA);
  setTimeout(function () {
    itemA.removeClass('move');
  }, 1000);
}

function ClosePopup(id) {
  if (!id) id = 'popup';
  var popup = $('#' + id);
  if (popup.data('edited') === true) {
    location.reload();
  }
  else {
    popup.fadeOut();
    if (popup.attr('class').indexOf("popupForm") === -1) {
      setTimeout(function () {
        popup.remove();
      }, 1000);
    }
  }
}

function closeModal(id) {
  $('#' + id).removeClass('is-active');
}

function OpenPopup(content) {
  $('body').append(content);
  window.addEventListener("keydown", function (e) {
    if (e.keyCode === 27) {
      ClosePopup();
    }
  });
}


function closeGallery() {
  var popup = $('#gallery__bg');
  popup.fadeOut();
  setTimeout(function () {
    popup.remove();
  }, 500);
}

function galleryPopup(img, group) {
  let mh = window.innerHeight - 60;
  let mw = window.innerWidth - 60;
  let content = '<div id="gallery__bg">';
  content += '<div class="gallery__popup">';
  content += '<a class="gallery__close" onclick="closeGallery()">X</a>';
  content += '<img class="gallery__img" style="max-height: ' + mh + 'px; max-width: ' + mw + 'px;" src="' + img + '" alt="IMG" />';
  if (group !== '' && group !== undefined) {
    content += '<a onclick="galleryNavi(false, \'' + group + '\')" class="gallery__prev"></a>';
    content += '<a onclick="galleryNavi(true, \'' + group + '\')" class="gallery__next"></a>';
  }
  content += '</div>';
  content += '</div>';

  $('body').append(content);

  if (group !== '' && group !== undefined) {
    window.addEventListener("keydown", function (e) {
      if (e.keyCode === 39) {
        galleryNavi(true, group);
      }
      else if (e.keyCode === 37) {
        galleryNavi(false, group);
      }
      else if (e.keyCode === 27) {
        closeGallery();
      }
    });

    //document.getElementById('gallery__bg').addEventListener('swiped-left', function (e) {
    //    galleryNavi(true, group);
    //});
    //document.getElementById('gallery__bg').addEventListener('swiped-right', function (e) {
    //    galleryNavi(false, group);
    //});
    //document.getElementById('gallery__bg').addEventListener('swiped-down', function (e) {
    //    closeGallery();
    //});
  }
  else {
    window.addEventListener("keydown", function (e) {
      if (e.keyCode === 27) {
        closeGallery();
      }
    });
    document.getElementById('gallery__bg').addEventListener('swiped-down', function (e) {
      closeGallery();
    });
  }
}

function galleryNavi(next, group) {
  let list = $('.js_zoom[data-group="' + group + '"]');
  let view_img = $('.gallery__img');
  let view_src = view_img.attr("src");
  for (let i = 0; i < list.length; i++) {
    let image = list[i];
    if (image.attributes.href !== undefined) {
      if (image.attributes.href.value.indexOf(view_src) !== -1) {
        let j = 0;
        if (next)
          j = i < list.length - 1 ? i + 1 : 0;
        else
          j = i > 0 ? i - 1 : list.length - 1;
        view_img.attr("src", list[j].attributes.href.value);
        break;
      }
    }
    else {
      view_img.attr("src", list[0].attributes.href.value);
    }
  }
}

function runGallery() {
  $('.js_zoom').click(function (e) {
    e.preventDefault();
    var img = $(this).attr('href');
    var group = $(this).data('group');
    galleryPopup(img, group);
  });
}


function checkBoxSelectAll() {
  $('.js_cb_all').click(function (e) {
    const group = $(this).data('group');
    const cbList = $(`.js_cb_list[data-group="${group}"]`);

    if ($(this).is(':checked') === true) {
      cbList.prop('checked', true);
    }
    else {
      cbList.prop('checked', false);
    }
  });
}


function ResultSelectUp() {
  var rsItems = document.getElementsByClassName("rsitem");
  var sltIndex = rsItems.length - 1;
  for (var i = 0; i < rsItems.length; i++) {
    var rsClass = rsItems[i].getAttribute('class');
    if (rsClass.indexOf("seleted") !== -1) {
      rsItems[i].className = "rsitem";
      sltIndex = i - 1;
      break;
    }
  }
  if (sltIndex >= 0)
    rsItems[sltIndex].className = "rsitem seleted";
  else
    rsItems[rsItems.length - 1].className = "rsitem seleted";
}

function ResultSelectDown() {
  var rsItems = document.getElementsByClassName("rsitem");
  var sltIndex = 0;
  for (var i = 0; i < rsItems.length; i++) {
    var rsClass = rsItems[i].getAttribute('class');
    if (rsClass.indexOf("seleted") !== -1) {
      rsItems[i].className = "rsitem";
      sltIndex = i + 1;
      break;
    }
  }
  if (sltIndex < rsItems.length)
    rsItems[sltIndex].className = "rsitem seleted";
  else
    rsItems[0].className = "rsitem seleted";
}

function ResultSelectThis() {
  var rsItems = document.getElementsByClassName("rsitem");
  for (var i = 0; i < rsItems.length; i++) {
    var rsClass = rsItems[i].getAttribute('class');
    if (rsClass.indexOf("seleted") !== -1) {
      rsItems[i].click();
    }
  }
}

function setWidthTable() {
  var total_width = 0;
  var list_th = $('.table-scroll tr th');
  for (var i = 0; i < list_th.length; i++) {
    var item = list_th[i];
    var width = item.getAttribute('width').replace('px', '');
    total_width += width === 'auto' ? 300 : parseInt(width);
  }
  $('.table-scroll').css('width', total_width + 'px');
}

function tableMobile() {
  var tables = $('.table');
  for (let i = 0; i < tables.length; i++) {
    var table = tables[i];
    var thead = table.children[0].children[0].children;
    var tbody = table.children[1].children;
    for (let tr = 0; tr < tbody.length; tr++) {
      var trItems = tbody[tr].children;
      for (let td = 0; td < trItems.length; td++) {

        if (thead[td].innerHTML !== '') {
          const label = document.createElement("label");
          label.innerHTML = thead[td].innerHTML;
          label.className = 'td-label';

          trItems[td].prepend(label);
        }

      }
    }
  }
}

function setDataWidth() {
  var list_width = $('[data-width]');
  for (var i = 0; i < list_width.length; i++) {
    var item = list_width[i];
    item.style.width = item.dataset.width;
  }
}

function checkRequired(formId) {
  var status = true;
  var listRequired = $('#' + formId + ' [data-required]');
  for (var i = 0; i < listRequired.length; i++) {
    var item = listRequired[i];
    if (!!item.value === false) {
      status = false;
      item.className = item.className + ' is-danger';
    }
    else {
      item.className = item.className.replace('is-danger', '');
    }
  }
  return status;
}

function checkEmpty(inputId) {
  var input = $('#' + inputId);
  if (!!input.val() === false) {
    input.addClass('is-danger');
    input.focus();
    return false;
  }
  else {
    input.removeClass('is-danger');
    return true;
  }
}

function getCountOrder(status) {
  $.ajax({
    type: "GET",
    url: "/APIv1/Sheet/GetCount",
    data: { status },
    dataType: "json",
    success: function (res) {
      $(`#count_order_${status}`).html(res);
      //setTimeout(function () {
      //    getCountOrder(status);
      //}, 30000);
    }
  });
}

$(document).ready(function () {
  $('.overWord').click(function () {
    let text = $(this);
    if (text.attr('class').indexOf('show') === -1)
      text.addClass('show');
  });

  $('.text_2line').click(function () {
    let text = $(this);
    if (text.attr('class').indexOf('show') === -1)
      text.addClass('show');
    else
      text.removeClass('show');
  });

  $('[data-enter="disabled"]').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
      e.preventDefault();
      return false;
    }
  });

  $('.input[type="money"').keyup(function () {
    let value = $(this).val().toString().replace(/,/g, '');
    $(this).val(formatToMoney(value));
  });

  $(".navbar-burger").click(function () {
    $(".navbar-burger").toggleClass("is-active");
    $(".navbar-menu").toggleClass("is-active");
  });

  $('.js_loading').live("click", function () {
    $('body').addClass('page-loading');
  });

  //$('button[type="submit"]').live("click", function () {
  //    $('body').addClass('page-loading');
  //});

  checkBoxSelectAll();
  runGallery();
  setDataWidth();

  if (location.href.indexOf('order/crawl') === -1) {
    getCountOrder(2);
  }

  if (isMobile) {
    tableMobile();
  }

  // Copy to Clipboard
  var clipboard = new ClipboardJS('.js_copy');
  clipboard.on('success', function (e) {
    const copied = e.trigger;
    copied.className = copied.className + ' is_copied';
    setTimeout(function () {
      copied.className = copied.className.replace('is_copied', '');
    }, 1000);
  });

  //Show Notification
  if (getParameterByName('msgg') !== null) {
    showNotify(getParameterByName('msgg'), true);
    changeUrlAfterNotify();
  }
  if (getParameterByName('msgr') !== null) {
    showNotify(getParameterByName('msgr'), false);
    changeUrlAfterNotify();
  }

  window.addEventListener("keydown", function (e) {
    if (e.keyCode === 118) {
      var btn_save = $('.btn_save');
      btn_save[0].click();
    }
    else if (e.keyCode === 119) {
      var btn_create = $('.btn_create');
      btn_create[0].click();
    }
  });
});

document.addEventListener('DOMContentLoaded', () => {
  (document.querySelectorAll('.notification .delete') || []).forEach(($delete) => {
    $notification = $delete.parentNode;

    $delete.addEventListener('click', () => {
      $notification.parentNode.removeChild($notification);
    });
  });
});

function changeShop() {
  const old = getParameterByName('shop');
  const shop = $('#ddlShopId').val();
  let url = location.href;

  if (url.indexOf('?') === -1)
    url = url + `?shop=${shop}`;
  else if (url.indexOf(`shop=${old}`) !== -1)
    url = url.replace(`shop=${old}`, `shop=${shop}`);
  else
    url = url + `&shop=${shop}`;

  location.href = url;
}