
$(function() {
    $(".searchBox").autocomplete({
        minLength: 2, 
        source: function (request, response) {
            $.ajax({
                url: "/Product/GetProductsNames",
                type: "GET",
                dataType: "json",
                data: { title: request.term },
                success: function (result) {
                    response(result);
                }
            })
        },
        select: function (event, ui) {
            $("input.searchBox").val(ui.item.value);
            $("button.searchBtn:submit").trigger("click");            
        }
    });
});

$(window).resize(function () {
    $(".searchBox").autocomplete("search");
});


$(function () {
    $(".body-content").on("click", ".jqAddToCart", addToCart);
    $(".body-content").on("keydown", ".jqPhoneNumberInput", function (e) { onlyNumbers(e); });
});

function addToCart() {
    var el = $(this);
    if (!el.hasClass("jqAddToCart"))
        return;
    $(".body-content").off("click", ".jqAddToCart", addToCart);
    var id = el.attr("data-productId");
    var prevState = el.html();
    $.ajax({
        type: 'POST',
        url: "/ShoppingCart/AddToCart",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ 'productId': id }),
        beforeSend: function () {
            if (el.hasClass("btn-lg")) {
                el.html('<div"><img src="/Content/img/800.svg" alt="Обработка"'
                + 'style="margin:auto;"></div>');
            }
            else {
                el.html('<div style=width:82px;height:20px"><img src="/Content/img/800.svg" alt="Обработка"'
                    + 'style="margin:auto;max-height:20px;"></div>');
            }
        },
        complete: function(jqXHR, textStatus){
            if (textStatus != "success")
                el.html(prevState);
            $(".body-content").on("click", ".jqAddToCart", addToCart);
        },
        success: function (result) {
            var count = parseInt(result.itemsCount);
            if (count != NaN && count > 0) {
                $(".cartCount").text(count.toString());
                $(".cartCount").css("display", "block");
                el.removeClass("jqAddToCart");
                el.removeClass("btn-success");
                el.addClass("btn-primary");
                el.attr("href", "/ShoppingCart/Order");
                el.html('<span class="glyphicon glyphicon-ok-circle"></span> В корзине');
                return;
            }
            el.html(prevState);
        }
    });
}

function onlyNumbers(e, allowComma) {
    if (allowComma == true && e.keyCode === 110)
        return;
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
        (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57))
        && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
}