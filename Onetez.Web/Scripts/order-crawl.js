
const size = 500;

function crawlOrder(id, page) {
   $(`#crawl_${id}`).addClass('is-loading');
   $.ajax({
      type: "GET",
      url: "/APIv1/Order/CrawlOrder",
      data: { id, page },
      dataType: "json",
      success: function (res) {
         console.log(`PAGE ${page} = ${res.message}`);
         if (res.status === true) {
            if (res.message === 'OK') {
               showDone(id);
            }
            else {
               $(`#result_${id}`).html(page * size);
               crawlOrder(id, page + 1);
            }
         }
         else {
            showNotify(res.message, false);
            crawlOrder(id, page + 1);
         }
      }
   });
}

function showDone(id) {
   $(`#crawl_${id}`).removeClass('is-loading');
   $(`#crawl_${id}`).addClass('is-success');
   $(`#crawl_${id}`).html('ĐÃ XONG');
}