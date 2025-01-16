
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

        $(document).ready(function () {
            const apiKey = 'PFXdUFwPMihLk1IzaFIng3tUuzhftAHd'; // Your API Key
            const countryCode = 'IN'; // Country code for India
            const year = 2025; // Year for fetching holidays

            // Fetch holidays from the Calendarific API
            function fetchIndianHolidays() {
                const url = `https://calendarific.com/api/v2/holidays?api_key=${apiKey}&country=${countryCode}&year=${year}`;

                fetch(url)
                    .then(response => response.json())
                    .then(data => {
                        if (data.response && data.response.holidays) {
                            const holidays = data.response.holidays;
                            console.log("Fetched holidays:", holidays);

                            // Create events from the holidays
                            const events = holidays.map(holiday => {
                                return {
                                    title: holiday.name,
                                    start: moment(holiday.date.iso).format(),  // Ensure the correct format for FullCalendar
                                    description: holiday.description,
                                    allDay: true
                                };
                            });

                            // Add fetched holidays as events to FullCalendar
                            addHolidaysToCalendar(events);
                        } else {
                            console.error("Error fetching holidays:", data);
                        }
                    })
                    .catch(error => {
                        console.error("Failed to fetch holidays from the API:", error);
                    });
            }

            // Add holidays to FullCalendar
            function addHolidaysToCalendar(holidays) {
                console.log("Adding holidays to calendar:", holidays);

                // Clear existing events and add new ones
                $('#calendar').fullCalendar('removeEvents'); // Clear existing events
                $('#calendar').fullCalendar('addEventSource', holidays); // Add fetched holidays as events
            }

            // Fetch holidays and festivals and add them to the calendar
            fetchIndianHolidays();

            // Initialize FullCalendar
            $('#calendar').fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                events: function (start, end, timezone, callback) {
                    // We will use the fetched holidays, but you may also want to store them locally
                    const events = getStoredEvents();
                    callback(events);  // Fetch events from localStorage
                },
                dayClick: function (date, jsEvent, view) {
                    // Code for day click
                },
                eventClick: function (event, jsEvent, view) {
                    // Code for event click
                },
                viewRender: function (view, element) {
                    // Re-add events from localStorage after view is rendered
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', getStoredEvents());
                }
            });

            // Function to get stored events from localStorage
            function getStoredEvents() {
                return JSON.parse(localStorage.getItem('events')) || [];
            }

            // Function to show event details in a modal
            function showEventDetails(event) {
                $('#event-details-title').text(event.title);
                $('#event-details-time').text(event.start);
                $('#event-details-description').text(event.description);
                $('#event-details-modal').show();
            }

            // Function to close the event form
            function closeForm() {
                $('#event-form').hide();
            }

            // Function to add an event
            function addEvent() {
                const eventName = $('#event-name').val();
                const eventDate = $('#event-date').val();
                const eventTime = $('#event-time').val();
                const eventDescription = $('#event-description').val();
                const isFullDay = $('#event-full-day').prop('checked');

                if (eventName && eventDate && eventDescription) {
                    let eventStart = eventDate;

                    // If not a full day event, add time to the start date
                    if (!isFullDay && eventTime) {
                        eventStart += ' ' + eventTime;
                    }

                    const eventId = new Date().getTime(); // Unique event ID

                    const event = {
                        id: eventId,
                        title: eventName,
                        start: eventStart,
                        description: eventDescription,
                        allDay: isFullDay
                    };

                    // Add event to FullCalendar
                    $('#calendar').fullCalendar('renderEvent', event);

                    // Save event to localStorage
                    saveEventToLocalStorage(event);

                    closeForm(); // Close the event form
                } else {
                    alert('Please fill in all fields');
                }
            }
             

            // Function to save event to localStorage
            function saveEventToLocalStorage(event) {
                let events = JSON.parse(localStorage.getItem('events')) || [];
                events = events.filter(e => e.id !== event.id); // Prevent duplicate events
                events.push(event); // Add the new event
                localStorage.setItem('events', JSON.stringify(events));

                // Re-render the events in all views (month, week, day)
                $('#calendar').fullCalendar('removeEvents'); // Clear existing events
                $('#calendar').fullCalendar('addEventSource', getStoredEvents()); // Add updated events
            }

            // Function to delete an event
            function deleteEvent() {
                if (currentEvent) {
                    // Remove the event from FullCalendar
                    $('#calendar').fullCalendar('removeEvents', currentEvent.id);

                    // Remove the event from localStorage
                    let events = JSON.parse(localStorage.getItem('events')) || [];
                    events = events.filter(event => event.id !== currentEvent.id); // Remove the deleted event
                    localStorage.setItem('events', JSON.stringify(events));

                    // Re-render the events in all views (month, week, day)
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', getStoredEvents());

                    closeDetailsModal(); // Close the event details modal
                }
            }
            // Function to edit an event
            function editEvent() {
                if (currentEvent) {
                    const eventName = $('#event-name').val();
                    const eventDate = $('#event-date').val();
                    const eventTime = $('#event-time').val();
                    const eventDescription = $('#event-description').val();
                    const isFullDay = $('#event-full-day').prop('checked');

                    if (eventName && eventDate && eventDescription) {
                        let eventStart = eventDate;

                        if (!isFullDay && eventTime) {
                            eventStart += ' ' + eventTime;
                        }

                        currentEvent.title = eventName;
                        currentEvent.start = eventStart;
                        currentEvent.description = eventDescription;
                        currentEvent.allDay = isFullDay;

                        $('#calendar').fullCalendar('updateEvent', currentEvent);

                        saveEventToLocalStorage(currentEvent);

                        closeForm();
                    } else {
                        alert('Please fill in all fields');
                    }
                }
            }

            // Function to close the event details modal
            function closeDetailsModal() {
                $('#event-details-modal').hide();
            }

            // Attach event handlers
            $('#addEventBtn').on('click', addEvent);
            $('#editEventBtn').on('click', editEvent);
            $('#closeFormBtn').on('click', closeForm);
            $('#closeDetailsModal').on('click', closeDetailsModal);
            $('#deleteEventBtn').on('click', deleteEvent);
        });

    