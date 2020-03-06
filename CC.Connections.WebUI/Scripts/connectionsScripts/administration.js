$(document).ready(function () {
    var selectcounter = 1;

    $("td").each(function () {
        idja = "selectable" + selectcounter;
        $(this).attr('id', idja);
        $(this).attr('onclick', 'selectText("' + idja + '")');
        selectcounter++;
    });
});

function selectText(containerid) {
    var range;
    if (document.selection) {
        range = document.body.createTextRange();
        range.moveToElementText(document.getElementById(containerid));
        range.select();
    } else if (window.getSelection) {
        range = document.createRange();
        range.selectNode(document.getElementById(containerid));
        window.getSelection().addRange(range);
    }
}