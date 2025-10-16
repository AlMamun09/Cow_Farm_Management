function handleAjaxFormSubmit(formId, successMessage) {
    $('#' + formId).on('submit', function (e) {
        e.preventDefault(); // Stop default form submission

        var form = $(this);
        $(this).find('.text-danger').text(''); // Clear previous errors

        if (form.valid()) {
            $.ajax({
                type: "POST",
                url: form.attr('action'),
                data: form.serialize(),
                success: function (response) {
                    if (response.success) {
                        // Use SweetAlert2 for the success message
                        Swal.fire({
                            title: successMessage, // Use the custom success message from the view
                            icon: "success",
                            draggable: true
                        }).then((result) => {
                            // This part runs AFTER the user interacts with the alert
                            if (result.isConfirmed) {
                                // Redirect to the Index page
                                window.location.href = response.redirectUrl;
                            }
                        });
                    } else {
                        // Handle server-side validation errors
                        $.each(response.errors, function (key, value) {
                            $('[name="' + key + '"]').next('.text-danger').text(value.join(', '));
                        });
                    }
                },
                error: function () {
                    // Use SweetAlert2 for error messages too
                    Swal.fire({
                        title: 'Error!',
                        text: 'An unexpected error occurred.',
                        icon: 'error'
                    });
                }
            });
        }
    });
}