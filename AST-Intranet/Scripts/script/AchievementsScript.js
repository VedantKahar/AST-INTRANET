
        const toggleSidebar = document.getElementById('toggleSidebar');
        const sidebar = document.getElementById('sidebar');

        let isSidebarClicked = false; // Flag to track if sidebar is manually toggled

        // Toggle sidebar open/close on click
        toggleSidebar.addEventListener('click', function () {
            isSidebarClicked = !isSidebarClicked; // Toggle state

            if (isSidebarClicked) {
                sidebar.classList.add('open'); // Open the sidebar
                sidebar.style.width = '250px'; // Explicitly set width when open
            } else {
                sidebar.classList.remove('open'); // Close the sidebar
                sidebar.style.width = '70px'; // Explicitly set width when closed
            }

            // Remove hover event listeners when sidebar is manually toggled
            sidebar.removeEventListener('mouseenter', hoverOpen);
            sidebar.removeEventListener('mouseleave', hoverClose);

            // Re-enable hover effect only when sidebar is closed
            if (!isSidebarClicked) {
                // Only re-enable hover when sidebar is closed
                sidebar.addEventListener('mouseenter', hoverOpen);
                sidebar.addEventListener('mouseleave', hoverClose);
            }
        });

        // Hover behavior for opening the sidebar
        function hoverOpen() {
            if (!isSidebarClicked) { // Only open on hover if not manually toggled
                sidebar.classList.add('open');
                sidebar.style.width = '250px'; // Ensure the sidebar is open on hover
            }
        }

        // Hover behavior for closing the sidebar
        function hoverClose() {
            if (!isSidebarClicked) { // Only close on hover if not manually toggled
                sidebar.classList.remove('open');
                sidebar.style.width = '70px'; // Ensure the sidebar is closed on hover
            }
        }

        // Initially enable hover effect only when sidebar is closed
        sidebar.addEventListener('mouseenter', hoverOpen);
        sidebar.addEventListener('mouseleave', hoverClose);

        let currentIndex = 0; // Track the current image index
        const images = document.querySelectorAll('.slider-images img'); // Get all images in the slider
        const totalImages = images.length; // Total number of images

        // Function to change the image by incrementing the index
        function nextImage() {
            currentIndex = (currentIndex + 1) % totalImages; // Ensure index is within bounds (cyclic)
            updateSlider();
        }

        // Function to change the image by decrementing the index
        function prevImage() {
            currentIndex = (currentIndex - 1 + totalImages) % totalImages; // Ensure index is within bounds (cyclic)
            updateSlider();
        }

        // Update the slider to show the current image
        function updateSlider() {
            const slider = document.querySelector('.slider-images');
            const offset = -currentIndex * 100; // Move slider to the correct image
            slider.style.transform = `translateX(${offset}%)`;
        }

        // Auto change the image every 5 seconds
        setInterval(nextImage, 5000);

        // Initial call to update the slider when the page loads
        document.addEventListener("DOMContentLoaded", function () {
            updateSlider();
        });

