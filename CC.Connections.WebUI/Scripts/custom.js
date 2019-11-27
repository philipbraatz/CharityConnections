
$(function () {

    // Displays the volunteer category nav bar when hovering over "find volunteer opportunities"
    $("#findVolunteerBtn").mouseenter(function () {
        // This was how we were showing seconnd nav before
        //$("#volunteerBar").slideToggle("slow");

        // This will keep second (find volunteer) nav at top of page when scrolling
        $("#volunteerBar").appendTo(".nav1").slideToggle("slow");
    })

    // Changes the picture (different color) when hovering over link
    $("#paw").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/paw_category.svg').replaceWith('<img src="../../Content/Images/paw_categoryOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/paw_categoryOrange.svg').replaceWith('<img src="../../Content/Images/paw_category.svg">');
    })

    $("#book").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/bookBtn.svg').replaceWith('<img src="../../Content/Images/bookBtnOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/bookBtnOrange.svg').replaceWith('<img src="../../Content/Images/bookBtn.svg">');
    })

    $("#health").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/healthBtn.svg').replaceWith('<img src="../../Content/Images/healthBtnOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/healthBtnOrange.svg').replaceWith('<img src="../../Content/Images/healthBtn.svg">');
    })

    $("#people").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/peopleBtn.svg').replaceWith('<img src="../../Content/Images/peopleBtnOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/peopleBtnOrange.svg').replaceWith('<img src="../../Content/Images/peopleBtn.svg">');
    })

    $("#tree").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/treeBtn.svg').replaceWith('<img src="../../Content/Images/treeBtnOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/treeBtnOrange.svg').replaceWith('<img src="../../Content/Images/treeBtn.svg">');
    })

    $("#hands").mouseenter(function () {
        $(this).find('img').attr('src', '../../Content/Images/helping_hands.svg').replaceWith('<img src="../../Content/Images/helping_handsOrange.svg">');
    }).mouseleave(function () {
        $(this).find('img').attr('src', '../../Content/Images/helping_handsOrange.svg').replaceWith('<img src="../../Content/Images/helping_hands.svg">');
    })

});