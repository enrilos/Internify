function getEmail(id) {

    let btn = document.querySelector('#email-btn');

    btn.textContent = "Loading...";

    fetch(`/api/Candidate/${id}`)
        .then(res => res.text())
        .then(email => (() => {
            const paragraph = document.createElement('p');
            paragraph.textContent = `Email: ${email}`
            btn.parentElement.appendChild(paragraph);
            btn.remove();
        })())
        .catch(err => console.error(err));
}