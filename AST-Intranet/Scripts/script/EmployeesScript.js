   
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


        async function fetchData() {
            try {
                const response = await fetch('http://localhost:3000/departments');
                const data = await response.json();

                // Display total employees, new joiners, and total departments
                document.getElementById('totalEmployees').innerText = data.totalEmployees;
                document.getElementById('newJoiners').innerText = data.newJoiners;
                document.getElementById('totalDepartments').innerText = data.totalDepartments;

                // Display departments dynamically
                const departmentContainer = document.getElementById('departmentContainer');
                data.departments.forEach(department => {
                    const deptDiv = document.createElement('div');
                    deptDiv.classList.add('department');
                    deptDiv.innerHTML = `<h3>${department[1]}</h3><hr /><p>Description of ${department[1]}</p>`;
                    departmentContainer.appendChild(deptDiv);
                });
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        }

        // Fetch data when page loads
        window.onload = fetchData;

        function goToDepartment(department) {
            // Redirect to the department-specific page
            window.open(department + '-employees.html', '_blank'); // For example, 'software-employees.html'
        }


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
                // Filter Male/Female data for the selected year range
                dataToDisplay = maleFemaleData;
            } else if (selectedGraphType === 'department') {
                // Filter Department data for the selected year range
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


    //function loadEmployees(departmentName) {
//    console.log("loadEmployees called for department:", departmentName); // Debugging line

//    $.ajax({
//        url: '@Url.Action("GetEmployeesByDepartment", "Employees")',
//        type: 'GET',
//        data: { departmentName: departmentName },
//        success: function (data) {
//            $('#employee-list').html(data); // Load the returned HTML into the div
//        },
//        error: function (xhr, status, error) {
//            console.error('Error loading employees: ', error); // Log errors
//            alert('Error loading employees: ' + error);
//        }
//    });
//}



//console.log("EmployeesScript.js loaded");

//// Now define showEmployees
//var getEmployeesUrl = '@Url.Action("GetEmployeesByDepartment", "Employees")';

//function showEmployees(department) {
//    console.log("showEmployees called with department: ", department);
//    $.ajax({
//        url: getEmployeesUrl,
//        data: { departmentName: department },
//        type: 'GET',
//        success: function (data) {
//            console.log("Employees Data: ", data); // Log data to see if it's correct
//            var tableBody = $("#employeeTableBody");
//            tableBody.empty(); // Clear any previous rows

//            if (data.length > 0) {
//                data.forEach(function (employee) {
//                    var row = "<tr>";
//                    row += "<td>" + employee.EmpCode + "</td>";
//                    row += "<td>" + employee.EmpName + "</td>";
//                    row += "<td>" + employee.Designation + "</td>";
//                    row += "<td>" + employee.Email + "</td>";
//                    row += "</tr>";
//                    tableBody.append(row);
//                });
//                document.getElementById("employeeModal").style.display = "block";
//            } else {
//                tableBody.append("<tr><td colspan='4'>No employees found</td></tr>");
//                document.getElementById("employeeModal").style.display = "block";
//            }
//        },

//        error: function () {
//            alert('Error fetching employee data');
//        }
//    });
//}
//function closeModal() {
//    document.getElementById("employeeModal").style.display = "none";
//}