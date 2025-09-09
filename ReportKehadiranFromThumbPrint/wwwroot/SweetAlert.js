function loadSweetAlert() {
    // Create a script element
    var script = document.createElement('script');
    script.src = '_content/CurrieTechnologies.Razor.SweetAlert2/sweetAlert2.min.js';
    script.onload = function () {
        console.log('SweetAlert loaded successfully');
    };
    document.head.appendChild(script);
}
