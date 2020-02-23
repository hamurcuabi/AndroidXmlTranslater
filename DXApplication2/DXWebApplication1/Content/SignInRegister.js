(function() {
    function onPasswordButtonEditButtonClick(s, e) {
        var inputType = s.GetInputElement().type;
        var turnOnPasswordMode = inputType !== "password";

        s.GetInputElement().type = turnOnPasswordMode ? "password" : "text";

        var eyeButton = s.GetMainElement().getElementsByClassName("eye-button")[0];
        if (turnOnPasswordMode)
            ASPxClientUtils.RemoveClassNameFromElement(eyeButton, "show-password");
        else
            ASPxClientUtils.AddClassNameToElement(eyeButton, "show-password");
    }

    function isPasswordSimple(password) {
        var passwordMinLength = 8;
        return password.length > 0 && password.length < passwordMinLength;
    }

    function getErrorText(editor) {
        var password = passwordButtonEdit.GetText(),
            confirmPassword = confirmPasswordButtonEdit.GetText();
        if(editor === passwordButtonEdit && isPasswordSimple(password)) {
            return "Use 8 characters or more for your password";
        } else if(editor === confirmPasswordButtonEdit && password !== confirmPassword) {
            return "The password you entered do not match";
        }
        return "";
    }

    function onPasswordValidation(s, e) {
        var errorText = getErrorText(s);
        if(errorText) {
            e.isValid = false;
            e.errorText = errorText;
        }
    }

    window.onPasswordButtonEditButtonClick = onPasswordButtonEditButtonClick;
    window.onPasswordValidation = onPasswordValidation;
})();