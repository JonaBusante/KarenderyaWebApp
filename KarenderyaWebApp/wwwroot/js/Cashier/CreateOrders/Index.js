
document.addEventListener("DOMContentLoaded", function () {

    const quantityInputs = document.querySelectorAll(".quantity-input");
    const totalAmountElement = document.getElementById("totalAmount");
    const orderForm = document.getElementById("orderForm");

    function calculateTotal() {
        let totalAmount = 0;

        quantityInputs.forEach(function (input) {
            const price = Number(input.dataset.price);
            const stock = Number(input.dataset.stock);
            const quantity = Number(input.value);

            const validQuantity =
                Number.isFinite(quantity) && quantity >= 0
                    ? quantity
                    : 0;

            const subtotal = price * validQuantity;
            totalAmount += subtotal;

            const row = input.closest("tr");
            const subtotalElement = row?.querySelector(".item-subtotal");

            if (subtotalElement) {
                subtotalElement.textContent = subtotal.toLocaleString(
                    "en-PH",
                    {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    }
                );
            }

            input.setCustomValidity("");

            if (!Number.isInteger(validQuantity)) {
                input.setCustomValidity("Please enter a whole number.");
            } else if (validQuantity > stock) {
                input.setCustomValidity(
                    `Only ${ stock } item(s) are available.`
                );
            }
        });

        if (totalAmountElement) {
            totalAmountElement.textContent = totalAmount.toLocaleString(
                "en-PH",
                {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }
            );
        }
    }

    quantityInputs.forEach(function (input) {
        input.addEventListener("input", calculateTotal);
        input.addEventListener("change", calculateTotal);
    });

    if (orderForm) {
        orderForm.addEventListener("submit", function (event) {
            calculateTotal();

            let hasSelectedItem = false;
            let hasInvalidQuantity = false;

            quantityInputs.forEach(function (input) {
                const quantity = Number(input.value);
                const stock = Number(input.dataset.stock);

                if (quantity > 0) {
                    hasSelectedItem = true;
                }

                if (
                    !Number.isFinite(quantity) ||
                    !Number.isInteger(quantity) ||
                    quantity < 0 ||
                    quantity > stock
                ) {
                    hasInvalidQuantity = true;
                    input.reportValidity();
                }
            });

            if (hasInvalidQuantity) {
                event.preventDefault();
                return;
            }

            if (!hasSelectedItem) {
                event.preventDefault();
                alert("Please select at least one food item.");
            }
        });
    }

    calculateTotal();


    const successModalElement = document.getElementById("successModal");

    if (successModalElement && typeof bootstrap !== "undefined") {
        const successModal = new bootstrap.Modal(successModalElement);
        successModal.show();
    }

    // Close the modal and return to a fresh order page.
    const okayButton = document.getElementById("successOkayButton");

    if (okayButton) {
        okayButton.addEventListener("click", function () {
            window.location.href = window.location.pathname;
        });
    }

});