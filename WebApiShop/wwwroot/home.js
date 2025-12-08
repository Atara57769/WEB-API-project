function displayExistUser() {
    const existUser = document.querySelector(".existUser")
    existUser.style.display = "flex"
}

function saveUserInSession(user) {
    sessionStorage.setItem('user', JSON.stringify(user))
}

async function getUsers() {
    try {
        const response = await fetch('api/users')
        if (!response.ok) {
            throw new Error("Failed to retrieve users")
        }
        else {
            const data = await response.json()
            alert(data)
        }
    }
    catch (error) {
        alert("Error: " + error.message)
    }
}

async function addUser() {
    try {
        const email = document.querySelector("#email").value
        const firstName = document.querySelector("#firstName").value
        const lastName = document.querySelector("#lastName").value
        const password = document.querySelector("#password").value
        const user = { email, firstName, lastName, password }
        const response = await fetch('api/users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        
        if (!response.ok) {
            throw Error("Failed to create account. Please try again")
        }
        const data = await response.json()
        alert("Account created successfully!")
    }
    catch (error) {
        alert("Error: " + error.message)
    }
}

async function login() {
    try {
        const email = document.querySelector("#emailLogin").value
        const password = document.querySelector("#passwordLogin").value
        const loginUser = { email, password }
        const response = await fetch('api/users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginUser)
        })
        
        if (!response.ok) {
            if (response.status === 401) {
                alert("Login failed: Incorrect email or password. Please try again.")
                return
            }
            throw Error("Login failed. Please try again later.")
        }
        
        const data = await response.json()
        saveUserInSession(data)
        window.location.href = "update.html"
    }
    catch (error) {
        alert("Error: " + error.message)
    }
}

async function checkPasswordScore() {
    try {
        const password = document.querySelector("#password").value
        const progress = document.querySelector("#passwordScore")
        const response = await fetch('api/passwords/passwordScore', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(password)
        })

        if (!response.ok) {
            throw Error("Failed to check password strength")
        }
        const data = await response.json()
        progress.value = data * 25
    }
    catch (error) {
        alert("Error: " + error.message)
    }
}
