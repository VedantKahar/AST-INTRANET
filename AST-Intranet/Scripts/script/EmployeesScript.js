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

// Sample Data for Male/Female Employees per Year
const maleFemaleData = {
    labels: ['2020', '2021', '2022', '2023', '2024'], // Year labels
    datasets: [
        {
            label: 'Male Employees',
            data: [250, 270, 300, 320, 345], // Male employee count per year
            borderColor: '#4CAF50',
            backgroundColor: '#4CAF50',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Female Employees',
            data: [150, 180, 200, 210, 222], // Female employee count per year
            borderColor: '#FF4081',
            backgroundColor: '#FF4081',
            fill: false,
            tension: 0.1,
        },
    ],
};

// Sample Data for Employees per Department per Year
const departmentData = {
    labels: ['2020', '2021', '2022', '2023', '2024'], // Year labels
    datasets: [
        {
            label: 'Software Department',
            data: [50, 55, 60, 65, 72], // Software department employee count per year
            borderColor: '#2196F3',
            backgroundColor: '#2196F3',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'HR Department',
            data: [100, 120, 130, 140, 142], // HR department employee count per year
            borderColor: '#FFC107',
            backgroundColor: '#FFC107',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Customer Service Department',
            data: [75, 80, 85, 90, 105], // Customer Service department employee count per year
            borderColor: '#8BC34A',
            backgroundColor: '#8BC34A',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Plant Operation Department',
            data: [60, 65, 70, 80, 107], // Plant Operation department employee count per year
            borderColor: '#FF5722',
            backgroundColor: '#FF5722',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Finance & Accounts',
            data: [45, 55, 60, 65, 70], // Finance & Accounts department employee count per year
            borderColor: '#9C27B0',
            backgroundColor: '#9C27B0',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Sales & Proposals',
            data: [40, 45, 50, 55, 60], // Sales & Proposals department employee count per year
            borderColor: '#03A9F4',
            backgroundColor: '#03A9F4',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Business Development',
            data: [35, 38, 42, 47, 55], // Business Development department employee count per year
            borderColor: '#673AB7',
            backgroundColor: '#673AB7',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Projects',
            data: [60, 65, 70, 75, 80], // Projects department employee count per year
            borderColor: '#FF9800',
            backgroundColor: '#FF9800',
            fill: false,
            tension: 0.1,
        },
        {
            label: 'Engineering & Operations',
            data: [70, 80, 85, 95, 102], // Engineering & Operations department employee count per year
            borderColor: '#009688',
            backgroundColor: '#009688',
            fill: false,
            tension: 0.1,
        },
    ],
};

// Chart Initialization
const ctx = document.getElementById('employeeChart').getContext('2d');
let currentChart = null; // Holds the current chart object

// Create the chart (either Male/Female or Department data)
function createChart(data) {
    if (currentChart) {
        currentChart.destroy(); // Destroy the previous chart if it exists
    }

    currentChart = new Chart(ctx, {
        type: 'bar', // You can change this to 'line','bar', 'radar', etc., depending on the graph style you want
        data: data, // Pass the selected data (maleFemaleData or departmentData)
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
                    title: {
                        display: true,
                        text: 'Year', // X-axis label
                    },
                    grid: {
                        display: true,
                        color: 'rgba(0,0,0,0.1)', // Optional, to make the gridlines less prominent
                    },
                },
                y: {
                    title: {
                        display: true,
                        text: 'Number of Employees', // Y-axis label
                    },
                    ticks: {
                        beginAtZero: true, // Start from 0 for better clarity
                        stepSize: 20, // Adjust the step size as needed
                    },
                },
            },
        },
    });
}

// Update the chart based on the selected year range
function updateChart() {
    const selectedGraphType = document.getElementById('graphType').value;
    const startYear = parseInt(document.getElementById('startYear').value);
    const endYear = parseInt(document.getElementById('endYear').value);

    // Filter the data based on the selected year range
    const yearLabels = maleFemaleData.labels;
    const startIndex = yearLabels.indexOf(startYear.toString());
    const endIndex = yearLabels.indexOf(endYear.toString());

    // If the start year is greater than the end year, return early
    if (startIndex > endIndex) {
        alert("Start year cannot be greater than End year.");
        return;
    }

    const filteredData = {
        labels: yearLabels.slice(startIndex, endIndex + 1),
        datasets: [],
    };

    // Select the appropriate data based on graph type (gender or department)
    let dataToDisplay;
    if (selectedGraphType === 'gender') {
        dataToDisplay = maleFemaleData;
    } else if (selectedGraphType === 'department') {
        dataToDisplay = departmentData;
    }

    // Filter datasets for the selected year range
    filteredData.datasets = dataToDisplay.datasets.map(dataset => ({
        ...dataset,
        data: dataset.data.slice(startIndex, endIndex + 1),
    }));

    // Update the chart with the new filtered data
    createChart(filteredData);
}

// Initial chart load
createChart(departmentData); // Start with the department data

// Event listener for dropdown changes (to switch between Male/Female and Department data)
const graphTypeSelect = document.getElementById('graphType');
const startYearSelect = document.getElementById('startYear');
const endYearSelect = document.getElementById('endYear');

// Add one event listener for each dropdown
graphTypeSelect.addEventListener('change', updateChart);
startYearSelect.addEventListener('change', updateChart);
endYearSelect.addEventListener('change', updateChart);