const welcomeText = document.querySelector(".welcomText")
const currentUser = JSON.parse(sessionStorage.getItem('user'))
welcomeText.textContent = `Welcome back ${currentUser.firstName} ${currentUser.lastName}`

function saveUserInSession(user) {
    sessionStorage.setItem('user', JSON.stringify(user))
}

async function updateUser() {
    try {
        const email = document.querySelector("#email").value
        const firstName = document.querySelector("#firstName").value
        const lastName = document.querySelector("#lastName").value
        const password = document.querySelector("#password").value
        let currentUser = JSON.parse(sessionStorage.getItem('user'))
        const id = currentUser.id
        const user = { id, email, firstName, lastName, password }
        const response = await fetch(`api/users/${currentUser.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        if (!response.ok) {
            throw Error("Failed to update profile. Please try again.")
        }
        saveUserInSession(user)
        alert("Profile updated successfully!")
    }
    catch (error) {
        alert("Error: " + error.message)
    }
}
