
$(document).ready(function () {
    $("#RoleName").on("change", function () {
        if ($(this).val() != "Doctor") {
            $(".SpecialistDoctordiv").hide();
        } else {
            $(".SpecialistDoctordiv").show();
        }
    });

    $(".add_phone_number").on("click", function () {
        var phoneNumberDiv = '<div class="phone-number form-group"><input type="text" name="PhoneNumbers" placeholder="Phone Number" /><button type="button" class="remove-phone-number">Remove</button></div>';
        $("#phoneNumbersContainer").append(phoneNumberDiv);
    });

    //// Allow removing phone number fields
    $("#phoneNumbersContainer").on("click", ".remove-phone-number", function () {
        $(this).parent().remove();
    });




    $(".add_SpecialDoctor").on("click", function () {
        var specialistDiv = '<div class="specialistDiv form-group"><input type="text" name="SpecialistDoctors" placeholder="specialist"/><button type="button" class="remove_specialdoctor">Remove</button></div>';
        $("#add_SpecialDoctorContainer").append(specialistDiv);
    });

    // Allow removing specialist doctor fields
    $("#add_SpecialDoctorContainer").on("click", ".remove_specialdoctor", function () {
        $(this).parent().remove();
    });




    $("#MakeAppoint").on("click", function () {
        $(".showAppointment").show();
    });

});

//});





