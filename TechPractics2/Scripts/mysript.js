var coll = document.getElementsByClassName("mycollapsible");
var i;

for (i = 0; i < coll.length; i++) {
    coll[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var item = document.getElementById("naravartopid");
        var table = document.getElementById("LoginTableId");
        var link0 = document.getElementById("navbarBrandId");
        var link1 = document.getElementById("navbarLinkId_1");
        var link2 = document.getElementById("navbarLinkId_2");
        var butt = document.getElementById("togglebutton");
        var content = this.nextElementSibling;
        if (content.style.display === "block") {
            content.style.display = "none";
            item.style.backgroundColor = "cornflowerblue";
            item.style.borderColor = "black";
            table.style.backgroundColor = "transparent";
            link0.style.display = "none";
            link1.style.display = "none";
            link2.style.display = "none";
            butt.style.display = "none";
        } else {
            content.style.display = "block";
            item.style.backgroundColor = "transparent";
            item.style.borderColor = "transparent";
            table.style.backgroundColor = "gray";
            link0.style.display = "block";
            link1.style.display = "block";
            link2.style.display = "block";
            butt.style.display = "block";
        }
    });
}
