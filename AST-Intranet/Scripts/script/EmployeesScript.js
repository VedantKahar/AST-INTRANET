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
// Random color generator for chart lines
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// Create the chart with dynamic data
let currentChart = null;
const ctx = document.getElementById('employeeChart').getContext('2d'); // Assuming you have a canvas with id 'employeeChart'

// Fetch Department Employee data from the server
function fetchDepartmentData(startYear, endYear) {
    return fetch(`/Employees/GetEmployeesByDepartmentPerYear?startYear=${startYear}&endYear=${endYear}`)
        .then(response => response.json())
        .then(data => {
            const departments = Array.from(new Set(data.map(item => item.department))); // Get unique departments
            const departmentData = {
                labels: data.map(item => item.year), // Extract years
                datasets: departments.map(department => {
                    return {
                        label: department,
                        data: data.filter(item => item.department === department).map(item => item.employee_count),
                        borderColor: getRandomColor(),
                        backgroundColor: getRandomColor(),
                        fill: false,
                        tension: 0.1,
                    };
                }),
            };
            return departmentData;
        });
}

// Fetch Male/Female Employee data from the server
function fetchMaleFemaleData(startYear, endYear) {
    return fetch(`/Employees/GetMaleFemaleEmployeesByYear?startYear=${startYear}&endYear=${endYear}`)
        .then(response => response.json())
        .then(data => {
            return {
                labels: data.map(item => item.year), // Extract years
                datasets: [
                    {
                        label: 'Male Employees',
                        data: data.map(item => item.male_count), // Extract male counts
                        borderColor: '#4CAF50',
                        backgroundColor: '#4CAF50',
                        fill: false,
                        tension: 0.1,
                    },
                    {
                        label: 'Female Employees',
                        data: data.map(item => item.female_count), // Extract female counts
                        borderColor: '#FF4081',
                        backgroundColor: '#FF4081',
                        fill: false,
                        tension: 0.1,
                    },
                ],
            };
        });
}

// Fetch year range dynamically from the backend
function fetchYearRange() {
    fetch('/YourController/GetYearRange')  // Adjust the URL to your actual endpoint
        .then(response => response.json())
        .then(data => {
            const [earliestYear, latestYear] = data;

            const startYearSelect = document.getElementById('startYear');
            const endYearSelect = document.getElementById('endYear');

            // Clear existing options
            startYearSelect.innerHTML = '';
            endYearSelect.innerHTML = '';

            // Add options dynamically for years
            for (let year = earliestYear; year <= latestYear; year++) {
                const option = document.createElement('option');
                option.value = year;
                option.textContent = year;
                startYearSelect.appendChild(option);

                const optionEnd = document.createElement('option');
                optionEnd.value = year;
                optionEnd.textContent = year;
                endYearSelect.appendChild(optionEnd);
            }

            // Set default values for start and end years
            startYearSelect.value = earliestYear;
            endYearSelect.value = latestYear;

            // Trigger the chart update after populating the dropdowns
            updateChart();
        })
        .catch(error => console.error('Error fetching year range:', error));
}

// Initial chart load and year range fetch
fetchYearRange();

// Create the chart with dynamic data
function createChart(data) {
    if (currentChart) {
        currentChart.destroy(); // Destroy previous chart instance to prevent duplication
    }

    currentChart = new Chart(ctx, {
        type: 'line', // Can change to 'bar' or other chart types if needed
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                },
            },
            scales: {
                x: {
                    type: 'category',
                    title: { display: true, text: 'Year' },
                },
                y: {
                    title: { display: true, text: 'Number of Employees' },
                    ticks: { beginAtZero: true, stepSize: 1 },
                },
            },
        },
    });
}

// Fetch data and create chart based on selected graph type and year range
function updateChart() {
    const selectedGraphType = document.getElementById('graphType').value;
    const startYear = parseInt(document.getElementById('startYear').value);
    const endYear = parseInt(document.getElementById('endYear').value);

    if (selectedGraphType === 'department') {
        fetchDepartmentData(startYear, endYear).then(data => createChart(data));
    }
}

// Listen for changes in the dropdown and update the chart accordingly
document.getElementById('graphType').addEventListener('change', updateChart);
document.getElementById('startYear').addEventListener('change', updateChart);
document.getElementById('endYear').addEventListener('change', updateChart);

// Initial chart load (on page load or when the page is ready)
updateChart();