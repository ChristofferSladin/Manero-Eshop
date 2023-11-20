$(document).ready(function () {
    loadShoppingCartPartial();
});

function loadShoppingCartPartial() {
    $.ajax({
        url: '/Home/HeaderPartial', // Modified URL
        data: {},
        method: 'GET',
        success: function (result) {
            $('#shoppingCart').html(result);

            // Attach event listeners
            const dropdownButton = document.querySelector('.dropdown-button'); // Changed to querySelector
            const dropdownContent = document.querySelector('.dropdown-content'); // Assuming dropdown content has this class

            if (dropdownButton && dropdownContent) {
                dropdownButton.addEventListener('click', (event) => {
                    // Prevent the event from closing the dropdown if it's already open
                    event.stopPropagation();
                    dropdownContent.style.display = dropdownContent.style.display === 'none' ? 'block' : 'none';
                });
            }

            // Close the dropdown if clicked outside
            window.addEventListener('click', (event) => {
                if (!dropdownButton.contains(event.target) && !dropdownContent.contains(event.target) && dropdownContent) {
                    dropdownContent.style.display = 'none';
                }
            });

            // Prevent clicks within the dropdown content from closing it
            dropdownContent.addEventListener('click', (event) => {
                event.stopPropagation();
            });
        },
        error: function (error) {
            console.error(error);
        }
    });
}
