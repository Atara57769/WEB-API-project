function displyExistUser() {
    const existUser = document.querySelector(".existUser")
    existUser.style.display="flex"
}
function saveUserInSession(user) {
    sessionStorage.setItem('user', JSON.stringify(user))
}
async function getResponse() {
    try {
        const response = await fetch('api/users')
        if (!response.ok) {
            throw new Error("error")
        }
        else {
            const data = await response.json()
            alert(data)
        }
    }
    catch (error) {
        alert(error)
    }
}
async function addUser() {
    try {
        const email = document.querySelector("#email").value
        const firstName = document.querySelector("#firstName").value
        const lastName = document.querySelector("#lastName").value
        const password = document.querySelector("#password").value
        const user = { email, firstName, lastName, password }
        const response= await fetch('api/users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)

        });
        if (!response.ok) {
            throw Error("Run into a problem")
        }
        const data = await response.json();
        alert(" Sign in successfully")
    }
    catch (error) {
        alert(error)
    }
}

async function Login() {
    try {
        const email = document.querySelector("#emailLogin").value
        const password = document.querySelector("#passwordLogin").value
        const LoginUser = { email, password}
        const response = await fetch('api/users/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(LoginUser)
        });
        if (!response.ok) {
            throw Error("error")
        }
        if (response.status == 204) {
            alert("Email or password incorrect! please try again ")
            return
        }
            const data = await response.json();
            saveUserInSession(data)
            window.location.href = "update.html"
    }
    catch (error) {
        alert(error)
    }
}