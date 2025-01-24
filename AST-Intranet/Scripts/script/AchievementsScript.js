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


        let currentIndex = 0;
        const images = document.querySelectorAll('.slider-images img'); // Select all images in the slider
        const totalImages = images.length;
        const titleElement = document.getElementById('achievement-title'); // Title element
        const descriptionElement = document.getElementById('achievement-description'); // Description element

        const achievements = [
            {
                title: 'Company Achievement 1',
                description: 'Details about the achievement. This can include awards or recognitions received by the company.',
                imageSrc: '~/Images/images/AST bg.jfif'
            },
            {
                title: 'Company Achievement 2',
                description: 'Details about another achievement. This includes more information about the company\'s growth or progress.',
                imageSrc: '~/Images/images/AST-building.png'
            }
        ];

        function showImage(index) {
            // Hide all images
            images.forEach((img) => {
                img.classList.remove('active');
            });

            // Show the current image
            images[index].classList.add('active');

            // Update the title and description based on current index
            titleElement.textContent = achievements[index].title;
            descriptionElement.textContent = achievements[index].description;
        }

        function nextImage() {
            currentIndex = (currentIndex + 1) % totalImages; // Loop back to the first image when at the end
            showImage(currentIndex);
        }

        function prevImage() {
            currentIndex = (currentIndex - 1 + totalImages) % totalImages; // Loop back to the last image when at the beginning
            showImage(currentIndex);
        }

        // Automatically slide every 5 seconds
        setInterval(nextImage, 5000);

        // Initialize the first image
        showImage(currentIndex);
