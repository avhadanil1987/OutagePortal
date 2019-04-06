$('#Password').focusout(function () {
    var txtpassword = $('#Password').val();

    if (txtpassword != "") {
        var key = CryptoJS.enc.Utf8.parse('8681878489828180');
        var iv = CryptoJS.enc.Utf8.parse('86818785698926180');

        var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(txtpassword), key, { keySize: 128 / 8, iv: iv, mode: CryptoJS.mode.CBC, padding: CryptoJS.pad.Pkcs7 });
        $('#EncryptPassword').val(encryptedpassword);
    }
});

SubmitRegsiterForm = function () {
    if ($("#FirstName").val() != "" && $("#LastName").val() != "" && $("#UserEmail").val() != "") {
        $('#ConfirmPassword').val("Password@1234");
        $('#Password').val("Password@1234");
    }
}

LoginForm = function () {
    if ($("#UserEmail").val() != "" && $("#Password").val()!="") {
        $('#Password').val("Password@1234");
    }
}
