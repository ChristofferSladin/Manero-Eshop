document.addEventListener("DOMContentLoaded", function () {
    let startX;
    const radioButtons = document.querySelectorAll('input[name="carousel-radio"]');

    document.querySelector('.carousel').addEventListener('touchstart', function (e) {
        startX = e.touches[0].clientX;
    });

    document.querySelector('.carousel').addEventListener('touchend', function (e) {
        let endX = e.changedTouches[0].clientX;
        let deltaX = startX - endX;

        let currentCheckedIndex;
        radioButtons.forEach((radio, index) => {
            if (radio.checked) {
                currentCheckedIndex = index;
            }
        });

        if (deltaX > 50) { // Swipe left
            let nextIndex = currentCheckedIndex + 1;
            if (nextIndex < radioButtons.length) {
                radioButtons[nextIndex].checked = true;
            }
        } else if (deltaX < -50) { // Swipe right
            let prevIndex = currentCheckedIndex - 1;
            if (prevIndex >= 0) {
                radioButtons[prevIndex].checked = true;
            }
        }
    });
});
