window.interopFunctions = {
    initToast: function (element) {
        console.log('init', $(element));
        $(element).toast({
            autohide: false,
            animation: true
        });
    },

    showToast: function (element) {
        console.log('show', element, $(element));
        $(element).toast('show');
    },

    hideToast: function (element) {
        $(element).show();
    },

    disposeToast: function (element) {
        $(element).show();
    }
}