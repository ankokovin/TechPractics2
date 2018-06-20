if (!Modernizr.inputtypes.date) {
    $(document).ready(function () {
        $(function () {
            $("input[type='date']")
                .datepicker()
                .get(0)
                .setAttribute("type", "text");
        })
    })
}