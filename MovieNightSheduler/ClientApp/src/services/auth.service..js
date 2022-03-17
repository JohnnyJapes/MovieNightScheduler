import axios from "axios";
const API_URL = "https://localhost:44324/api/User";
class AuthService {
    login(username, password) {
        return axios
            .post(API_URL + "authenticate", {
                username,
                password
            })
            .then(response => {
                if (response.data.jwtToken)
                    localStorage.setItem("user", JSON.stringify(response.data));
                return response.data;

            });

    }
    logout() {
        localStorage.removeItem("user");
    }
    register(username, password) {
        return axios.post(API_URL + "register", {
            username,
            password
        })
    }
    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user'));
    }
}