let navBtns = document.querySelectorAll('.container-nav-menu-content a:not(:last-child)');

navBtns.forEach(btn => {
    btn.addEventListener('mousedown', function (e) {
        let x = e.clientX - e.target.offsetLeft;
        let y = e.clientY - e.target.offsetTop;

        let ripple = document.createElement('span');

        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';

        ripple.classList.add('ripple');

        this.appendChild(ripple);

        setTimeout(() => {
            ripple.remove();
        }, 1000);
    });
})