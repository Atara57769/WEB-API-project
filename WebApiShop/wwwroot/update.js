const welcomText = document.querySelector(".welcomText")
const curretUser = JSON.parse(sessionStorage.getItem('user'))
welcomText.textContent = `welcome back ${curretUser.firstName} ${curretUser.lastName}`
function saveUserInSession(user) {
    sessionStorage.setItem('user', JSON.stringify(user))
}
async function updateUser() {
    try {
        const email = document.querySelector("#email").value
        const firstName = document.querySelector("#firstName").value
        const lastName = document.querySelector("#lastName").value
        const password = document.querySelector("#password").value
        let currentUser =JSON.parse( sessionStorage.getItem('user'))
        const id = currentUser.id
        const user = { id, email, firstName, lastName, password }
        const response = await fetch(`api/users/${currentUser.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)

        });
        if (!response.ok) {
            throw Error("run into a problem")
        }
        saveUserInSession(user)
        alert("update sucessfly")
    }
    catch (error) {
        alert(error)

    }
}