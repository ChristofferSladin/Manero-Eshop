$(document).ready(function () {
    loadShoppingCartPartial();
});

function loadShoppingCartPartial() {
    $.ajax({
        url: '/Home/HeaderPartial',
        data: {},
        method: 'GET',
        success: function (result) {
            $('#shoppingCart').html(result);
            // Attach event listeners
            const dropdownButton = document.querySelector('.dropdown-button');
            const dropdownContent = document.querySelector('.dropdown-content');
            const closeButton = document.getElementById('close-button');
            closeButton.addEventListener('click', () => {
                dropdownContent.style.display = 'none';
            });
            if (dropdownButton && dropdownContent) {
                // Retrieve the state from localStorage
                const isDropdownOpen = localStorage.getItem('dropdownState') === 'open';

                // Set initial state
                dropdownContent.style.display = isDropdownOpen ? 'block' : 'none';

                dropdownButton.addEventListener('click', (event) => {
                    // Prevent the event from closing the dropdown if it's already open
                    event.stopPropagation();
                    // Toggle the display state
                    dropdownContent.style.display = dropdownContent.style.display === 'none' ? 'block' : 'none';

                    // Save the state to localStorage
                    const newState = dropdownContent.style.display === 'none' ? 'closed' : 'open';
                    localStorage.setItem('dropdownState', newState);
                });
            }

            // Close the dropdown if clicked outside
            window.addEventListener('click', (event) => {
                if (!dropdownButton.contains(event.target) && !dropdownContent.contains(event.target) && dropdownContent) {
                    dropdownContent.style.display = 'none';
                    // Save the state to localStorage
                    localStorage.setItem('dropdownState', 'closed');
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
