import axios from "axios";


const instance = axios.create({
    baseURL: "https://localhost:44424/api",
    headers: {
        "Content-Type": "application/json"
    }
});

instance.interceptors.request.use(
    (config) => {
        const token = JSON.parse(localStorage.getItem("user")).jwtToken;
        if (token) {
            config.headers["Authorization"] = 'Bearer ' + token;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
instance.interceptors.response.use(
    (res) => {
        return res;
    },
    async (err) => {
        const originalConfig = err.config;
        console.log('loop');
        console.log(originalConfig);
        console.log(err.response);
        if (originalConfig.url !== "/User/authenticate" && err.response) {
            //token expired
            if (err.response.status === 401 && !originalConfig._retry) {
                //this flag should catch infinite loops
                originalConfig._retry = true;
                try {
                    const rs = await instance.post("/User/refresh-token")
                    if (rs.data.jwtToken)
                        localStorage.setItem("user", JSON.stringify(rs.data));
                    return instance(originalConfig);
                }
                catch (error) {
                    return Promise.reject(error);
                }
            }
        }
        return Promise.reject(err);
    }

);

export default instance;