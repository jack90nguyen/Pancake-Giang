
function resetPassword(id) {
    const name = $(`#row_${id} .avatar-round .is-text`).html();

    if (confirm(`Bạn có chắc muốn cấp lại mật khẩu cho tài khoản "${name}" không ?`)) {
        $.ajax({
            type: "POST",
            url: "/APIv1/User/ResetPassword",
            data: { id },
            dataType: "json",
            success: function (res) {
                showNotify(res.msg, res.status);
            }
        });
    }
}

function deleteItem(id) {
    const name = $(`#row_${id} .avatar-round .is-text`).html();

    if (confirm(`Bạn có chắc muốn xóa tài khoản "${name}" không ?`)) {
        $.ajax({
            type: "POST",
            url: "/APIv1/User/Delete",
            data: { id },
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    $(`#row_${id}`).remove();
                }
                else {
                    showNotify(res.msg, false);
                }
            }
        });
    }
}