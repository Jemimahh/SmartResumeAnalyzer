document.addEventListener('DOMContentLoaded', function () {
    const inputs = document.querySelectorAll("input[data-val-fileextension]");

    inputs.forEach(function (input) {
        input.addEventListener('change', function () {
            const allowed = input.getAttribute("data-val-fileextension-extensions").split(',');
            const fileName = input.value.split("\\").pop().toLowerCase();
            const ext = "." + fileName.split('.').pop();

            if (!allowed.includes(ext)) {
                input.setCustomValidity(input.getAttribute("data-val-fileextension"));
            } else {
                input.setCustomValidity("");
            }
        });
    });
});