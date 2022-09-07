
function deleteItem(id) {
    if (confirm('Bạn có chắc muốn xóa khách hàng này ?')) {
        $.ajax({
            type: "POST",
            url: "/APIv1/Customer/Delete",
            data: { id, status },
            dataType: "json",
            success: function (res) {
                if (res.status === true) {
                    $(`#row_${id}`).remove();
                }
                else {
                    showNotify(res.msg, false);
                }
            }
        });
    }
}