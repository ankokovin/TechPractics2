$('input[name=UseProfile]').change(function () {
    if ($(this).is(':checked')) {
        var len = $('select[name=ProfileId] option').length;
        if (len == 0)
        {
            $(this).prop('checked', false);
            $('#NoProfileLable').show();
            $('#NewProfileLink').show();
        }
        else {
            $('.profile').hide();
            $('select[name=ProfileId]').show();
        }

       
    } else {
        $('.profile').show();
        $('select[name=ProfileId]').hide();
    }           
})


