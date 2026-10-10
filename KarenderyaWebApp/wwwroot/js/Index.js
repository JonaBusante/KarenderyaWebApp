
$(document).ready(function () {

    const $form = $('#main_form');

    $('#Username').trigger('focus');

    $form.on('submit', async function (e) {
        e.preventDefault();

        if (!this.checkValidity()) {
            this.reportValidity();
            return;
        }

        const request = {
            username: $('#Username').val().trim(),
            password: $('#Password').val()
        };

        try {
            const response = await fetch('/api/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(request)
            });

            const result = await response.json();

            if (result.success) {
                window.location.href = result.page;
            } else {
                alert(result.message);
            }
        } catch (error) {
            console.error('Login error:', error);
            alert('Unable to connect to the server. Please try again.');
        }
    });

});