
$(document).ready(function (global) {
    $(".Collapsable").click(function () {
        var id = $(this).parent().children();
        var ul = id.filter("ul");
        var spans = id.filter("span");
        var li = ul.children();
        var ch = li.children();
        var buttons = ch.filter("button").toggle();
        ul.toggle();
        spans.toggle();
        $(this).toggle();

    });

    global.OpenedBrackets = 0;
    function clearCur() {
        $('#CurrentExpression').text("");
    }

    $("#SelectEntity").change(function () {
        clearCur();
        var newclass = $(this).find("option:selected").val();
        var oldClass = $('#MType').val();
        var id = '#' + newclass;
        $('#MType')[0].value = newclass;
        $('#OrderEntry').find("*").show();



        $(".UnHide").each(function () {
            $(this).removeClass("UnHide");
        })

        var temp = $(id).addClass("UnHide");
        $("#OrderEntry").find("button").hide();
        $(".UnHide").find("button").show();
        $(id).find(".Collapsable").each(function () {

            var id = $(this).parent().children();
            var ul = id.filter("ul");
            var spans = id.filter("span");
            var li = ul.children();
            var ch = li.children();
            var buttons = ch.filter("button").toggle();
            ul.toggle();
            spans.toggle();

            $(this).toggle();
        });

    });
    $('#MType')[0].value = "OrderEntry";


    $(".Collapsable").each(function () {

        var id = $(this).parent().children();
        var ul = id.filter("ul");
        var spans = id.filter("span");
        var li = ul.children();
        var ch = li.children();
        var buttons = ch.filter("button").toggle();
        ul.toggle();
        spans.toggle();

        $(this).toggle();
    });
    $(".Collapsable").parent().children().filter("button").each(function () {
        $(this).click(function () {
            var tempstr = $(this).attr('name');
            var par = $(this).parent().parent().parent();
            var tempClass = $('#MType')[0].value;
            while (par.attr("id") != tempClass) {
                tempstr = par.attr("id") + "." + tempstr;
                par = par.parent().parent();
            }
            $("#CurrentSelector").text(tempstr);
        });
    });

    $("#BracketOpen").click(function () {
        $("#CurrentExpression").append("( ");
        global.OpenedBrackets++;
    });
    $("#BracketClose").click(function () {
        if (global.OpenedBrackets > 0) {
            $("#CurrentExpression").append(") ");
            global.OpenedBrackets--;
        }
    });
    $("#ClearBtn").click(clearCur);



    $('#AddToCurExp').click(function () {
        var str = $("#CurrentSelector").text()+" ";
        alert($("#Operations").find("option:selected").val());
        str = str + $("#Operations").find("option:selected").val()+" ";
        str = str + $("#userinp").val()+" ";
    if ($("#logop").find("option:selected").text().length > 0) {
        str = str + $("#logop").find("option:selected").text()+" ";
    }
        $("#CurrentExpression").append(str);
        var st = $("#CurrentExpression").text();
        $('#Exp')[0].value = st;
});
});

