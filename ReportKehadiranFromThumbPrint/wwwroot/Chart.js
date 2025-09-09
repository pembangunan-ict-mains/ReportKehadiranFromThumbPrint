window.updateChart = (labels, dataValues) => {
    setTimeout(() => {
        const canvas = document.getElementById('Chart1');
        if (!canvas) {
            console.error("Canvas element not found!");
            return;
        }

        const ctx = canvas.getContext('2d');
        const randomColors = generateRandomColors(dataValues.length);

        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    data: dataValues,
                    backgroundColor: randomColors,
                    borderColor: '#ffffff',
                    borderWidth: 2,
                    hoverOffset: 10
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: { beginAtZero: true },
                    x: { grid: { display: false } }
                },
                plugins: { legend: { display: false } }
            }
        });
    }, 500);
};

//window.updateChart = (labels, dataValues) => {
//    setTimeout(() => {
//        const canvas = document.getElementById('Chart1');
//        if (!canvas) {
//            console.error("Canvas element not found!");
//            return;
//        }

//        const ctx = canvas.getContext('2d');
//        const randomColors = generateRandomColors(dataValues.length);

//        new Chart(ctx, {
//            type: 'doughnut',
//            data: {
//                labels: labels,
//                datasets: [{
//                    data: dataValues,
//                    backgroundColor: randomColors,
//                    borderColor: '#ffffff',
//                    borderWidth: 2,
//                    hoverOffset: 10
//                }]
//            },
//            options: {
//                responsive: true,
//                maintainAspectRatio: false,
//                plugins: {
//                    legend: {
//                        display: true,
//                        position: 'top'
//                    },
//                    tooltip: {
//                        callbacks: {
//                            label: (context) => {
//                                const label = context.label || '';
//                                const value = context.parsed || 0;
//                                return `${label}: ${value.toLocaleString()}`;
//                            }
//                        }
//                    }
//                }
//            }
//        });
//    }, 500);
//};


const generateRandomColors = (count) => {
    const colors = [];
    for (let i = 0; i < count; i++) {
        const color = `hsl(${Math.floor(Math.random() * 360)}, 70%, 60%)`;
        colors.push(color);
    }
    return colors;
};