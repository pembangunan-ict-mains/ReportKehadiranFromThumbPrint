window.hideModal = (selector) => {
    const modalElement = document.querySelector(selector);
    if (modalElement) {
        const modalInstance = bootstrap.Modal.getOrCreateInstance(modalElement);
        modalInstance.hide();
    }
};
